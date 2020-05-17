using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarControllerE : MonoBehaviour
{
    //[Header("Controller")] 
    //public bool playerControlled;               //Variable qui permet de controllé le véhicule avec WASD.       ***Variables abandonnées pour l'instant puisque chaque AI à sont prorpes prefab 
    //public bool elianControlled;                //Variable qui permet de controllé le véhicule AI élian.     ------> Toujours vrai puisque ce prefab est Élian AI 
    //public bool felixControlled;                //Variable qui permet de controllé le véhicule AI felix. 
    //public bool maximeControlled;               //Variable qui permet de controllé le véhicule AI maxime. 
    //public bool jonathanControlled;             //Variable qui permet de controllé le véhicule AI jonathan. 


    [Header("Colliders/Car Stuff")]
    public WheelCollider Wheel_Collider_FL;     //Wheel collider roue avant gauche. 
    public WheelCollider Wheel_Collider_FR;     //Wheel collider roue avant droit. 
    public WheelCollider Wheel_Collider_RL;     //Wheel collider roue derriere gauche. 
    public WheelCollider Wheel_Collider_RR;     //Wheel collider roue derriere droit. 

    public Transform Wheel_Transformation_FL;   //Wheel collider roue avant gauche. 
    public Transform Wheel_Transformation_FR;   //Wheel collider roue avant droit. 
    public Transform Wheel_Transformation_RL;   //Wheel collider roue derriere gauche. 
    public Transform Wheel_Transformation_RR;   //Wheel collider roue derriere droit. 
    private Rigidbody rb;                       //RigidBody de la voiture. 

    private Vector3 orientation;                //Orientation du véhicule. 

    public static float vitesse;                //Vitesse du véhicule. 


    //Variables de base du véhicule. 
    [Header("Driving Variables")]
    public float steeringAngle;                 //Angle de direction maximum de base. 
    public float maxBrakeTorque = 1000;         //Torque de freinage maximum de base. 
    public float maxTorque = 1000;              //Torque de d'accéleration maximum de base. 
    private float tempSteeringAngle;            //Angle de direction temporaire afin de controller le véhicule. 
    public float hauteurReset;                  //Hauteur de véhicule par rapport au sol quand le joueur reset celui-ci. 
    public float forceAntiFlip = 100;           //Force qui permet à la voiture de ne pas faire de tonneau. 


    //Variables de l'intélligence artificiel Élian AI. 
    [Header("Élian AI")]
    public Transform waypoints;                 //Transform du parent des points de cheminement. 
    private bool isColliding = false;           //Variable qui indique si le véhicule est en contact avec un point de cheminement.    
    private GameObject waypointBox;             //Collider du point de cheminement pour chaque sommet A*. 
    private Vector3[] path;                     //Array qui contient chaque sommet du chemin le plus rapide.   
    private int targetIndex;                    //Garde en mémoire où est rendu le véhicule dans les sommets du chemin le plus rapide.   
    private int waypointIndex;                  //Garde en mémoire où est rendu le véhicule dans les point de cheminement sur la piste.    
    private bool reachedEndPath;                //Variable qui indique si le véhicule est à la fin du chemin le plus rapide. 
    private float eulerAnglesY = 0;             //Angle de la voiture en Y lorsque la voiture passe un sommet dans le chemin le plus rapide.      
    public float maxSteeringAngle = 25;         //Angle maximum de direction pour intélligence artificiel Élian AI. 

    public static bool BoutonChanger = false;   //Variable pour inéragir avec les boutons 

    //Fonction appeler en premier lorsque le jeux est sur jouer. 
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Initialise le collider pour le point de passage des sommets du chemin le plus rapide. 
        waypointBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        waypointBox.GetComponent<BoxCollider>().isTrigger = true;
        waypointBox.tag = "waypointsE";

        //Commence la coroutine seulement si "Waypoints" n'est pas vide afin d'éviter des erreurs. 
        if (waypoints != null)
        {
            waypointIndex = 0;
            PathRequesterE.RequestPath(transform.position, waypoints.Find(waypointIndex.ToString()).position, OnPathFound);
        }




    }

    //Fonction qui s'exécute quand le chemin le plus rapide est trouvé ou pas. 
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        //Commence la coroutine si un chemin est trouvé. 
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("elianControlledScript");
            StartCoroutine("elianControlledScript");
        }

    }

    //Fonction qui se répete plusieurs fois par seconde. 
    IEnumerator elianControlledScript()
    {
        //Initialisations.... 
        Vector3 currentWaypoint = path[targetIndex];
        waypointBox.transform.localScale = new Vector3(140, 10, 0.5f);
        waypointBox.transform.position = currentWaypoint;
        waypointBox.layer = LayerMask.NameToLayer("Ignore Raycast");
        Destroy(waypointBox.GetComponent<MeshRenderer>());

        //Boucle dans laquel tout le contrôle du véhicule se produit 
        while (true)
        {
            //Si le véhicule touche un point de passage dans les sommets du chemin le plus court... 
            if (isColliding == true)
            {
                targetIndex++;
                eulerAnglesY = transform.eulerAngles.y;

                //Si le véhicule atteint le dernier sommet dans les sommets du chemin le plus court... 
                if (targetIndex >= path.Length)
                {
                    waypointIndex++;
                    //Si le prochain point de passage  dans la liste des points existe on continue... 
                    if (waypoints.Find(waypointIndex.ToString()) != null)
                    {
                        Debug.Log("Élian AI: Next Waypoint Found!");
                        PathRequesterE.RequestPath(transform.position, waypoints.Find(waypointIndex.ToString()).position, OnPathFound);
                        yield break;
                    }
                    //Si arriver à la fin du circuit, on recommence donc tout à 0 et on refait le circuit... 
                    else if (waypointIndex == waypoints.childCount)
                    {
                        waypointIndex = 0;
                        targetIndex = 0;
                        PathRequesterE.RequestPath(transform.position, waypoints.Find(waypointIndex.ToString()).position, OnPathFound);
                        yield break;
                    }
                }
                //On fait apparaitre la boite invisible du point de passage sur le sommet cible. 
                currentWaypoint = path[targetIndex];
                waypointBox.transform.position = currentWaypoint;
            }
            //Initialisations Raycast 
            RaycastHit hit;
            Vector3 sensorBasePos = transform.position;
            sensorBasePos += transform.forward * 2.30f;
            sensorBasePos += transform.up * 0.9f;
            bool stuck = false;
            bool attacking = false;
            bool stuckOnTop = false;


            //Angle relatif 
            Vector3 relativeVector = transform.InverseTransformPoint(currentWaypoint);

            //Mesure de l'angle des roue pour atteindre le sommet cible. 
            float newSteerAngle = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
            Wheel_Collider_FL.steerAngle = newSteerAngle;
            Wheel_Collider_FR.steerAngle = newSteerAngle;

            //Change l'angle du point de passage sur le prochain sommet du chemin le plus rapide. 
            waypointBox.transform.eulerAngles = new Vector3(0, eulerAnglesY, 0);

            //Mesure de l'orientation (angle) du véhicule 
            orientation = transform.InverseTransformDirection(rb.velocity);

            //Mesure de la vitesse du véhicule 
            vitesse = orientation.z * 3.6f;

            //Calcule l'angle des roues... 
            if (Mathf.Abs(vitesse) < 50)
            {
                tempSteeringAngle = (-(Mathf.Abs(vitesse) / 5) + maxSteeringAngle);
            }
            else
            {
                tempSteeringAngle = 15;
            }

            //Raycast pour detecter si le véhicule est pris dans un mur ou autre objet. 
            if (Physics.Raycast(sensorBasePos, transform.forward, out hit, 5f) && vitesse < 2.0f)
            {
                stuck = true;
                Wheel_Collider_FL.motorTorque = -maxTorque;
                Wheel_Collider_FR.motorTorque = -maxTorque;
                Wheel_Collider_RR.motorTorque = -maxTorque;
                Wheel_Collider_RL.motorTorque = -maxTorque;
                Wheel_Collider_FL.brakeTorque = 0;
                Wheel_Collider_FR.brakeTorque = 0;
                Wheel_Collider_RL.brakeTorque = 0;
                Wheel_Collider_RR.brakeTorque = 0;
                Wheel_Collider_FL.steerAngle = -1 * newSteerAngle;
                Wheel_Collider_FR.steerAngle = -1 * newSteerAngle;
            }
            //Raycast pour detecter si un autre véhicule est à droite. 
            if (Physics.Raycast(sensorBasePos, Quaternion.AngleAxis(90f, transform.up) * transform.forward, out hit, 5f, 1 << 15) && stuck == false)
            {
                attacking = true;
                Wheel_Collider_FL.motorTorque = maxTorque;
                Wheel_Collider_FR.motorTorque = maxTorque;
                Wheel_Collider_RR.motorTorque = maxTorque;
                Wheel_Collider_RL.motorTorque = maxTorque;
                Wheel_Collider_FL.brakeTorque = 0;
                Wheel_Collider_FR.brakeTorque = 0;
                Wheel_Collider_RL.brakeTorque = 0;
                Wheel_Collider_RR.brakeTorque = 0;
                Wheel_Collider_FL.steerAngle = maxSteeringAngle;
                Wheel_Collider_FR.steerAngle = maxSteeringAngle;
            }
            //Raycast pour detecter si un autre véhicule est à gauche. 
            if (Physics.Raycast(sensorBasePos, Quaternion.AngleAxis(-90f, transform.up) * transform.forward, out hit, 5f, 1 << 15) && stuck == false)
            {
                attacking = true;
                Wheel_Collider_FL.motorTorque = maxTorque;
                Wheel_Collider_FR.motorTorque = maxTorque;
                Wheel_Collider_RR.motorTorque = maxTorque;
                Wheel_Collider_RL.motorTorque = maxTorque;
                Wheel_Collider_FL.brakeTorque = 0;
                Wheel_Collider_FR.brakeTorque = 0;
                Wheel_Collider_RL.brakeTorque = 0;
                Wheel_Collider_RR.brakeTorque = 0;
                Wheel_Collider_FL.steerAngle = -1 * maxSteeringAngle;
                Wheel_Collider_FR.steerAngle = -1 * maxSteeringAngle;
            }
            //Raycast pour detecter si le véhicule est sur son toit.
            if (Physics.Raycast(sensorBasePos, transform.up, out hit, 5f) && vitesse < 0.5f)
            {
                stuckOnTop = true;
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
                rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);

            }


            //Commandes pour gérer la vitesse selon plusieurs facteurs dont la disctance du prochain sommet, l'angle des roues, etc. 
            if (Vector3.Distance(transform.position, currentWaypoint) >= 10 && Vector3.Distance(transform.position, currentWaypoint) <= 30 && stuck == false && attacking == false && vitesse >= 50 && vitesse <= 60)
            {
                Wheel_Collider_FL.motorTorque = 0;
                Wheel_Collider_FR.motorTorque = 0;
                Wheel_Collider_RR.motorTorque = 0;
                Wheel_Collider_RL.motorTorque = 0;
                Wheel_Collider_FL.brakeTorque = 0;
                Wheel_Collider_FR.brakeTorque = 0;
                Wheel_Collider_RL.brakeTorque = 0;
                Wheel_Collider_RR.brakeTorque = 0;

            }
            else if (Vector3.Distance(transform.position, currentWaypoint) >= 0 && Vector3.Distance(transform.position, currentWaypoint) <= 45 && stuck == false && attacking == false && vitesse > 65)
            {
                Wheel_Collider_FL.motorTorque = 0;
                Wheel_Collider_FR.motorTorque = 0;
                Wheel_Collider_RR.motorTorque = 0;
                Wheel_Collider_RL.motorTorque = 0;
                Wheel_Collider_FL.brakeTorque = maxBrakeTorque;
                Wheel_Collider_FR.brakeTorque = maxBrakeTorque;
                Wheel_Collider_RL.brakeTorque = maxBrakeTorque;
                Wheel_Collider_RR.brakeTorque = maxBrakeTorque;
            }
            else if (newSteerAngle > 23 && stuck == false && attacking == false && vitesse > 10)
            {
                Wheel_Collider_FL.motorTorque = 0;
                Wheel_Collider_FR.motorTorque = 0;
                Wheel_Collider_RR.motorTorque = 0;
                Wheel_Collider_RL.motorTorque = 0;
                Wheel_Collider_FL.brakeTorque = maxBrakeTorque;
                Wheel_Collider_FR.brakeTorque = maxBrakeTorque;
                Wheel_Collider_RL.brakeTorque = maxBrakeTorque;
                Wheel_Collider_RR.brakeTorque = maxBrakeTorque;
            }
            else if (stuck == false && attacking == false)
            {
                Wheel_Collider_FL.motorTorque = maxTorque;
                Wheel_Collider_FR.motorTorque = maxTorque;
                Wheel_Collider_RR.motorTorque = maxTorque;
                Wheel_Collider_RL.motorTorque = maxTorque;
                Wheel_Collider_FL.brakeTorque = 0;
                Wheel_Collider_FR.brakeTorque = 0;
                Wheel_Collider_RL.brakeTorque = 0;
                Wheel_Collider_RR.brakeTorque = 0;
            }
            yield return null;
        }
    }

    //Fonction qui s'éxecute quand la voiture entre dans un Collider type Trigger. 
    public void OnTriggerEnter(Collider col)
    {
        string tag = col.gameObject.tag;

        //Si le trigger à le bon tag il entre en collision avec le point de passage sommet du chemin le plus court. 
        if ("waypointsE" == tag)
        {
            isColliding = true;
        }
    }

    //Fonction qui s'éxecute quand la voiture sort d'un Collider type Trigger. 
    void OnTriggerExit(Collider col)
    {
        string tag = col.gameObject.tag;

        //Si le trigger à le bon tag il arrete d'être en collision avec le point de passage sommet du chemin le plus court. 
        if ("waypointsE" == tag)
        {
            isColliding = false;
        }
    }

    //Fonction qui affiche des objets dans l'éditeur Unity. 
    public void OnDrawGizmos()
    {
        //Affiche des cubes sur les sommets du chemin le plus court et dessine les aretes entre les sommets. 
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

    //Fonction qui s'éxecute quand la voiture entre en collision avec un autre GameObject qui a un collider et reste en collision. 
    private void OnCollisionStay(Collision collision)
    {
        //Permet de savoir si le véhicule est sur l'herbe, la route, ou autre... 
        if (collision.gameObject.tag == "Herbe" || collision.gameObject.tag == "Rampe" || collision.gameObject.tag == "Asphalte")
        {
            //Permet de reset la voiture avec la touche "R". 
            if (Input.GetKeyDown(KeyCode.R))
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
                rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
            }
        }
    }

    //Fonction qui s'éxecute quand la voiture entre en collision avec un autre GameObject qui a un collider. 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BoutonChanger")
        {
            BoutonChanger = true;
        }
    }

    //Focntion qui s'actualise plusieurs fois par seconde. 
    void FixedUpdate()
    {
        //Change le torque maximum en focntion de la vitesse du véhicule 
        maxTorque = (-1f / 25f) * Mathf.Pow(vitesse, 2) + 1000;
    }

    //Focntion qui s'actualise plusieurs fois par seconde. 
    void Update()
    {
        //Change la direction des roues visuellement. 
        Vector3 temp = Wheel_Transformation_FL.localEulerAngles;
        Vector3 temp1 = Wheel_Transformation_FR.localEulerAngles;
        temp.y = Wheel_Collider_FL.steerAngle - (Wheel_Transformation_FL.localEulerAngles.z);
        Wheel_Transformation_FL.localEulerAngles = temp;
        temp1.y = Wheel_Collider_FR.steerAngle - Wheel_Transformation_FR.localEulerAngles.z;
        Wheel_Transformation_FR.localEulerAngles = temp1;

        Change_Position_Roue(Wheel_Collider_FL, Wheel_Transformation_FL);
        Change_Position_Roue(Wheel_Collider_FR, Wheel_Transformation_FR);
        Change_Position_Roue(Wheel_Collider_RL, Wheel_Transformation_RL);
        Change_Position_Roue(Wheel_Collider_RR, Wheel_Transformation_RR);
    }

    //Focntion qui change la direction des roues visuellement et applique la force vers le bas (anti-flip). 
    private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.Rotate(Wheel_Collider_FL.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        ForceVersLeBas();

    }

    //Plus la voiture va vite, plus il y a un grande force d'appliquer sur la voiture pour l'empecher de se renverser. 
    private void ForceVersLeBas()
    {
        Wheel_Collider_FL.attachedRigidbody.AddForce(-transform.up * forceAntiFlip *
                                                     Wheel_Collider_FL.attachedRigidbody.velocity.magnitude);
    }

}
