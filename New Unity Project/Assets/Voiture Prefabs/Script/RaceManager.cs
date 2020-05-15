using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public PositionVoiture[] allCars;
    public PositionVoiture[] carOrder;
    
    public void Start()
    {
        // set up the car objects
        carOrder = new PositionVoiture[allCars.Length];
        InvokeRepeating("ManualUpdate", 0.5f, 0.5f);
    }
    
    // this gets called every frame
    public void ManualUpdate()
    {
        foreach (PositionVoiture car in allCars)
        {
            carOrder[car.GetCarPos(allCars) - 1] = car;
        }
        Debug.Log(carOrder[0]);
    }
}