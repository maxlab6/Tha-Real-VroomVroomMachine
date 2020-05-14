using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaceManager : MonoBehaviour
{

    public Transform[] allCars;
    public float[] carOrder;

    public void Start()
    {
        // set up the car objects
        carOrder = new float[allCars.Length];
        InvokeRepeating("ManualUpdate", 0.5f, 0.5f);
    }
    /*
    // this gets called every frame
    public void ManualUpdate()
    {
        foreach (Transform car in allCars)
        {
            carOrder[car.GetCarPos(allCars) - 1] = car;
        }
        Debug.Log(carOrder[0]);
    }
    */
}



