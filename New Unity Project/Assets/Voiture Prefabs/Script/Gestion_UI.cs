using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gestion_UI : MonoBehaviour
{
    PositionVoiture positionVoiture;
    public Text txtVitesse;
    private float vitesseKMH;

    public Text tempsTxt;
    private float temps, tempsTempo;
    private float heure, minute, seconde;

    public Text txtTours;
    private int tour;


    private void Awake()
    {
        GameObject joueur = GameObject.Find("Vehicule Joueur 2.2");
        positionVoiture = joueur.GetComponentInChildren<PositionVoiture>();
    }

    void Update()
    {
        
        VitesseVoiture();
        Chrono();
        GestionDesTours();
    }


    private void GestionDesTours()
    {
        tour = positionVoiture.currentLapPos;
        txtTours.text = tour.ToString();
    }


    private void VitesseVoiture()
    {
        vitesseKMH = Mathf.Abs(Controle_Voiture.vitesse);

        txtVitesse.text = vitesseKMH.ToString("0") + " Km/h";
    }


    private void Chrono()
    {

        temps = positionVoiture.temps;
        seconde = (temps % 60f) % 100;
        minute = (int)(temps / 60f);

        Debug.Log("TEMPS ===" + temps);

        tempsTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00");

    }
}
