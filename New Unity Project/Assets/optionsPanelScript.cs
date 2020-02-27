using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class optionsPanelScript : MonoBehaviour
{
    public AudioMixer mixer1;

    public void SetMusicVolume(float sliderValue)
    {
        mixer1.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
