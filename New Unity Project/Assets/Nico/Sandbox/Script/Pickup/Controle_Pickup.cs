using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_Pickup : MonoBehaviour
{
    public WheelCollider WC_FL;
    public WheelCollider WC_FR;
    public WheelCollider WC_RL;
    public WheelCollider WC_RR;

    public Transform WT_FL;
    public Transform WT_FR;
    public Transform WT_RL;
    public Transform WT_RR;

    private Rigidbody rb;

    private Vector3 orientation;

    public float maxBrakeTorque = 2000;
    public float maxTorque = 2000;
    public float steeringAngle =25;
    private float tempSteeringAngle;
    public float hauteurReset = 2;
    public float forceAntiFlip = 100;

    public static float vitesse;

    public static bool BoutonChanger = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        orientation = transform.InverseTransformDirection(rb.velocity);
        vitesse = orientation.z * 3.6f;

        if (vitesse > 5)
        {
            if (Input.GetKey(KeyCode.W))
            {
                WheelPower(maxTorque, 0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                WheelPower(0f, maxBrakeTorque * 20f);
            }
            else
            {
                WheelPower(0f, 0f);
            }
        }


        else if (vitesse < -5)
        {
            if (Input.GetKey(KeyCode.W))
            {
                WheelPower(0f, maxBrakeTorque * 20f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                WheelPower(-maxTorque, 0f);
            }
            else
            {
                WheelPower(0f, 0f);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                WheelPower(maxTorque, 0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                WheelPower(-maxTorque, 0f);
            }
            else
            {
                WheelPower(0f, 0f);
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

        WC_FL.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
        WC_FR.steerAngle = tempSteeringAngle * Input.GetAxis("Horizontal");
    }
    void Update()
    {
        //changing tyre direction
        Vector3 temp = WT_FL.localEulerAngles;
        Vector3 temp1 = WT_FR.localEulerAngles;
        temp.y = WC_FL.steerAngle - (WT_FL.localEulerAngles.z);
        WT_FL.localEulerAngles = temp;
        temp1.y = WC_FR.steerAngle - WT_FR.localEulerAngles.z;
        WT_FR.localEulerAngles = temp1;

        Change_Position_Roue(WC_FL, WT_FL);
        Change_Position_Roue(WC_FR, WT_FR);
        Change_Position_Roue(WC_RL, WT_RL);
        Change_Position_Roue(WC_RR, WT_RR);
    }


    private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.Rotate(WC_FL.rpm / 60 * 360 * Time.deltaTime, 0, 0);


        _collider.attachedRigidbody.AddForce(-transform.up * forceAntiFlip *
                                                     _collider.attachedRigidbody.velocity.magnitude);



        //ForceVersLeBas();

    }

    //Plus la voiture va vite, plus il y a un grande force d'appliquer sur la voiture pour
    //l'empecher de se renverser
    private void ForceVersLeBas()
    {
        WC_FL.attachedRigidbody.AddForce(-transform.up * forceAntiFlip *
                                                     WC_FL.attachedRigidbody.velocity.magnitude);
    }



    private void WheelPower(float _power,float _break)
    {
        WC_FL.motorTorque = _power;
        WC_FR.motorTorque = _power;
        WC_RL.motorTorque = _power;
        WC_RR.motorTorque = _power;

        WC_FL.brakeTorque = _break;
        WC_FR.brakeTorque = _break;
        WC_RL.brakeTorque = _break;
        WC_RR.brakeTorque = _break;
    }

}

