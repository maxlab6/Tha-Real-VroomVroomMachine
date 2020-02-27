using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Temps : MonoBehaviour
{

    public Text tempsTxt;
    private float temps;
    

    private float heure, minute, seconde, miliseconde;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temps +=  Time.deltaTime;
        miliseconde = temps % 100f;
        seconde =  (temps % 60f) % 100;
        minute = (int) (temps / 60f);

        tempsTxt.text = minute.ToString("00") + " : " + seconde.ToString("00.00")/* + miliseconde.ToString()*/;
        
        
    }
}
