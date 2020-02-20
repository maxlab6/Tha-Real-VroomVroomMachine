using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_FL_Test : MonoBehaviour
{

    public static string Wheel_FL;
    public Transform wheel;

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Plancher");
        if(collision.gameObject.transform.tag == "Plancher")
        {
            Wheel_FL = "Plancher";
        }       
        else if(collision.gameObject.transform.tag == "Mur Avant")
        {
            Wheel_FL = "Mur Avant";
        }
        else if(collision.gameObject.transform.tag == "Mur Droit")
        {
            Wheel_FL = "Mur Droit";
        }
        else if (collision.gameObject.transform.tag == "Mur Gauche")
        {
            Wheel_FL = "Mur Gauche";
        }
        else if (collision.gameObject.transform.tag == "Mur Arrière")
        {
            Wheel_FL = "Mur Arrière";
        }
        else if (collision.gameObject.transform.tag == "Plafond")
        {
            Wheel_FL = "Plafond";
        }
        
    }
}
