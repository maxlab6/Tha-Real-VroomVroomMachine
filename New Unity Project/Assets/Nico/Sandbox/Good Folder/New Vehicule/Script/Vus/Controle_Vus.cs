using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controle_Vus : MonoBehaviour
{
    //Wheeel Colliders
    private WheelCollider WC_FL;
    private WheelCollider WC_FR;
    private WheelCollider WC_RL;
    private WheelCollider WC_RR;

    //Wheel Transform
    private Transform WT_FL;
    private Transform WT_FR;
    private Transform WT_RL;
    private Transform WT_RR;

    //Tire Track Transform
    private Transform Trail_FL;
    private Transform Trail_FR;
    private Transform Trail_RL;
    private Transform Trail_RR;

    //Tire Track Renderer
    private TrailRenderer Trail_Maker_FL;
    private TrailRenderer Trail_Maker_FR;
    private TrailRenderer Trail_Maker_RL;
    private TrailRenderer Trail_Maker_RR;


    //RigidBody
    private Rigidbody rb;

    //Controle Vehicule
    private Vector3 orientation;
    public float maxBrakeTorque = 2000;
    public float maxTorque = 2000;
    public float steeringAngle = 25;
    private float tempSteeringAngle;
    public float hauteurReset = 2;
    public float forceAntiFlip = 100;
    public static float vitesse;

    public static bool BoutonChanger = false;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Rampe")
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

    private void Start()
    {
        TrouverObjet();
    }

    private void FixedUpdate()
    {
        ControleVehicule();
        AnimationDerapage(WC_FL, WT_FL, Trail_Maker_FL, Trail_FL);
        AnimationDerapage(WC_FR, WT_FR, Trail_Maker_FR, Trail_FR);
        AnimationDerapage(WC_RL, WT_RL, Trail_Maker_RL, Trail_RL);
        AnimationDerapage(WC_RR, WT_RR, Trail_Maker_RR, Trail_RR);
    }

    private void Update()
    {
        AnimationRoues();
    }

    private void TrouverObjet()
    {
        //RigidBody
        rb = GameObject.Find("SandBox/Vus Jeu").GetComponent<Rigidbody>();

        //Wheel Collider
        WC_FL = GameObject.Find("SandBox/Vus Jeu/Wheel_Collider/WC_FL").GetComponent<WheelCollider>();
        WC_FR = GameObject.Find("SandBox/Vus Jeu/Wheel_Collider/WC_FR").GetComponent<WheelCollider>();
        WC_RL = GameObject.Find("SandBox/Vus Jeu/Wheel_Collider/WC_RL").GetComponent<WheelCollider>();
        WC_RR = GameObject.Find("SandBox/Vus Jeu/Wheel_Collider/WC_RR").GetComponent<WheelCollider>();

        //Wheel Transform
        WT_FL = GameObject.Find("SandBox/Vus Jeu/Wheel_Transform/WT_FL").GetComponent<Transform>();
        WT_FR = GameObject.Find("SandBox/Vus Jeu/Wheel_Transform/WT_FR").GetComponent<Transform>();
        WT_RL = GameObject.Find("SandBox/Vus Jeu/Wheel_Transform/WT_RL").GetComponent<Transform>();
        WT_RR = GameObject.Find("SandBox/Vus Jeu/Wheel_Transform/WT_RR").GetComponent<Transform>();

        //Trail Transform
        Trail_FL = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_FL").GetComponent<Transform>();
        Trail_FR = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_FR").GetComponent<Transform>();
        Trail_RL = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_RL").GetComponent<Transform>();
        Trail_RR = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_RR").GetComponent<Transform>();

        //Trail Renderer
        Trail_Maker_FL = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_FL").GetComponent<TrailRenderer>();
        Trail_Maker_FR = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_FR").GetComponent<TrailRenderer>();
        Trail_Maker_RL = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_RL").GetComponent<TrailRenderer>();
        Trail_Maker_RR = GameObject.Find("SandBox/Vus Jeu/Trail_Maker/Trail_Maker_RR").GetComponent<TrailRenderer>();
    }

    private void ControleVehicule()
    {
        orientation = transform.InverseTransformDirection(rb.velocity);
        vitesse = orientation.z * 3.6f;

        if (Input.GetKey(KeyCode.W))
        {
            ToucheW(vitesse);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ToucheS(vitesse);
        }
        else
        {
            WC_FL.motorTorque = 0f;
            WC_FR.motorTorque = 0f;
            WC_RL.motorTorque = 0f;
            WC_RR.motorTorque = 0f;

            WC_FL.brakeTorque = maxBrakeTorque / 4;
            WC_FR.brakeTorque = maxBrakeTorque / 4;
            WC_RL.brakeTorque = maxBrakeTorque / 4;
            WC_RR.brakeTorque = maxBrakeTorque / 4;
        }
        Sterring(vitesse);
    }

    private void ToucheW(float _vitesse)
    {
        if (_vitesse > -10)
        {
            WC_FL.motorTorque = maxTorque;
            WC_FR.motorTorque = maxTorque;
            WC_RL.motorTorque = maxTorque;
            WC_RR.motorTorque = maxTorque;

            WC_FL.brakeTorque = 0f;
            WC_FR.brakeTorque = 0f;
            WC_RL.brakeTorque = 0f;
            WC_RR.brakeTorque = 0f;
        }
        else
        {
            WC_FL.motorTorque = 0f;
            WC_FR.motorTorque = 0f;
            WC_RL.motorTorque = 0f;
            WC_RR.motorTorque = 0f;

            WC_FL.brakeTorque = maxBrakeTorque * 20f;
            WC_FR.brakeTorque = maxBrakeTorque * 20f;
            WC_RL.brakeTorque = maxBrakeTorque * 20f;
            WC_RR.brakeTorque = maxBrakeTorque * 20f;
        }
    }

    private void ToucheS(float _vitesse)
    {
        if (_vitesse > 10)
        {
            WC_FL.motorTorque = 0f;
            WC_FR.motorTorque = 0f;
            WC_RL.motorTorque = 0f;
            WC_RR.motorTorque = 0f;

            WC_FL.brakeTorque = maxBrakeTorque * 20f;
            WC_FR.brakeTorque = maxBrakeTorque * 20f;
            WC_RL.brakeTorque = maxBrakeTorque * 20f;
            WC_RR.brakeTorque = maxBrakeTorque * 20f;
        }
        else
        {
            WC_FL.motorTorque = -maxTorque;
            WC_FR.motorTorque = -maxTorque;
            WC_RL.motorTorque = -maxTorque;
            WC_RR.motorTorque = -maxTorque;

            WC_FL.brakeTorque = 0f;
            WC_FR.brakeTorque = 0f;
            WC_RL.brakeTorque = 0f;
            WC_RR.brakeTorque = 0f;
        }
    }

    private void Sterring(float _vitesse)
    {
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

    private void AnimationRoues()
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
        _transform.Rotate(_collider.rpm / 60 * 360 * Time.deltaTime, 0, 0);


        //_collider.attachedRigidbody.AddForce(-transform.up * forceAntiFlip * _collider.attachedRigidbody.velocity.magnitude);
    }

    private void AnimationDerapage(WheelCollider _collider, Transform _WheelTransform, TrailRenderer _TrailMaker, Transform _TrailTransform)
    {

        WheelHit hit;
        _collider.GetGroundHit(out hit);

        if (_collider.GetGroundHit(out hit) == true)
        {
            _TrailMaker.emitting = true;
            if (Mathf.Abs(hit.sidewaysSlip) > _collider.sidewaysFriction.extremumSlip)
            {
                _TrailMaker.textureMode = LineTextureMode.Stretch;
            }
            else
            {
                _TrailMaker.emitting = false;
            }
            
        }
        else
        {
            _TrailMaker.emitting = false;
        }

        _TrailTransform.localPosition = new Vector3(_WheelTransform.transform.localPosition.x, _WheelTransform.transform.localPosition.y - (_collider.radius * 10 + 0.01f), _WheelTransform.transform.localPosition.z);
    }   
}
