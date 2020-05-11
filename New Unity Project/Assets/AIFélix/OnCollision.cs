using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public bool isColliding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "car")
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "car")
        {
            isColliding = false;
        }
    }

    public bool getIsColliding()
    {
        return isColliding;
    }
}
