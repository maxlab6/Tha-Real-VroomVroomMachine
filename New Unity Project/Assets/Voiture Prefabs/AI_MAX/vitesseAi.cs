﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vitesseAi : MonoBehaviour
{

    private Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        txt.text = VoitureAi_Max.vitesse.ToString("0") + " km/h";

    }


}
