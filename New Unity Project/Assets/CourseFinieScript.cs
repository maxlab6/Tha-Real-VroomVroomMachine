using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseFinieScript : MonoBehaviour
{
    public bool courseFinie;
    RaceManager raceManager_ = new RaceManager();
    GameObject[] posTextList;
    GameObject[] nameTextList;
    GameObject[] tempsTextList;


    void Start()
    {
        int i = 0;
        int pos = 0;
        string posText;
        string nomText;
        string tempsText;

        raceManager_ = GameObject.Find("RaceManager").GetComponent<RaceManager>();

        posTextList = new GameObject[raceManager_.allCars.Length];
        nameTextList = new GameObject[raceManager_.allCars.Length];
        tempsTextList = new GameObject[raceManager_.allCars.Length];

        for (i = 0; i < raceManager_.allCars.Length; i++)
        {
           
           pos = i + 1;
           posText = "PosText" + pos.ToString();
           nomText = "NomText" + pos.ToString();
           tempsText = "TempsText" + pos.ToString();
           posTextList[i] = GameObject.Find(posText);
           nameTextList[i] = GameObject.Find(nomText);
           tempsTextList[i] = GameObject.Find(tempsText);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (courseFinie == true)
        {
            int i = 0;
            int pos = 0;
            string posText;

            for (i = 0; i < raceManager_.allCars.Length; i++)
            {
                pos = i + 1;
                posText = "PosText" + pos.ToString();
                posTextList[i].GetComponent<UnityEngine.UI.Text>().text = pos.ToString();
            }
        }
    }
}
