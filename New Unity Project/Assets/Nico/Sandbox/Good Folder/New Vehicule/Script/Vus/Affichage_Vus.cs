using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Affichage_Vus : MonoBehaviour
{
    private Text txtVitesse;
    private float vitesseKMH;
    private void Start()
    {
        txtVitesse = GameObject.Find("Ensemble Vus Jeu/Panel Vus/Canvas Vus/Vitesse/TxtVitesse").GetComponent<Text>();
    }
    void Update()
    {
        vitesseVoiture();
    }

    private void vitesseVoiture()
    {
        vitesseKMH = Mathf.Abs(Controle_Vus.vitesse);

        txtVitesse.text = vitesseKMH.ToString("0") + " Km/h";
    }
}
