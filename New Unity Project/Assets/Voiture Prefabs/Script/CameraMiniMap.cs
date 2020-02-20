using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMiniMap : MonoBehaviour
{
    
    public Transform trans;
    
    public Vector3 offset;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = trans.position  - offset; 
        
    }
}
