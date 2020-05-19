// Auteur : Félix Doyon
// Ce script contient tous les fonctions relative au controle de l'IA (angle des roues, vitesse, point à suivre ...)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleVoitureIaFelix : MonoBehaviour
{
    [Header("Wheel Collider")] //Les 4 wheel collider de l'auto
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Variables de physique")]
    public float maxSteerAngle = 45f; //Angle maximale des roue
    public float targetSteerAngle = 0f; // Angle actuel des roues
    public float turnSpeed = 5f; //Vitesse de rotation des roues
    public float maxTorque = 500f; //Force maxiamle des roues pour avancer
    public float maxBrakeTorque = 1000f; //Force maximales des roues pour freiner
    public float currentSpeed; //Vitesse actuel
    public float maxSpeed; //Vitesse maximale
    public bool isBraking; //Si l'auto doit freiner ou non

    [Header("Modulation de vitesse")]
    public float targetSpeed; //Vitesse visé
    public float brakingFactor = 1.5f; //Multiplicateur appliqué sur targetSpeed dans un if pour savoir s'il doit seulement mettre le torque à 0 ou freiner aussi
    public float modulationFactor = 0f; //variable temporaire pour calculé la vitesse visé

    [Header("Chemin/Checkpoints")]
    public Transform parentCheckPoint; //GameObject qui contient les checks points
    private List<Transform> checkPoints; //Liste des checks points
    public int checkPointActuel; //Compteur pour savoir à quelle check point on est rendu
    public int nbCheckPointsAvance; //Nombre de check point d'avance qu'il faut regarder (toujours à 1)

    [Header("Path Finding")]
    public GameObject gameManager; //Le GameObject du game manager (contient la grille et le script de path finding)
    private List<NodeF> cheminCourt; //Liste des nodes (dans ce cas ci des transforms) qui constituent le chemin à emprunter
    public int currentNode = 0; //Compteur pour savoir vers quelle node il faut aller
    public int nbNodeAvance = 8; //Variable pour savoir combien de node à l'avance l'auto doit se diriger (pour rendre les virages plus fluide)
    public float distanceAvance = 5f; //Distance entre la node et l'auto à laquelle nbNodeAvance est incrémenter
    public bool afficherPath = true; //Savoir si on affiche ou non le chemin court visuellement (pas en jeu)

    [Header("Sensor")]
    public float sensorLeght = 10f; //Longueur des Rays Casts
    public float frontSideSensorPosition = 1F; //Longueur qu'on décale les Rays Casts pour avoir ceux des côtés
    public float frontSensorAngle = 30f; //Angle des Rays Casts qui sont en angle
    public Vector3 frontSensorPosition = new Vector3(0f, 0.5f, 1.5f); //Position relative du premier Ray Cast par rapport au centre de l'auto
    public bool avoiding = false; //Si l'auto est en train d'esquiver
    public bool reculer = false; //Si l'auto est en train de reculer


    void Start()
    {
        Transform[] pathTransform = parentCheckPoint.GetComponentsInChildren<Transform>();

        checkPoints = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++) //Extrait les checks points du GameObject où ils sont contenu
        {
            if (pathTransform[i] != parentCheckPoint.transform)
            {
                checkPoints.Add(pathTransform[i]);
            }
        }

        cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]); //Calculer le chemin au commencement

    }


    void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckDistance();
        ValidationCheckPoint();
        ModulationDeVitesse();
        Braking();
        LerpToSteerAngle();
        DefinirVariableSelonVitesse();
        ForceVersLeBas();
    }

    private void DefinirVariableSelonVitesse() //Définit nbNodeAvance et distanceAvance selon la vitesse et affiche ou non le chemin
    {
        gameManager.GetComponent<PathFindingF>().grid.afficher = afficherPath; //Affiche ou non le chemin
        if (targetSpeed < 30f)
        {
            nbNodeAvance = 2;
            distanceAvance = 12;
        }
        else if (targetSpeed < 60f)
        {
            nbNodeAvance = 4;
            distanceAvance = 12;
        }
        else if (targetSpeed < 100f)
        {
            nbNodeAvance = 5;
            distanceAvance = 12;
        }
        else
        {
            nbNodeAvance = 6;
            distanceAvance = 17;
        }
    }

    private void CheckDistance() //Regarde s'il faut incrémenter currentNode selon la distance d'avance
    {
        if (Vector3.Distance(transform.position, cheminCourt[currentNode].Position) < distanceAvance)
        {
            currentNode++;
        }
    }

    private void ModulationDeVitesse() //Fonction pour moduler la vitesse
    {
        Vector3 relativeVectorModulation = transform.InverseTransformPoint(cheminCourt[currentNode + nbNodeAvance * 2].Position); //Calcule le vecteur relatif entre l'auto et un certain check point
        modulationFactor = (1f - (relativeVectorModulation.x / relativeVectorModulation.magnitude)) * maxSpeed - Mathf.Abs(relativeVectorModulation.x) * 4; //Fonction pour calculer la vitesse relative
        if (modulationFactor > 30f) //Empêche que la vitesse soit trop réduite
        {
            targetSpeed = modulationFactor;
        }
        else
        {
            targetSpeed = 30f;
        }
    }

    private void Sensors() //Fonction pour les Rays Castsw
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position; //Prend la position de l'auto
        sensorStartPos += transform.forward * frontSensorPosition.z; //Ajoute les composante pour que le Ray Cast soir à l'avant au milieu
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplier = 0; //Remet les variables à 0
        avoiding = false;
        reculer = false;

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, 3f)) //Ray Cast plus petit à l'avant pour reculer
        {
            if (hit.collider.CompareTag("mur"))
            {
                reculer = true;
            }
            else
            {
                reculer = false;
            }
        }

        sensorStartPos += transform.right * frontSideSensorPosition; //Change la position du Ray Cast
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght)) //Ray Cast avant droite
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car") || hit.collider.CompareTag("carAiMax"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }



        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLeght)) //Ray Cast avant droit en angle
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car") || hit.collider.CompareTag("carAiMax"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }


        sensorStartPos -= transform.right * frontSideSensorPosition * 2; //Change la position du Ray Cast
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght)) //Ray Cast avant gauche
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car") || hit.collider.CompareTag("carAiMax"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }


        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLeght)) //Ray Cast avant gauche en angle
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car") || hit.collider.CompareTag("carAiMax"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        if (avoidMultiplier == 0) //Si les deux Rays Cast avant ont été touchés, le avoidMultiplier va être a 0 et donc, on va regarder le comportement de celui à l'avant
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght)) //Ray Cast avant au centre
            {
                if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car") || hit.collider.CompareTag("carAiMax"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0) //Regarde la normale du vecteur pour savoir s'il faut aller à gauche ou à droite
                    {
                        avoidMultiplier = -1f;
                    }
                    else
                    {
                        avoidMultiplier = 1f;
                    }
                }

            }
        }

        if (avoiding && !reculer) //Si un Ray Cast à été toucher
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
        else if (reculer) //S'il faut reculer
        {
            targetSteerAngle = 0;
        }
    }

    private void Braking() //Fonction pour appliquer le frein s'il faut
    {
        if (isBraking) //S'il doit freiner
        {
            wheelRL.brakeTorque = maxBrakeTorque; //Applique le frein
            wheelRR.brakeTorque = maxBrakeTorque;
        }
        else if (!isBraking)
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }

    private void ValidationCheckPoint() //Fonction pour valider le passage des checkpoint
    {
        if (checkPoints[checkPointActuel].GetComponent<OnCollision>().getIsColliding()) //Utilise le script OnCollision qui est sur les checks points pour savoir si l'auto rentre dedans
        {
            checkPointActuel++;
            if (checkPointActuel + nbCheckPointsAvance >= checkPoints.Count) //Si on doit aller au dernier check point
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[0]); //Recalcule le chemin le plus court
            }
            else
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]); //Recalcule le chemin le plus court
            }
            currentNode = 0;
        }
        if (checkPointActuel == checkPoints.Count) //Si on est rendu au dernier ckeck point
        {
            checkPointActuel = 0;
            if (checkPointActuel + nbCheckPointsAvance >= checkPoints.Count)
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[0]); //Recalcule le chemin le plus court
            }
            else
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]); //Recalcule le chemin le plus court
            }
            currentNode = 0; //Remet la node actuel à 0
        }
    }

    private void Drive() //Fonction pour avancer
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000; //Calcule la vitesse actuel à partir de la vitesse de rotation des roues
        if (currentSpeed < maxSpeed && currentSpeed < targetSpeed && !isBraking && !reculer || currentSpeed < 15f) //Si la vitesse actuel est plus petite que la vitesse max et la vitesse visé et qu'il ne doit pas freiner ou reculer, mais que la vitesse est pas trop faible
        {
            wheelFL.motorTorque = maxTorque; //Applique la force maximale au roues
            wheelFR.motorTorque = maxTorque;
        }
        else if (reculer) //S'il doit reculer
        {
            wheelFL.motorTorque = -maxTorque; //Applique la force maximal par en arrière
            wheelFR.motorTorque = -maxTorque;
        }
        else
        {
            wheelFL.motorTorque = 0; //N'applique aucune force
            wheelFR.motorTorque = 0;
        }
        if (currentSpeed > brakingFactor * targetSpeed && currentSpeed > 15f) //Si la vitesse est beaucuop plus grande que la vitesse vissé
        {
            isBraking = true; //Met la variable de frein en true pour la fonction Brake()
        }
        else
        {
            isBraking = false;
        }
    }

    private void ApplySteer() //Fonction pour appliqué l'angle désiré aux roues
    {
        if (avoiding || reculer) return; //Si on recule ou on esquive, la fonction est ignoré
        Vector3 relativeVector = transform.InverseTransformPoint(cheminCourt[currentNode + nbNodeAvance].Position); //Calcule le veteur relatif entre l'auto et la node désiré
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle; //Calcule l'angle du veteur pour l'appliqué aux roues
        targetSteerAngle = newSteer;
    }

    private void LerpToSteerAngle() //Fonction pour que les roues tournent pluis fluidement et moins brusquement
    {
        if (currentSpeed > 200) //Calcule la vitesse de rotation des roues selon la vitesse
        {
            turnSpeed = 8;
        }
        else if (currentSpeed > 100)
        {
            turnSpeed = 6;
        }
        else if (currentSpeed > 50)
        {
            turnSpeed = 4;
        }
        else
        {
            turnSpeed = 2;
        }
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed); //Applique l'angle aux roues selon une différence de temps
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void ForceVersLeBas() //Fonction pour appliqué la même force au sol que l'auto du joueur humain
    {
        wheelFL.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheelFL.attachedRigidbody.velocity.magnitude);
        wheelFR.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheelFL.attachedRigidbody.velocity.magnitude);
        wheelRL.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheelFL.attachedRigidbody.velocity.magnitude);
        wheelRR.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheelFL.attachedRigidbody.velocity.magnitude);
    }
}
