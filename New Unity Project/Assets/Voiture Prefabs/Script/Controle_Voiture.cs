using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_Voiture : MonoBehaviour
{
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

    public float maxBrakeTorque = 1000;
    public float maxTorque = 1000;
    public float steeringAngle;
    private float tempSteeringAngle;
    public float hauteurReset;
    public float forceAntiFlip = 100;

    public static float vitesse;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Plancher")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rb.MovePosition(new Vector3(rb.position.x, rb.position.y + hauteurReset, rb.position.z));
                rb.MoveRotation(Quaternion.Euler(rb.transform.localEulerAngles.x, rb.transform.localEulerAngles.y, 0));
            }
        }

    }

    
    void FixedUpdate()
    {
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

        if(Mathf.Abs(vitesse) < 50)
        {
            tempSteeringAngle = (-(Mathf.Abs(vitesse)/5) + steeringAngle);            
        }else
        {
            tempSteeringAngle = 15;
        }
        
        Wheel_Collider_FL.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
        Wheel_Collider_FR.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
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


    private void Change_Position_Roue (WheelCollider _collider, Transform _transform)
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
