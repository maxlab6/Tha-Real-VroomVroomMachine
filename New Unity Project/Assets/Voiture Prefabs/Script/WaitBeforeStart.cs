using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBeforeStart : MonoBehaviour
{
    public GameObject countDown;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartCountDown");
    }

    IEnumerator StartCountDown()
    {
        Time.timeScale = 0;
        float pausedTime = Time.realtimeSinceStartup + 4f;
        while (Time.realtimeSinceStartup < pausedTime)
        {
            yield return 0;
        }


        countDown.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
