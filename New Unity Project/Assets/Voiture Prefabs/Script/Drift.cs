using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public TrailRenderer emmetteurMarqueAD;
    public TrailRenderer emmetteurMarqueAG;
    public TrailRenderer emmetteurMarqueDD;
    public TrailRenderer emmetteurMarqueDG;

    public WheelCollider roueAD;
    public WheelCollider roueAG;
    public WheelCollider roueDD;
    public WheelCollider roueDG;

    public float pointDeDerapage;


    // Update is called once per frame
    void Update()
    {
        VerifieDerapage(roueAD, emmetteurMarqueAD);
        VerifieDerapage(roueAG, emmetteurMarqueAG);
        VerifieDerapage(roueDD, emmetteurMarqueDD);
        VerifieDerapage(roueDG, emmetteurMarqueDG);

    }

    private void VerifieDerapage(WheelCollider collider, TrailRenderer emmetteurMarque)
    {
        WheelHit hit;
        collider.GetGroundHit(out hit);


        if(collider.GetGroundHit(out hit) == true && Mathf.Abs(hit.sidewaysSlip) > pointDeDerapage)
        {
            emmetteurMarque.emitting = true;
        }
        else
        {
            emmetteurMarque.emitting = false;
        }

    }
}
