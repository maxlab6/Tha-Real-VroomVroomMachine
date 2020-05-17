using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vus_Show : MonoBehaviour
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


    void Start()
    {
        //Wheel Collider
        WC_FL = GameObject.Find("Vus Show/Wheel_Collider/WC_FL").GetComponent<WheelCollider>();
        WC_FR = GameObject.Find("Vus Show/Wheel_Collider/WC_FR").GetComponent<WheelCollider>();
        WC_RL = GameObject.Find("Vus Show/Wheel_Collider/WC_RL").GetComponent<WheelCollider>();
        WC_RR = GameObject.Find("Vus Show/Wheel_Collider/WC_RR").GetComponent<WheelCollider>();

        //Wheel Transform
        WT_FL = GameObject.Find("Vus Show/Wheel_Transform/WT_FL").GetComponent<Transform>();
        WT_FR = GameObject.Find("Vus Show/Wheel_Transform/WT_FR").GetComponent<Transform>();
        WT_RL = GameObject.Find("Vus Show/Wheel_Transform/WT_RL").GetComponent<Transform>();
        WT_RR = GameObject.Find("Vus Show/Wheel_Transform/WT_RR").GetComponent<Transform>();
    }


    void Update()
    {
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

    }

    
}
