using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonQuitter : MonoBehaviour
{
    public void buttonMenuP()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void buttonQuitter()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit ();
    #endif
    }
}
