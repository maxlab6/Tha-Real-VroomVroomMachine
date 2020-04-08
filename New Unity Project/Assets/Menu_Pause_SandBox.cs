using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Pause_SandBox : MonoBehaviour
{
	public Button Retour;
	public Button ModifierVoiture;
	public Button Options;
	public Button MenuPrincipale;
	public Button Quitter;
	public GameObject paneau;
	public static bool GameIsPause = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			Stop();
		}

		Retour.onClick.AddListener(Continuer);


	}

	private void Continuer()
	{
		paneau.SetActive(false);
		Time.timeScale = 1f;
	}

	private void Stop()
	{
		paneau.SetActive(true);
		Time.timeScale = 0f;
	}
}
