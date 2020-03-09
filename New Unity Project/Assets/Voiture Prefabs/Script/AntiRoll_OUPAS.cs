using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public WheelCollider roueGauche;
    public WheelCollider roueDroite;
    public float antiRoll = 1000;
    private Rigidbody rb;
    private WheelHit hit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate ()
    {
        float extensionSusG = 1.0f;
        float extensionSusD = 1.0f;
        hit = new WheelHit();

        bool roueSolG = roueGauche.GetGroundHit(out hit);
        bool roueSolD = roueDroite.GetGroundHit(out hit);

        if (roueSolG)
        {
            extensionSusG = (-roueGauche.transform.InverseTransformDirection(hit.point).y - roueGauche.radius) / roueGauche.suspensionDistance;
        }

        if (roueSolD)
        {
            extensionSusD = (-roueDroite.transform.InverseTransformDirection(hit.point).y - roueDroite.radius) / roueDroite.suspensionDistance;
        }

        float forceAntiRoll = (extensionSusG - extensionSusD) * antiRoll;

        if (roueSolG)
        {
            rb.AddForceAtPosition(roueGauche.transform.up * -forceAntiRoll, roueGauche.transform.position);
        }

        if (roueDroite)
        {
            rb.AddForceAtPosition(roueDroite.transform.up * forceAntiRoll, roueDroite.transform.position);
        }
    }
}
