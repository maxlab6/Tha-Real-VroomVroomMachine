using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public PositionVoiture[] allCars;
    public PositionVoiture[] carOrder;
    public bool joueurAFini = false;
    public int toursTotaux = 2;
    public GameObject HUD;
    public GameObject MenuFinale;
    
    public void Start()
    {
        Debug.Log(contreLaMontreToggle.contreLaMontreBool);
        if(contreLaMontreToggle.contreLaMontreBool == true)
        {
            for(int i=1; i < allCars.Length; i++)
            {
                Destroy(allCars[i].transform.parent.gameObject);
            }
        }

        // set up the car objects
        carOrder = new PositionVoiture[allCars.Length];
        InvokeRepeating("ManualUpdate", 0.5f, 0.5f);
    }

  
    // this gets called every frame
    public void ManualUpdate()
    {
        int nbVoituresFinis = 0;
        for (int i = 0; i < allCars.Length; i++)
        {
            if (allCars[i].currentLapPos >= toursTotaux)
            {
                nbVoituresFinis++;
                allCars[i].aTerminerCourse = true;
            }

            if (allCars[0].currentLapPos >= toursTotaux)
            {
                joueurAFini = true;
            }
        }

        if(joueurAFini)
        {
            HUD.SetActive(false);
            MenuFinale.GetComponent<CanvasGroup>().alpha = 1;
            CourseFinieScript.courseFinie = true;
        }

        foreach (PositionVoiture car in allCars)
        {
            carOrder[car.GetCarPos(allCars) - 1] = car;
        }
    }
}