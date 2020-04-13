using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMiniMap : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject Vus;    
    public Vector3 offset;

    // Update is called once per frame

    void FixedUpdate()
    {
        if(Pickup.activeSelf == true)
        {
            transform.position = Pickup.transform.position  - offset;
        }
        else if(Vus.activeSelf == true)
        {
            transform.position = Vus.transform.position - offset;
        }


         
        
    }
}
