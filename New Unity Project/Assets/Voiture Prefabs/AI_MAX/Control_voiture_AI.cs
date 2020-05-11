using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_voiture_AI : MonoBehaviour
{
    public WheelCollider WheelColliderAG;
    public WheelCollider WheelColliderAD;
    public WheelCollider WheelColliderDG;
    public WheelCollider WheelColliderDD;

    public Transform transformRoudAG;
    public Transform transformRoudAD;
    public Transform transformRoudDG;
    public Transform transformRoudDD;

    private Rigidbody rb;
    private WheelHit hit;

    private Vector3 orientation;

    public float angleMaxRoue;
    private float angleRoueTempo;
    public float forceAntiFlip = 100;

    public static float vitesse;

    public static bool BoutonChanger = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        orientation = transform.InverseTransformDirection(rb.velocity);
        vitesse = orientation.z * 3.6f;

        if (Mathf.Abs(vitesse) < 50)
        {
            angleRoueTempo = (-(Mathf.Abs(vitesse) / 5) + angleMaxRoue);
        }
        else
        {
            angleRoueTempo = 15;
        }

        WheelColliderAG.steerAngle = angleRoueTempo * Input.GetAxis("Horizontal");
        WheelColliderAD.steerAngle = angleRoueTempo * Input.GetAxis("Horizontal");
    }

    void Update()
    {
        //changing tyre direction
        Vector3 temp = transformRoudAG.localEulerAngles;
        Vector3 temp1 = transformRoudAD.localEulerAngles;
        temp.y = WheelColliderAG.steerAngle - (transformRoudAG.localEulerAngles.z);
        transformRoudAG.localEulerAngles = temp;
        temp1.y = WheelColliderAD.steerAngle - transformRoudAD.localEulerAngles.z;
        transformRoudAD.localEulerAngles = temp1;

        Change_Position_Roue(WheelColliderAG, transformRoudAG);
        Change_Position_Roue(WheelColliderAD, transformRoudAD);
        Change_Position_Roue(WheelColliderDG, transformRoudDG);
        Change_Position_Roue(WheelColliderDD, transformRoudDD);
    }


    private void Change_Position_Roue(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.Rotate(WheelColliderAG.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        ForceVersLeBas();

    }

    //Plus la voiture va vite, plus il y a un grande force d'appliquer sur la voiture pour
    //l'empecher de se renverser
    private void ForceVersLeBas()
    {
        WheelColliderAG.attachedRigidbody.AddForce(-transform.up * forceAntiFlip * WheelColliderAG.attachedRigidbody.velocity.magnitude);
    }
}
