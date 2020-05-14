using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleVoitureIaFelix : MonoBehaviour
{
    [Header("Wheel Collider")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Variables de physique")]
    public float maxSteerAngle = 45f;
    public float targetSteerAngle = 0f;
    public float turnSpeed = 5f;
    public float maxTorque = 500f;
    public float maxBrakeTorque = 1000f;
    public float currentSpeed;
    public float maxSpeed;
    public bool isBraking;

    [Header("Modulation de vitesse")]
    public float modulationVariable = 1f;
    public float targetSpeed;
    public float brakingFactor = 1.5f;

    [Header("Chemin/Checkpoints")]
    public Transform parentCheckPoint;
    private List<Transform> checkPoints;
    public int checkPointActuel;
    public int nbCheckPointsAvance;

    [Header("Path Finding")]
    public GameObject gameManager;
    private List<NodeF> cheminCourt;
    public int currentNode = 0;
    public int nbNodeAvance = 8;
    public float distanceAvance = 5f;
    public bool afficherPath = true;

    [Header("Sensor")]
    public float sensorLeght = 10f;
    public float frontSideSensorPosition = 1F;
    public float frontSensorAngle = 30f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.5f, 1.5f);
    public bool avoiding = false;
    public bool reculer = false;


    void Start()
    {
        Transform[] pathTransform = parentCheckPoint.GetComponentsInChildren<Transform>();

        checkPoints = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != parentCheckPoint.transform)
            {
                checkPoints.Add(pathTransform[i]);
            }
        }

        cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]);

    }


    void FixedUpdate()
    {
        TrouverChemin();
        Sensors();
        ApplySteer();
        Drive();
        CheckDistance();
        ValidationCheckPoint();
        ModulationDeVitesse();
        Braking();
        LerpToSteerAngle();
        Test();
        ForceVersLeBas();
    }

    private void Test()
    {
        gameManager.GetComponent<PathFindingF>().grid.afficher = afficherPath;
        if (targetSpeed < 30f)
        {
            nbNodeAvance = 2;
        }
        else if (targetSpeed < 50f)
        {
            nbNodeAvance = 4;
        }
        else
        {
            nbNodeAvance = 6;
        }
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, cheminCourt[currentNode].Position) < distanceAvance)
        {
            currentNode++;
        }
    }

    private void TrouverChemin()
    {

    }

    private void ModulationDeVitesse()
    {
        Vector3 relativeVectorModulation = transform.InverseTransformPoint(cheminCourt[currentNode + nbNodeAvance].Position);
        float modulationFactor = Mathf.Pow((1f - (relativeVectorModulation.x / relativeVectorModulation.magnitude)),2) * maxSpeed;
        //float modulationFactor = maxSpeed * (1f - (relativeVectorModulation.x / relativeVectorModulation.magnitude)) * (1f - (1 / relativeVectorModulation.x));
        targetSpeed = modulationFactor;
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplier = 0;
        avoiding = false;
        reculer = false;

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, 3f))
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
            {
                reculer = true;
            }
            else
            {
                reculer = false;
            }
        }

        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght))
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }
        


        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLeght))
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }


        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght))
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }


        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLeght))
        {
            if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLeght))
            {
                if (hit.collider.CompareTag("mur") || hit.collider.CompareTag("car"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if (hit.normal.x < 0)
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

        if (avoiding && !reculer)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
        else if (reculer)
        {
            targetSteerAngle = 0;
        }
    }

    private void Braking()
    {
        if (!reculer)
        {
            if (isBraking)
            {
                wheelRL.brakeTorque = maxBrakeTorque;
                wheelRR.brakeTorque = maxBrakeTorque;
            }
            else if (!isBraking)
            {
                wheelRL.brakeTorque = 0;
                wheelRR.brakeTorque = 0;
            }
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }

    private void ValidationCheckPoint()
    {
        if (checkPoints[checkPointActuel].GetComponent<OnCollision>().getIsColliding())
        {
            checkPointActuel++;
            if (checkPointActuel + nbCheckPointsAvance >= checkPoints.Count)
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[0]);
            }
            else
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]);
            }
            currentNode = 0;
        }
        if (checkPointActuel == checkPoints.Count)
        {
            checkPointActuel = 0;
            if (checkPointActuel + nbCheckPointsAvance >= checkPoints.Count)
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[0]);
            }
            else
            {
                cheminCourt = gameManager.GetComponent<PathFindingF>().TrouverCheminCourt(checkPoints[checkPointActuel + nbCheckPointsAvance]);
            }
            currentNode = 0;
        }
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed && currentSpeed < targetSpeed && !isBraking && !reculer|| currentSpeed < 15f)
        {
            wheelFL.motorTorque = maxTorque;
            wheelFR.motorTorque = maxTorque;
        }
        else if (reculer)
        {
            wheelFL.motorTorque = -maxTorque;
            wheelFR.motorTorque = -maxTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
        if (currentSpeed > brakingFactor * targetSpeed && currentSpeed > 15f)
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }
    }

    private void ApplySteer()
    {
        if (avoiding || reculer) return;
        Vector3 relativeVector = transform.InverseTransformPoint(cheminCourt[currentNode + nbNodeAvance].Position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void ForceVersLeBas()
    {
        wheelFL.attachedRigidbody.AddForce(-transform.up * 100 *
                                                     wheelFL.attachedRigidbody.velocity.magnitude);
    }
}
