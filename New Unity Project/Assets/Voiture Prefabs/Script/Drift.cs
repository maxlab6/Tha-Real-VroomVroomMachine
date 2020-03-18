using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public GameObject emmetteurMarqueAD;
    public GameObject emmetteurMarqueAG;
    public GameObject emmetteurMarqueDD;
    public GameObject emmetteurMarqueDG;

    public WheelCollider roueAD;
    public WheelCollider roueAG;
    public WheelCollider roueDD;
    public WheelCollider roueDG;

    


    // Update is called once per frame
    void Update()
    {
        //VerifieDerapage(roueAD);
        //VerifieDerapage(roueAG);
        VerifieDerapage(roueDD);
        VerifieDerapage(roueDG);

    }

    private void VerifieDerapage(WheelCollider collider)
    {
        WheelHit hit;
        collider.GetGroundHit(out hit);
        //Debug.Log(hit.sidewaysSlip);
    }
}
