using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contreLaMontreToggle : MonoBehaviour
{
    static public bool contreLaMontreBool;
    // Start is called before the first frame update
    void Awake()
    {
        contreLaMontreBool = GetComponent<UnityEngine.UI.Toggle>().isOn;
        Debug.Log(contreLaMontreBool);
    }



    public void onToggleNoAI(bool value)
    {
        contreLaMontreBool = value;
    }

}
