using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarController : MonoBehaviour
{
    [Header("Controller")]
    public bool playerControlled;
    public bool elianControlled;
    public bool felixControlled;
    public bool maximeControlled;
    public bool jonathanControlled;

    [Header("Colliders/Car Stuff")]
    public WheelCollider Wheel_Collider_FL;
    public WheelCollider Wheel_Collider_FR;
    public WheelCollider Wheel_Collider_RL;
    public WheelCollider Wheel_Collider_RR;

    public Transform Wheel_Transformation_FL;
    public Transform Wheel_Transformation_FR;
    public Transform Wheel_Transformation_RL;
    public Transform Wheel_Transformation_RR;
    private Rigidbody rb;

    private WheelHit hit;

    private Vector3 orientation;

    public static float vitesse;

    [Header("Driving Variables")]
    public float steeringAngle;
    public float maxBrakeTorque = 1000;
    public float maxTorque = 1000;
    public float maxSteeringAngle = 45;
    private float tempSteeringAngle;
    public float hauteurReset;
    public float forceAntiFlip = 100;

    //Position
    [Header("Position")]
    public int currentWaypointPos;
    public int counterWaypointPos;
    public int currentLapPos;
    public Transform lastWaypointPos;

    //Élian AI
    [Header("Élian AI")]
    public Transform waypoints;
    private bool isColliding = false;
    private GameObject waypointBox;
    private Vector3[] path;
    private int targetIndex;
    private int waypointIndex;
    private bool reachedEndPath;



    public static bool BoutonChanger = false;

    //Cameras
    private Camera cameraConducteurAvant;
    private Camera cameraConducteurArriere;
    private Camera cameraMinimap;
    private Camera cameraMultiple;
    private Vector3 camPos;
    private Transform carToFollow;
    private Vector3 positionCameraPoursuiteAvancer = new Vector3(8, 5, 8);
    private Vector3 rotationCameraPoursuiteAvancer = new Vector3(25, 0, 0);
    private Vector3 positionCameraPoursuiteReculer = new Vector3(8, 5, 8);
    private Vector3 rotationCameraPoursuiteReculer = new Vector3(25, 180, 0);
    private Vector3 positionCameraTop = new Vector3(0, 40, 0);
    private Vector3 rotationCameraTop = new Vector3(90, 0, 0);
    private float vitesseTransition = 8;
    private float vitesseChangementCamera = -10;
    private bool camTop;
    private bool camChase;

    void Start()
    {
        carToFollow = this.transform;
        cameraConducteurAvant = this.transform.Find("Camera_Conducteur_Avant").GetComponent<Camera>();
        cameraConducteurArriere = this.transform.Find("Camera_Conducteur_Arriere").GetComponent<Camera>();
        cameraMinimap = this.transform.Find("CameraMiniMap").GetComponent<Camera>();
        cameraMultiple = this.transform.Find("CameraMultiple").GetComponent<Camera>();


        if (elianControlled == true)
        {
            waypointBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            waypointBox.GetComponent<BoxCollider>().isTrigger = true;
            waypointBox.tag = "waypointsE";
            if (waypoints != null)
            {
                waypointIndex = 0;
                PathRequesterE.RequestPath(transform.position, waypoints.Find(waypointIndex.ToString()).position, OnPathFound);
            }

            Destroy(this.transform.Find("Camera_Conducteur_Avant").gameObject);
            Destroy(this.transform.Find("Camera_Conducteur_Arriere").gameObject);
            Destroy(this.transform.Find("CameraMiniMap").gameObject);
            Destroy(this.transform.Find("CameraMultiple").gameObject);
        }
        else if (jonathanControlled == true)
        {


            Destroy(this.transform.Find("Camera_Conducteur_Avant").gameObject);
            Destroy(this.transform.Find("Camera_Conducteur_Arriere").gameObject);
            Destroy(this.transform.Find("CameraMiniMap").gameObject);
            Destroy(this.transform.Find("CameraMultiple").gameObject);
        }
        else if (maximeControlled == true)
        {


            Destroy(this.transform.Find("Camera_Conducteur_Avant").gameObject);
            Destroy(this.transform.Find("Camera_Conducteur_Arriere").gameObject);
            Destroy(this.transform.Find("CameraMiniMap").gameObject);
            Destroy(this.transform.Find("CameraMultiple").gameObject);
        }
        else if (felixControlled == true)
        {


            Destroy(this.transform.Find("Camera_Conducteur_Avant").gameObject);
            Destroy(this.transform.Find("Camera_Conducteur_Arriere").gameObject);
            Destroy(this.transform.Find("CameraMiniMap").gameObject);
            Destroy(this.transform.Find("CameraMultiple").gameObject);
            Destroy(this.transform.Find("CanvasChar").gameObject);
            Destroy(this.transform.Find("GameObject").gameObject);
        }
        else if (playerControlled == true)
        {
            cameraConducteurAvant.enabled = true;
            cameraConducteurArriere.enabled = true;
            cameraMinimap.enabled = true;
            cameraMultiple.enabled = true;
        }

        rb = GetComponent<Rigidbody>();
    }

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

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        waypointBox.transform.localScale = new Vector3(100, 10, 0.5f);
        waypointBox.transform.position = currentWaypoint;
        waypointBox.layer = LayerMask.NameToLayer("Ignore Raycast");
        while (true)
        {
            if (isColliding == true)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    waypointIndex++;
                    if (waypoints.Find(waypointIndex.ToString()) != null)
                    {
                        Debug.Log("Next Waypoint Found!");
                        PathRequesterE.RequestPath(transform.position, waypoints.Find(waypointIndex.ToString()).position, OnPathFound);
                        yield break;
                    }
                    else
                    {
                        Wheel_Collider_FL.motorTorque = 0;
                        Wheel_Collider_FR.motorTorque = 0;
                        Wheel_Collider_RR.motorTorque = 0;
                        Wheel_Collider_RL.motorTorque = 0;
                        Wheel_Collider_FL.brakeTorque = maxTorque;
                        Wheel_Collider_FR.brakeTorque = maxTorque;
                        Wheel_Collider_RL.brakeTorque = maxTorque;
                        Wheel_Collider_RR.brakeTorque = maxTorque;
                        yield break;
                    }
                }
                currentWaypoint = path[targetIndex];
                waypointBox.transform.position = currentWaypoint;
            }

            Vector3 relativeVector = transform.InverseTransformPoint(currentWaypoint);
            float newSteerAngle = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
            Wheel_Collider_FL.steerAngle = newSteerAngle;
            Wheel_Collider_FR.steerAngle = newSteerAngle;
            RaycastHit hit;
            Vector3 sensorBasePos = transform.position;
            sensorBasePos += transform.forward * 2.30f;
            sensorBasePos += transform.up * 0.9f;
            bool stuck = false;
            bool attacking = false;


            //Sensors
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
                Wheel_Collider_FL.steerAngle = -1*newSteerAngle;
                Wheel_Collider_FR.steerAngle = -1*newSteerAngle;
            }
            if (Physics.Raycast(sensorBasePos,  Quaternion.AngleAxis(90f, transform.up) * transform.forward, out hit, 5f, 1 << 12))
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
            }

            if (Physics.Raycast(sensorBasePos, Quaternion.AngleAxis(-90f, transform.up) * transform.forward, out hit, 5f, 1 << 12))
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
            }



            if (Mathf.Abs(newSteerAngle) > 20)
            {
                if (vitesse < 10 && stuck == false && attacking == false)
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
                else if (vitesse > 30 && stuck == false && attacking == false)
                {
                    Wheel_Collider_FL.motorTorque = 0;
                    Wheel_Collider_FR.motorTorque = 0;
                    Wheel_Collider_RR.motorTorque = 0;
                    Wheel_Collider_RL.motorTorque = 0;
                    Wheel_Collider_FL.brakeTorque = maxTorque;
                    Wheel_Collider_FR.brakeTorque = maxTorque;
                    Wheel_Collider_RL.brakeTorque = maxTorque;
                    Wheel_Collider_RR.brakeTorque = maxTorque;
                }
            }
            else
            {
                if (vitesse < 80 && stuck == false && attacking == false)
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
                else if (vitesse > 80 && stuck == false && attacking == false)
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
            }



            orientation = transform.InverseTransformDirection(rb.velocity);
            vitesse = orientation.z * 3.6f;

            waypointBox.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            if (Mathf.Abs(vitesse) < 50)
            {
                tempSteeringAngle = (-(Mathf.Abs(vitesse) / 5) + maxSteeringAngle);
            }
            else
            {
                tempSteeringAngle = 15;
            }

            yield return null;

        }
    }

    public float GetDistancePos()
    {
        return (transform.position - lastWaypointPos.position).magnitude + currentWaypointPos * 100 + currentLapPos * (100 * GameObject.Find("Waypoints").transform.childCount);
    }

    public int GetCarPos(CarController[] allCars)
    {
        float distance = GetDistancePos();
        int position = 1;
        foreach (CarController car in allCars)
        {
            if (car.GetDistancePos() > distance)
                position++;
        }
        return position;
    }

    public void OnTriggerEnter(Collider col)
    {
        string tag = col.gameObject.tag;

        if ("waypointsE" == tag)
        {
            isColliding = true;
        }
        if ("waypoints" == tag)
        {
            currentWaypointPos = System.Convert.ToInt32(col.gameObject.name);
            {
                if (currentWaypointPos == 0 && counterWaypointPos == GameObject.Find("Waypoints").transform.childCount)
                {
                    counterWaypointPos = 0;
                    currentLapPos++;
                }
                Debug.Log(counterWaypointPos);
                Debug.Log(currentWaypointPos);
                if (counterWaypointPos == currentWaypointPos)
                {
                    Debug.Log("...................................................................................");
                    lastWaypointPos = col.transform;
                    counterWaypointPos++;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        string tag = col.gameObject.tag;

        if ("waypointsE" == tag)
        {
            isColliding = false;
        }
    }

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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Herbe" || collision.gameObject.tag == "Rampe" || collision.gameObject.tag == "Asphalte")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
                rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BoutonChanger")
        {
            BoutonChanger = true;
        }
    }

    void FixedUpdate()
    {
        if (playerControlled == true)
        {
            CameraUpdate();
            orientation = transform.InverseTransformDirection(rb.velocity);
            vitesse = orientation.z * 3.6f;

            if (vitesse > 5)
            {
                if (Input.GetKey(KeyCode.W))
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
                else if (Input.GetKey(KeyCode.S))
                {
                    Wheel_Collider_FL.motorTorque = 0;
                    Wheel_Collider_FR.motorTorque = 0;
                    Wheel_Collider_RR.motorTorque = 0;
                    Wheel_Collider_RL.motorTorque = 0;
                    Wheel_Collider_FL.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_FR.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_RL.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_RR.brakeTorque = maxBrakeTorque * 20;
                }
                else
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
            }


            else if (vitesse < -5)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Wheel_Collider_FL.motorTorque = 0;
                    Wheel_Collider_FR.motorTorque = 0;
                    Wheel_Collider_RR.motorTorque = 0;
                    Wheel_Collider_RL.motorTorque = 0;
                    Wheel_Collider_FL.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_FR.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_RL.brakeTorque = maxBrakeTorque * 20;
                    Wheel_Collider_RR.brakeTorque = maxBrakeTorque * 20;

                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Wheel_Collider_FL.motorTorque = -maxTorque;
                    Wheel_Collider_FR.motorTorque = -maxTorque;
                    Wheel_Collider_RR.motorTorque = -maxTorque;
                    Wheel_Collider_RL.motorTorque = -maxTorque;
                    Wheel_Collider_FL.brakeTorque = 0;
                    Wheel_Collider_FR.brakeTorque = 0;
                    Wheel_Collider_RL.brakeTorque = 0;
                    Wheel_Collider_RR.brakeTorque = 0;
                }
                else
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
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
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
                else if (Input.GetKey(KeyCode.S))
                {
                    Wheel_Collider_FL.motorTorque = -maxTorque;
                    Wheel_Collider_FR.motorTorque = -maxTorque;
                    Wheel_Collider_RR.motorTorque = -maxTorque;
                    Wheel_Collider_RL.motorTorque = -maxTorque;
                    Wheel_Collider_FL.brakeTorque = 0;
                    Wheel_Collider_FR.brakeTorque = 0;
                    Wheel_Collider_RL.brakeTorque = 0;
                    Wheel_Collider_RR.brakeTorque = 0;
                }
                else
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
            }

            if (Mathf.Abs(vitesse) < 50)
            {
                tempSteeringAngle = (-(Mathf.Abs(vitesse) / 5) + steeringAngle);
            }
            else
            {
                tempSteeringAngle = 15;
            }

            Wheel_Collider_FL.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
            Wheel_Collider_FR.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
        }
    }



    void Update()
    {
        //changing tyre direction
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

    void CameraUpdate()
    {
        //Choix de camera
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera vue poursuite
            camChase = true;
            camTop = false;

            cameraMultiple.enabled = true;
            cameraConducteurAvant.enabled = false;
            cameraConducteurArriere.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Camera vue du haut
            camChase = false;
            camTop = true;

            cameraMultiple.enabled = true;
            cameraConducteurAvant.enabled = false;
            cameraConducteurArriere.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Camera vue conducteur
            camChase = false;
            camTop = false;

            cameraMultiple.enabled = false;
        }


        if (cameraMultiple.enabled == true)
        {
            //Initialisation variable de rotation
            var rot = transform.rotation.eulerAngles;

            //Vérifie quelle caméra est active
            if (camChase == true)
            {

                if (Input.GetKey(KeyCode.V))
                {
                    //Camera de poursuite reculon

                    //Position de la camera
                    camPos.x = carToFollow.position.x + (positionCameraPoursuiteReculer.x * Mathf.Sin(carToFollow.eulerAngles.y * Mathf.PI / 180));
                    camPos.y = positionCameraPoursuiteReculer.y + carToFollow.position.y;
                    camPos.z = carToFollow.position.z + (positionCameraPoursuiteReculer.z * Mathf.Cos(carToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = rotationCameraPoursuiteReculer.x;
                    rot.y = rotationCameraPoursuiteReculer.y + carToFollow.transform.localEulerAngles.y;
                    rot.z = rotationCameraPoursuiteReculer.z;
                }
                else if (vitesse > vitesseChangementCamera)
                {
                    //Camera de poursuite avancer

                    //Camera de poursuite reculon

                    //Position de la camera
                    camPos.x = carToFollow.position.x - (positionCameraPoursuiteAvancer.x * Mathf.Sin(carToFollow.eulerAngles.y * Mathf.PI / 180));
                    camPos.y = positionCameraPoursuiteAvancer.y + carToFollow.position.y;
                    camPos.z = carToFollow.position.z - (positionCameraPoursuiteAvancer.z * Mathf.Cos(carToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera                               
                    rot.x = rotationCameraPoursuiteAvancer.x;
                    rot.y = rotationCameraPoursuiteAvancer.y + carToFollow.transform.localEulerAngles.y;
                    rot.z = rotationCameraPoursuiteAvancer.z;
                }
                else
                {
                    //Camera de poursuite avancer

                    //Position de la camera
                    camPos.x = carToFollow.position.x + (positionCameraPoursuiteReculer.x * Mathf.Sin(carToFollow.eulerAngles.y * Mathf.PI / 180));
                    camPos.y = positionCameraPoursuiteReculer.y + carToFollow.position.y;
                    camPos.z = carToFollow.position.z + (positionCameraPoursuiteReculer.z * Mathf.Cos(carToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = rotationCameraPoursuiteReculer.x;
                    rot.y = rotationCameraPoursuiteReculer.y + carToFollow.transform.localEulerAngles.y;
                    rot.z = rotationCameraPoursuiteReculer.z;
                }
            }
            else if (camTop == true)
            {
                //Camera avec une vue du haut.

                //Position de la camera
                camPos.x = carToFollow.position.x + positionCameraTop.x;
                camPos.y = carToFollow.position.y + positionCameraTop.y;
                camPos.z = carToFollow.position.z + positionCameraTop.z;

                //Rotation de la camera           
                rot.x = rotationCameraTop.x;
                rot.y = rotationCameraTop.y + carToFollow.transform.localEulerAngles.y;
                rot.z = rotationCameraTop.z;
            }


            //Attribue les valeurs défini à la position et la rotation.
            // transform.position = Vector3.Lerp(transform.position, camPos, vitesseTransition * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(rot);
        }
        else if (cameraMultiple.enabled == false)
        {
            if (Input.GetKey(KeyCode.V))
            {
                cameraConducteurAvant.enabled = false;
                cameraConducteurArriere.enabled = true;
            }
            else if (vitesse > vitesseChangementCamera)
            {
                cameraConducteurAvant.enabled = true;
                cameraConducteurArriere.enabled = false;
            }
            else
            {
                cameraConducteurAvant.enabled = false;
                cameraConducteurArriere.enabled = true;
            }
        }
    }


    private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.Rotate(Wheel_Collider_FL.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        ForceVersLeBas();

    }

    //Plus la voiture va vite, plus il y a un grande force d'appliquer sur la voiture pour
    //l'empecher de se renverser
    private void ForceVersLeBas()
    {
        Wheel_Collider_FL.attachedRigidbody.AddForce(-transform.up * forceAntiFlip *
                                                     Wheel_Collider_FL.attachedRigidbody.velocity.magnitude);
    }

}
