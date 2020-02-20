using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Temps : MonoBehaviour
{

    public Text tempsTxt;

    private float heure, minute, seconde;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        seconde = (int)(Time.deltaTime % 60f);
        minute = (int)(Time.deltaTime / 60f);
        heure = (int)(Time.deltaTime / 360f);
        tempsTxt.text = heure.ToString() + " : " + minute.ToString() + " : " + seconde.ToString();
    }
}
