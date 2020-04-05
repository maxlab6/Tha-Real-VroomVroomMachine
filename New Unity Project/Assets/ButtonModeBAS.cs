using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonModeBAS : MonoBehaviour
{
    public void buttonBAC()
    {
        SceneManager.LoadScene("Sandbox", LoadSceneMode.Single);
    }
}
