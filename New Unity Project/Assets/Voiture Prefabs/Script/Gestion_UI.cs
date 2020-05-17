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

    private float minuteMeilleurTemps;
    private float secondeMeilleurTemps;

    public TextMeshProUGUI txtMeilleurTemps;
    

    public TextMeshProUGUI txtTours;
    private int tour;

    public TextMeshProUGUI txtPosition;
    private int position = 1;

    GameObject joueur;

    private void Awake()
    {
        joueur = GameObject.Find("Vehicule Joueur 2.2");
        positionVoiture = joueur.GetComponentInChildren<PositionVoiture>();
    }

    void Update()
    {
        positionVoiture = joueur.GetComponentInChildren<PositionVoiture>();
        VitesseVoiture();
        Chrono();
        GestionDesTours();
        ChangerMeilleurTemps();
        ChangerPosition();
    }


    private void GestionDesTours()
    {
        tour = (int)positionVoiture.currentLapPos + 1;
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

        tempsActuelleTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00");

    }

    private void ChangerMeilleurTemps()
    {
        if (positionVoiture.tempsRapide == -5)
        {
            txtMeilleurTemps.text = "00 : " + positionVoiture.tempsRapide.ToString("0.00");
        }
        else
        {
            secondeMeilleurTemps = (positionVoiture.tempsRapide % 60f) % 100;
            minuteMeilleurTemps = (int)(positionVoiture.tempsRapide / 60f);
            txtMeilleurTemps.text = minuteMeilleurTemps.ToString("00") + " : " + secondeMeilleurTemps.ToString("00.00");
        }
        

    }

    private void ChangerPosition()
    {
        switch (positionVoiture.position)
        {
            case 1:
                txtPosition.text = "1er";
                break;
            case 2:
                txtPosition.text = "2e";
                break;
            case 3:
                txtPosition.text = "3e";
                break;
            case 4:
                txtPosition.text = "4e";
                break;
            default:
                break;
        }
        
    }
}
