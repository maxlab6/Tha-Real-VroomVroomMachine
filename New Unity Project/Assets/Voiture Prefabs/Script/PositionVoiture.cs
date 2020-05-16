using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionVoiture : MonoBehaviour
{
    public int currentWaypointPos;
    public int counterWaypointPos;
    public int currentLapPos;
    public Transform lastWaypointPos;
    public bool aTerminerCourse = false;
    public bool aTerminerTour = false;
    
    public int position;

    public float temps, tempsTempo;
    
    public float tempsRapide = 0;
    private float heure, minute, seconde;
    

    public void OnTriggerEnter(Collider col)
    {
        string tag = col.gameObject.tag;

        if ("waypoints" == tag)
        {
            currentWaypointPos = System.Convert.ToInt32(col.gameObject.name);
            {
                if (currentWaypointPos == 0 && counterWaypointPos == GameObject.Find("Waypoints").transform.childCount)
                {
                    counterWaypointPos = 0;
                    currentLapPos++;
                }

                if (counterWaypointPos == currentWaypointPos)
                {
                    lastWaypointPos = col.transform;
                    counterWaypointPos++;
                }
            }
        }
    }

    public float GetDistancePos()
    {
        return (transform.position - lastWaypointPos.position).magnitude + currentWaypointPos * 100 + currentLapPos * (100 * GameObject.Find("Waypoints").transform.childCount);
    }

    public int GetCarPos(PositionVoiture[] allCars)
    {
        float distance = GetDistancePos();
        int _position = 1;
        foreach (PositionVoiture car in allCars)
        {
            if (car.GetDistancePos() > distance)
                position++;
        }
        position = _position;
        return position;
    }


    private void Update()
    {
       
        if(aTerminerCourse == false)
        {
            Chrono();
        }
            
    }

   
    public void Chrono()
    {

        if (currentWaypointPos == 0 && counterWaypointPos == GameObject.Find("Waypoints").transform.childCount)
        {
            Debug.Log("finisTour");
            tempsTempo = temps;
            if (tempsRapide > tempsTempo)
            {
                tempsRapide = tempsTempo;
            }
            
        }
        else
        {
            temps += Time.deltaTime;
            seconde = (temps % 60f) % 100;
            minute = (int)(temps / 60f);

        }
    }
}
