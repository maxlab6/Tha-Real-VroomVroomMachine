using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gestion_UI : MonoBehaviour
{
    public Text txtVitesse;
    private float vitesseKMH;

    public Text tempsTxt;
    private float temps, tempsTempo;
    private float heure, minute, seconde;

    public Text txtTours;
    private int tour;

    
    void Update()
    {
        vitesseVoiture();
        chrono();
        gestionDesTours();
    }


    private void gestionDesTours()
    {
        tour = tour_comlete.tours;
        txtTours.text = tour.ToString();
    }


    private void vitesseVoiture()
    {
        vitesseKMH = Mathf.Abs(Mouvement.vitesse);

        txtVitesse.text = vitesseKMH.ToString("0") + " Km/h";
    }


    private void chrono()
    {


        if(tour_comlete.boolComp == true)
        {
            tempsTempo += Time.deltaTime;
            tempsTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00");
        }
        else
        {

            temps += Time.deltaTime + tempsTempo;
            seconde = (temps % 60f) % 100;
            minute = (int)(temps / 60f);

            tempsTempo = 0.0f;
            tempsTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00");
        }
    }
}
