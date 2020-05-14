using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tour_comlete : MonoBehaviour
{

    public static int tours = 0;
    public static bool boolComp;
	public static bool m_finActive = false;

    // Update is called once per frame
    void start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
		if (m_finActive == true)
		{
			if (other.tag == "car")
			{
				tours += 1;
				boolComp = true;
				Debug.Log(tours);
			}
		}
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "car")
        {
            boolComp = false;
			m_finActive = false;
        }
    }
}
