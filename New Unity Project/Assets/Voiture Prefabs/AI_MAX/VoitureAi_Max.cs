using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoitureAi_Max : MonoBehaviour
{
    //Pour pathFinding
    public Transform cible;                  //Position du checkpoint Actuelle
    int compteurCheckpoints;                  //compteur de checkpoint
    Vector3[] path;                           //Chemin renvoyer par PathFinding qui est géré par PathRequestManager
    int targetIndex;                          //Index du target
    private List<Transform> checkpoints = new List<Transform>(); //
    static Vector3 pointcourrant;             //Point de passage actuelle dans le chemin pour se rendre au Checkpoint (qui est le pt passage finale du path
                                              
    //Pour ControlVoiture                     
    public float angleTournerMax = 25;        //angle maximale des roues
    public WheelCollider roueAD;              //WheelCollider de la roue avant droite
    public WheelCollider roueAG;              //WheelCollider de la roue avant gauche
    public WheelCollider roueDD;              //WheelCollider de la roue arrière droite
    public WheelCollider roueDG;              //WheelCollider de la roue arrière gauche
    public Transform transformRoueAD;         //position de la roue avant droite
    public Transform transformRoueAG;         //position de la roue avant gauche
    public Transform transformRoueDD;         //position de la roue arrière droite
    public Transform transformRoueDG;         //position de la roue arrière gauche
    private float coupleMoteurAdaptatif;      //force/couple moteur qui change selon la distance du pointcourrant
    private float brakeTorqueAdaptatif;       //force de freinage qui change selon la vitesse et la distance du pointcourrant
    float directionRoues;                     //direction des roues
    private Vector3 orientation;              //orientation de si avance ou recule
    public static float vitesse = 0f;         //vitesse de la voiture
    public float forceAntiFlip = 100f;        //force empêchant que la voiture se renverse et s'adaptant selon la vitesse
    public Transform transformJoueur;         //transforme (position dans le monde) du joueur
    public PositionVoiture positionAI;                   //position dans la course de l'IA
    public PositionVoiture positionJoueur;                //position dans la course du joueur
    public bool roueTourneMax = false;        //Les roue tourne à l'angle maximum
    public bool atteintCheckPoint = false;    //A atteint un checkpoint

    
    


    //Pour les sensors a l'avant (raycast)
    public Transform positionCarrosserie;     //position du « body », de la carosserie
    public float longueurRayons = 5f;         //Longueur des rayCast qui permet la détection de mur/voiture/sol
    public Vector3 positionRayonAvant = new Vector3(0, 0, 0); //position du rayon avant
    public static bool doitReculer = false;   //bool permetant de savoir si la voiture doit reculer, est changé lorsque le rayon avant touche a un mur
    public Rigidbody rb;                      //Rigidbody de la voiture (composant affecté par la physique de Unity)
    public bool estSurCapot = false;          //bool permetant de savoir si la voiture doit est remis sur ses roues, est changé lorsque le rayon sur le toit touche au sol
    public int hauteurReset = 1;              //Hauteur du sol auquel la voiture est remis sur ses roue



    //
    //SECTION SUIVRE CHEMIN
    //
    private void Awake()
    {
        //associe la rigidbody de la voiture at rb
        rb = GetComponent<Rigidbody>();
        
    }

    void Start()
    {

        compteurCheckpoints = 0;
        //Permet d'obtenir toutes les position des checkpoints qui sont les enfants de l'objet A*_Max
        Transform[] tableauCheckpoints = GameObject.Find("A*_Max").GetComponentsInChildren<Transform>();
        
        for (int i = 1; i < tableauCheckpoints.Length; i++)
        {
            checkpoints.Add(tableauCheckpoints[i]);
        }

        //comence en envoyant le premier checkpoints
        PathRequestManager_Max.RequestPath(transform.position, checkpoints[0].position, OnPathFound);
        compteurCheckpoints++;
    }

    private void Update()
    {
        //changing tyre direction
        Vector3 temp = transformRoueAG.localEulerAngles;
        Vector3 temp1 = transformRoueAD.localEulerAngles;
        temp.y = roueAG.steerAngle - (transformRoueAG.localEulerAngles.z);
        transformRoueAG.localEulerAngles = temp;
        temp1.y = roueAD.steerAngle - transformRoueAD.localEulerAngles.z;
        transformRoueAD.localEulerAngles = temp1;

        Change_Position_Roue(roueAG, transformRoueAG);
        Change_Position_Roue(roueAD, transformRoueAD);
        Change_Position_Roue(roueDG, transformRoueDG);
        Change_Position_Roue(roueDD, transformRoueDD);
    }

    //Si le chemin est trouvé (possible de s'y rendre) associe le chemin trouver a path et 
    //commence FollowPath qui s'occupe de la gestion du chemin
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    //Coroutine qui permet la gestion et du tracking du chemin suivie
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        pointcourrant = currentWaypoint;
        while (true)
        {
            //Si la distance entre le prochain point a aller est plus petite que 7, change le prochain point à aller ou si la distance entre le point du moment et le point après est plus grande que
            // la distance entre le voiture et le point d'après, alors change le point actuelle pour le point d'après
            if (Vector3.Distance(transform.position, currentWaypoint) <= 7 || Vector3.Distance(transform.position, path[targetIndex + 1]) < Vector3.Distance(currentWaypoint, path[targetIndex + 1])
                || Vector3.Distance(transform.position, currentWaypoint) >= Vector3.Distance(transform.position, path[targetIndex + 1]))
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                pointcourrant = currentWaypoint;
            }

            yield return null;
        }
    }

    //Permet, en mode développeur de voir les point de passage et les lignes entre-eux (chemin a suivre) 
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    //
    // SECTION CONTROL VOITURE
    //
    private void FixedUpdate()
    {

        ForceVersLeBas();
        orientation = rb.transform.InverseTransformDirection(rb.velocity);
        vitesse = Mathf.Abs(orientation.z * 3.6f);
        float distance = Vector3.Distance(transform.position, cible.transform.position);

        //Si la distance entre le checkpoint (sphère) est plus petit que 15 et que le checkpoint à été atteint (collider), change pour le prochain checkpoint
        if (distance < 40 /*&& atteintCheckPoint == true*/)
        {
            CheckPointMax.atteintCheckPoint = false;
            //Si le compteur = au nombre de checkpoint, alors il a atteint le dernier checkpoint et change pour le permier
            //permet de recommencer le trajet à l'infini
            if (compteurCheckpoints == checkpoints.Count)
            {
                compteurCheckpoints = 0;
                cible = checkpoints[compteurCheckpoints];
                PathRequestManager_Max.RequestPath(transform.position, cible.position, OnPathFound);

                compteurCheckpoints++;
            }
            //au sinon le checkpoint est changé pour le suivant
            else
            {
                cible = checkpoints[compteurCheckpoints];
                PathRequestManager_Max.RequestPath(transform.position, cible.position, OnPathFound);

                compteurCheckpoints += 1;
            }
        }

        Sensors();
        //Si doitReculer est true, alors les roues tourne en sens inverse pour faire reculer
        //et l'angle des roues est de 0 pour reculer en ligne droite
        if (doitReculer)
        {

            roueAD.motorTorque = -1500;
            roueAG.motorTorque = -1500;
            roueDD.motorTorque = -1500;
            roueDG.motorTorque = -1500;
            roueAD.steerAngle = 0;
            roueAG.steerAngle = 0;

        }
        //si estSurCapot est true, alors arrêt complet des roues pour empêcher de garder
        //leur « momentum » et que lorsque la voiture, assure que tout est « réinitialiser »
        else if (estSurCapot)
        {

            roueAD.motorTorque = 0;
            roueAG.motorTorque = 0;
            roueDD.motorTorque = 0;
            roueDG.motorTorque = 0;
            roueAD.brakeTorque = 5000f;
            roueAG.brakeTorque = 5000f;
            roueDD.brakeTorque = 5000f;
            roueDG.brakeTorque = 5000f;


        }
        //Sinon, s'occupe de suivre le chemin normalement
        else
        {
            Avancer(pointcourrant);
            Tourner(pointcourrant);
            Braker(pointcourrant);

            
        }

    }

    //Fonction qui s'occupe de tourner les roue selon la postion de la voiture et la position du point de passage
    public void Tourner(Vector3 currentWaypoint)
    {
        Vector3 vecteurRelatif = transform.InverseTransformPoint(currentWaypoint);
        vecteurRelatif = vecteurRelatif / vecteurRelatif.magnitude;
        directionRoues = (vecteurRelatif.x / vecteurRelatif.magnitude) * angleTournerMax;



        roueAD.steerAngle = directionRoues;
        roueAG.steerAngle = directionRoues;

        if (directionRoues >= angleTournerMax-2 || directionRoues <= -angleTournerMax + 2)
        {
            roueTourneMax = true;
        }
        else
        {
            roueTourneMax = false;
        }
    }

    //Fonction qui s'occupe de faire avancer la voiture. Plus la distance est grande entre le point de passage
    //alors plus la force moteur est grande. 
    public void Avancer(Vector3 checkpoint)
    {
        
       
        coupleMoteurAdaptatif = Mathf.Sqrt(10000f * Vector3.Distance(transform.position, checkpoint));

        //si ex l'IA est derrière le joueur et est a une distance supérieur à 75 entre les deux, alors l'IA 
        //voit sa force moteur augmenter et si au contraire, l'IA est en en avant du joueur avec une distance supérieur a 75,
        //alors sa force moteur se voit réduite
        if (Vector3.Distance(transform.position, transformJoueur.position) >= 75 && positionAI.position < positionJoueur.position)
        {
            coupleMoteurAdaptatif += 200;
        }
        else if (Vector3.Distance(transform.position, transformJoueur.position) >= 75 && positionAI.position > positionJoueur.position)
        {
            coupleMoteurAdaptatif -= 200;
            if (coupleMoteurAdaptatif <= 0)
            {
                coupleMoteurAdaptatif = Mathf.Sqrt(10000f * Vector3.Distance(transform.position, checkpoint));
            }
        }
        

        roueAD.motorTorque = coupleMoteurAdaptatif;
        roueAG.motorTorque = coupleMoteurAdaptatif;
        roueDD.motorTorque = coupleMoteurAdaptatif;
        roueDG.motorTorque = coupleMoteurAdaptatif;
    }

    //S'occupe de faire freiner la voiture selon quelque condition comme la vitesse et la distance 
    //pour permettre de mieux prendre la majorité des virages
    public void Braker(Vector3 checkpoint)
    {
        if (Vector3.Distance(transform.position, checkpoint) <= 20 && vitesse >= 85)
        {
            brakeTorqueAdaptatif = 5000;
            

        }
        else if (Vector3.Distance(transform.position, checkpoint) <= 30 && vitesse >= 100)
        {
            brakeTorqueAdaptatif = 6250;
        }
        else if (Vector3.Distance(transform.position, checkpoint) <= 40 && vitesse >= 120)
        {
            brakeTorqueAdaptatif = 7500;
        }
        else
        {
            brakeTorqueAdaptatif = 0;
        }

        if (roueTourneMax == true && vitesse > 30)
        {
            brakeTorqueAdaptatif += 1000;
            
        }

        roueAD.brakeTorque = brakeTorqueAdaptatif;
        roueAG.brakeTorque = brakeTorqueAdaptatif;
        roueDD.brakeTorque = brakeTorqueAdaptatif;
        roueDG.brakeTorque = brakeTorqueAdaptatif;
    }

    //Gère les RayCast (rayons) pour la détection de mur et du sol pour faire freiner et
    //remettre la voiture sur ses roues.
    private void Sensors()
    {
        Vector3 positionSensorsReculer = positionCarrosserie.position;
        Vector3 positionSensorsAntiFlip = positionCarrosserie.position;
        positionSensorsReculer += positionCarrosserie.forward * positionRayonAvant.z;

        //Sensor AvantMilieu, si le tag est autre chose que Terrain, Arrive, ou checkpointMax, alors doitReculer = true
        if (Physics.Raycast(positionSensorsReculer, positionCarrosserie.forward, out RaycastHit hit, longueurRayons))
        {
            if (hit.collider.tag != "Terrain" && hit.collider.tag != "Arrive" && hit.collider.tag != "checkpointMax" && hit.collider.tag != "waypoints")
            {
                Debug.Log(hit.collider.name);
                doitReculer = true;
                StopCoroutine("ReculerPendentXSec");
                StartCoroutine("ReculerPendentXSec");
            }

        }
        Debug.Log("doitReculer ?" + doitReculer);
        Debug.DrawLine(positionSensorsReculer, hit.point);

        positionSensorsAntiFlip += positionCarrosserie.up * 1f;

        //Sensor sur le toit permettant de remettre la voiture sur ses roues, si le rayon touche Asphalte ou Terrain
        //Alors active est sur le capot en commence la Coroutine AttendreAvantReset
        if (Physics.Raycast(positionSensorsAntiFlip, positionCarrosserie.up, out RaycastHit hitCapot, longueurRayons))
        {
            if (hitCapot.collider.CompareTag("Asphalte") || hitCapot.collider.CompareTag("Terrain"))
            {
                estSurCapot = true;
                StopCoroutine("AttendreAvantReset");
                StartCoroutine("AttendreAvantReset");

            }

            Debug.Log("estSurCapot ?" + estSurCapot);
            Debug.DrawLine(positionSensorsAntiFlip, hitCapot.point);
        }

    }


    //Coroutine AttendreAvantReset qui attend 2 secondes avant de remmetre la voiture sur ses roues
    IEnumerator AttendreAvantReset()
    {

        yield return new WaitForSeconds(2f);
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
        rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
        estSurCapot = false;

    }

    //Coroutine ReculerPendentXSec qui garde doitReculer a true pendant 1 seconde, obligeant a reculer pendant cette duré
    IEnumerator ReculerPendentXSec()
    {

        yield return new WaitForSeconds(1f);
        doitReculer = false;

    }

    //fonction qui permet de garder la voiture au sol et de réduire les chances de chavirer
    private void ForceVersLeBas()
    {
        roueAD.attachedRigidbody.AddForce(-transform.up * forceAntiFlip * roueAD.attachedRigidbody.velocity.magnitude);
    }

    //Permet l'animation des roues
    private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.Rotate(roueAD.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        ForceVersLeBas();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "checkpointMax")
        {
            atteintCheckPoint = true;
        }
    }
}