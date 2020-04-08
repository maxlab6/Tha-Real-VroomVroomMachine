using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift_Pickup : MonoBehaviour
{
    public WheelCollider WC_FL;
    public WheelCollider WC_FR;
    public WheelCollider WC_RL;
    public WheelCollider WC_RR;

    public Transform WT_FL;
    public Transform WT_FR;
    public Transform WT_RL;
    public Transform WT_RR;

    public Transform Trail_FL;
    public Transform Trail_FR;
    public Transform Trail_RL;
    public Transform Trail_RR;

    public TrailRenderer Trail_Maker_FL;
    public TrailRenderer Trail_Maker_FR;
    public TrailRenderer Trail_Maker_RL;
    public TrailRenderer Trail_Maker_RR;

    public Rigidbody rb;

    public float Max_To_Drift;
    public float distance_x;
    private float orientation;
    
    void Update()
    {
        VerifieDerapage(WC_FL, Trail_Maker_FL);
        VerifieDerapage(WC_FR , Trail_Maker_FR); 
        VerifieDerapage(WC_RL, Trail_Maker_RL);
        VerifieDerapage(WC_RR, Trail_Maker_RR);
        PositionTrailMaker();


    }

    private void VerifieDerapage(WheelCollider collider, TrailRenderer emmetteurMarque)
    {

        WheelHit hit;
        collider.GetGroundHit(out hit);

        if(collider.GetGroundHit(out hit) == true)
        {
            emmetteurMarque.emitting = true;
            if(Mathf.Abs(hit.sidewaysSlip) > Max_To_Drift)
            {               
                emmetteurMarque.textureMode = LineTextureMode.Stretch;
            }
            else
            {
                emmetteurMarque.textureMode = LineTextureMode.Tile;
            }            
        }
        else
        {
            emmetteurMarque.emitting = false;
        }

    }

    private void PositionTrailMaker()
    {
        //Debug.Log(Mathf.Atan( (WT_FR.transform.localPosition.y - WT_FL.transform.localPosition.y)/(WT_FR.transform.localPosition.x - WT_FL.transform.localPosition.x) )*2*Mathf.PI);
        
        Trail_FL.localPosition = new Vector3(WT_FL.transform.localPosition.x, WT_FL.transform.localPosition.y - 0.62f, WT_FL.transform.localPosition.z);
        Trail_FR.localPosition = new Vector3(WT_FR.transform.localPosition.x, WT_FR.transform.localPosition.y - 0.62f, WT_FR.transform.localPosition.z);
        Trail_RL.localPosition = new Vector3(WT_RL.transform.localPosition.x, WT_RL.transform.localPosition.y - 0.62f, WT_RL.transform.localPosition.z);
        Trail_RR.localPosition = new Vector3(WT_RR.transform.localPosition.x, WT_RR.transform.localPosition.y - 0.62f, WT_RR.transform.localPosition.z);
       
    }
}
