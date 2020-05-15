using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CourseFinieScript : MonoBehaviour
{
    static public bool courseFinie;
    RaceManager raceManager_ = new RaceManager();
    GameObject[] posTextList;
    GameObject[] nomTextList;
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
        nomTextList = new GameObject[raceManager_.allCars.Length];
        tempsTextList = new GameObject[raceManager_.allCars.Length];

        for (i = 0; i < raceManager_.allCars.Length; i++)
        {
           
           pos = i + 1;
           posText = "PosText" + pos.ToString();
           nomText = "NomText" + pos.ToString();
           tempsText = "TempsText" + pos.ToString();
           posTextList[i] = GameObject.Find(posText);
           nomTextList[i] = GameObject.Find(nomText);
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
                posTextList[i].GetComponent<TextMeshProUGUI>().text = pos.ToString();

                tempsTextList[i].GetComponent<TextMeshProUGUI>().text = raceManager_.carOrder[i].temps.ToString();

                nomTextList[i].GetComponent<TextMeshProUGUI>().text = raceManager_.carOrder[i].transform.parent.name;

            }
        }
    }

    public void OnMouseButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
