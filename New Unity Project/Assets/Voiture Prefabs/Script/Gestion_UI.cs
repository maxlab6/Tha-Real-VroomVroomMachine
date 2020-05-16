using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gestion_UI : MonoBehaviour
{
    PositionVoiture positionVoiture;
    public TextMeshProUGUI txtVitesse;
    private float vitesseKMH;

    public TextMeshProUGUI tempsActuelleTxt;
    private float temps;
    private float minute, seconde;

    public TextMeshProUGUI txtMeilleurTemps;
    

    public TextMeshProUGUI txtTours;
    private int tour;

    public TextMeshProUGUI txtPosition;
    private int position = 1;


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
        tour = positionVoiture.currentLapPos + 1;
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

        tempsActuelleTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00");

    }

    private void ChangerMeilleurTemps()
    {
        
            txtMeilleurTemps.text = positionVoiture.tempsRapide.ToString("0.00");
        
        
    }

    private void ChangerPosition()
    {
        txtPosition.text = positionVoiture.position.ToString();
    }
}
