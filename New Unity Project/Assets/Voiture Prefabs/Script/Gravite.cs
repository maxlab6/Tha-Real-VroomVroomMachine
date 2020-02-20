using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravite : MonoBehaviour
{
    private Rigidbody rb;

    private string Wheel_FL_On;
    private string Wheel_FR_On;
    private string Wheel_RL_On;
    private string Wheel_RR_On;

    private bool Plancher;
    private bool Mur_Avant;
    private bool Mur_Droit;
    private bool Mur_Arriere;
    private bool Mur_Gauche;
    private bool Plafond;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Plancher = true;
        Mur_Avant = false;
        Mur_Droit = false;
        Mur_Arriere = false;
        Mur_Gauche = false;
        Plafond = false;
    }

    private void FixedUpdate()
    {
        Verification();

        if(Plancher == true)
        {
            //Debug.Log("Plancher");
            rb.AddForce(new Vector3(0, -9.8f * rb.mass, 0));
        }
        else if (Mur_Avant == true)
        {
            rb.AddForce(new Vector3(-9.8f * rb.mass, 0, 0));
        }
        else if (Mur_Droit == true)
        {
            rb.AddForce(new Vector3(0, 0, 9.8f * rb.mass));
        }
        else if (Mur_Arriere == true)
        {
            rb.AddForce(new Vector3(-9.8f * rb.mass, 0, 0));
        }
        else if (Mur_Gauche == true)
        {
            rb.AddForce(new Vector3(0, 0, -9.8f * rb.mass));
        }
        else if (Plafond == true)
        {
            rb.AddForce(new Vector3(0, 9.8f * rb.mass, 0));
        }
    }

    private void Verification()
    {
        Wheel_FL_On = Wheel_FL_Test.Wheel_FL;


        if(Wheel_FL_On == "Plancher")
        {
            Debug.Log("Plancher");
            Plancher = true;
            Mur_Avant = false;
            Mur_Droit = false;
            Mur_Arriere = false;
            Mur_Gauche = false;
            Plafond = false;
        }
        else if (Wheel_FL_On == "Mur Avant")
        {
            Debug.Log("Mur Avant");
            Plancher = false;
            Mur_Avant = true;
            Mur_Droit = false;
            Mur_Arriere = false;
            Mur_Gauche = false;
            Plafond = false;
        }
        else if (Wheel_FL_On == "Mur Droit")
        {
            Plancher = false;
            Mur_Avant = false;
            Mur_Droit = true;
            Mur_Arriere = false;
            Mur_Gauche = false;
            Plafond = false;
        }
        else if (Wheel_FL_On == "Mur Gauche")
        {
            Plancher = false;
            Mur_Avant = false;
            Mur_Droit = false;
            Mur_Arriere = false;
            Mur_Gauche = true;
            Plafond = false;
        }
        else if (Wheel_FL_On == "Mur Arrière")
        {
            Plancher = false;
            Mur_Avant = false;
            Mur_Droit = false;
            Mur_Arriere = true;
            Mur_Gauche = false;
            Plafond = false;
        }
        else if (Wheel_FL_On == "Plafond")
        {
            Plancher = false;
            Mur_Avant = false;
            Mur_Droit = false;
            Mur_Arriere = false;
            Mur_Gauche = false;
            Plafond = true;
        }
    }

}
