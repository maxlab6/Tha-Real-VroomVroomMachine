using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup_UI : MonoBehaviour
{
    public Text txtVitesse;
    private float vitesseKMH;

    void Update()
    {
        vitesseVoiture();
    }

    private void vitesseVoiture()
    {
        vitesseKMH = Mathf.Abs(Controle_Pickup.vitesse);

        txtVitesse.text = vitesseKMH.ToString("0") + " Km/h";
    }

}
