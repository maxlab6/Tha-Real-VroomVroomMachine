using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tour_comlete : MonoBehaviour
{

    public static int tours = 1;
    public static bool boolComp;

    // Update is called once per frame
    void start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            tours += 1;
            boolComp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "car")
        {
            boolComp = false;
        }
    }
}
