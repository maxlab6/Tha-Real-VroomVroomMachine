using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseFinieScript : MonoBehaviour
{
    bool courseFinie;
    RaceManager raceManager_ = new RaceManager();


    // Update is called once per frame
    void Update()
    {
        if (courseFinie == true)
        {
            raceManager_ = GameObject.Find("RaceManager").GetComponent<RaceManager>();
            int i = 0;
            string posText;

            while (true)
            {
                posText = "PosText" + i.ToString() + 1;
                this.transform.Find(posText).GetComponent<UnityEngine.UI.Text>().text = posText.Remove(0, 7);  
            }
        }
    }
}
