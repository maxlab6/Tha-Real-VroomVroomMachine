using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
	public Button retour;
	public GameObject paneau;
	public static bool GameIsPause = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
		{
			Stop();
		}

		retour.onClick.AddListener(Continuer);

		
    }

	private void Continuer()
	{
		paneau.SetActive(false);
		Time.timeScale = 1f;
	}

	private void Stop ()
	{
		paneau.SetActive(true);
		Time.timeScale = 0f;
	}
}
