using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Affichage : MonoBehaviour
{
    public GameObject Pickup;
    public GameObject Vus;

    public Text txtVitesse;
    private void Start()
    {
        //txtVitesse = GameObject.Find("SanbBox/Accessoires SandBox/Affichage Véhicule/Vitesse/TxtVitesse").GetComponent<Text>();
    }
    private void FixedUpdate()
    {
        if(Pickup.activeSelf == true)
        {
            txtVitesse.text = Mathf.Abs(Controle_Pickup.vitesse).ToString("0") + " Km/h";
        }
        else if(Vus.activeSelf == true)
        {
            txtVitesse.text = Mathf.Abs(Controle_Vus.vitesse).ToString("0") + " Km/h";
        }
    }
}
