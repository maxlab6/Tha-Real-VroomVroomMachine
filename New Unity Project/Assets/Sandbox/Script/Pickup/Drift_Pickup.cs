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
        
        if(rb.transform.eulerAngles.z < 0f)
        {
            orientation = 360 + rb.transform.eulerAngles.z;
        }
        else
        {
            orientation = rb.transform.eulerAngles.z;
        }

        Trail_FL.position = new Vector3(WT_FL.transform.position.x /*-((0.62f) * Mathf.Sin(orientation)) - 0.25f*/,WT_FL.transform.position.y - (0.62f * Mathf.Cos(orientation)*2*Mathf.PI),WT_FL.transform.position.z);
        Trail_FR.position = new Vector3(WT_FR.transform.position.x + ((0.62f) * Mathf.Sin(orientation)) + 0.25f, WT_FR.transform.position.y - (0.62f * Mathf.Cos(orientation)), WT_FR.transform.position.z);
        Trail_RL.position = new Vector3(WT_RL.transform.position.x - ((0.62f) * Mathf.Sin(orientation)) - 0.25f, WT_RL.transform.position.y - (0.62f * Mathf.Cos(orientation)), WT_RL.transform.position.z);
        Trail_RR.position = new Vector3(WT_RR.transform.position.x + ((0.62f) * Mathf.Sin(orientation)) + 0.25f, WT_RR.transform.position.y - (0.62f * Mathf.Cos(orientation)), WT_RR.transform.position.z);
         
    }
}
