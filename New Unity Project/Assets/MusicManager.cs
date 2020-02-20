/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    private bool SongLoaded;
    void Awake()
    {

        MakeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {

        if (!PlayerPrefs.HasKey("Game Initialized"))
        {
            MusicState.SetMusicState(1);

            PlayerPrefs.SetInt(" Game Initialized", 123);
        }

    }




    void MakeSingleton()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void PlayMusic(bool play)
    {


        if (play)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
            }
            audioSource.Stop();
        }


    }
}
*/