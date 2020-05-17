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


    private void Start()
    {
        name = transform.parent.name;
    }

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
                    aTerminerTour = true;
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
        float result;
        if (lastWaypointPos == null)
        {
            lastWaypointPos = GameObject.Find("Waypoints").transform;
        }
        
        result = (transform.position - lastWaypointPos.position).magnitude + currentWaypointPos * 100 + currentLapPos * (100 * GameObject.Find("Waypoints").transform.childCount);
        return result;
    }

    public int GetCarPos(PositionVoiture[] allCars)
    {
        if (aTerminerCourse == false)
        {
            float distance = GetDistancePos();
            int _position = 1;
            foreach (PositionVoiture car in allCars)
            {
                if (car.GetDistancePos() > distance)
                    _position++;
            }
            position = _position;
            return _position;
        }
        return 0;
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

        if (aTerminerTour == true)
        {
            aTerminerTour = false;
            tempsTempo = temps;
            if (tempsRapide > tempsTempo || tempsRapide == 0)
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
