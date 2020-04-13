using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Pause_SandBox : MonoBehaviour
{
	//Menu
	public GameObject MenuPause;
	public GameObject MenuChoix;

	//Affichage
	public GameObject AffichageVehicule;	
	public GameObject CarViewer;

	//Menu Pause
	public Button Pause_Retour;
	public Button Pause_ModifierVoiture;
	public Button Pause_Options;
	public Button Pause_MenuPrincipale;
	public Button Pause_Quitter;
					
	//Véhicule 
	public GameObject Pickup;
	public GameObject Vus;
	


	void Start()
	{
		//Désactive les Véhicule
		Pickup.SetActive(false);
		Vus.SetActive(false);

		//Ouvre le menu Choix
		MenuChoix.SetActive(true);
		CarViewer.SetActive(true);
				

		AffichageVehicule.SetActive(false);
		MenuPause.SetActive(false);											
	}

	void FixedUpdate()
	{
		if(Pickup.activeSelf == true || Vus.activeSelf == true)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				MenuPause.SetActive(true);
				AffichageVehicule.SetActive(false);
				Time.timeScale = 0f;
			}
			//Menu Pause
			Pause_Retour.onClick.AddListener(RetourPause);
			Pause_ModifierVoiture.onClick.AddListener(ModifierVoiturePause);
			Pause_Options.onClick.AddListener(OptionPause);
			Pause_MenuPrincipale.onClick.AddListener(MenuPrincipalePause);
			Pause_Quitter.onClick.AddListener(QuitterPause);	
		}												
	}

	private void TrouverObjet()
	{
		/*
		ScriptVus = GetComponent<Controle_Vus>();
		ScriptAffichageVus = GetComponent<Affichage_Vus>();
		ScriptFrictionVus = GetComponent<Friction_Roue_Vus>();

		ScriptPickup = GetComponent<Controle_Pickup>();
		ScriptAffichagePickup = GetComponent<Affichage_Pickup>();
		ScriptFrictionPickup = GetComponent<Friction_Roue_Pickup>();

		ScriptStart = GetComponent<Menu_Start>();
		ScriptChoix = GetComponent<Choix_Véhicule>();
		

		//Menu Start
		Start_Jouer = GameObject.Find("Accessoires SandBox/Canvas Start/Bouton Retour").GetComponent<Button>();
		Start_Retour = GameObject.Find("Accessoires SandBox/Canvas Start/Bouton Jouer").GetComponent<Button>();

		//Menu Pause
		Pause_Retour = GameObject.Find("Accessoires SandBox/Canvas Menu Pause/Bouton Retour").GetComponent<Button>();
		Pause_ModifierVoiture = GameObject.Find("Accessoires SandBox/Canvas Menu Pause/Bouton Modifier Véhicule").GetComponent<Button>();
		Pause_Options = GameObject.Find("Accessoires SandBox/Canvas Menu Pause/Bouton Options").GetComponent<Button>();
		Pause_MenuPrincipale = GameObject.Find("Accessoires SandBox/Canvas Menu Pause/Bouton Menu Principale").GetComponent<Button>();
		Pause_Quitter = GameObject.Find("Accessoires SandBox/Canvas Menu Pause/Bouton Quitter").GetComponent<Button>();

		//Menu Choix Véhicule
		Choix_Retour = GameObject.Find("Accessoires SandBox/Canvas Choix Véhicule/Bouton Retour").GetComponent<Button>();
		Choix_Jouer = GameObject.Find("Accessoires SandBox/Canvas Choix Véhicule/Bouton Jouer").GetComponent<Button>();

		//Affichage
		MenuStart = GameObject.Find("Accessoires SandBox/Canvas Start");
		AffichageVehicule = GameObject.Find("Accessoires SandBox/Affichage Véhicule");
		CanvasMenuPause = GameObject.Find("Accessoires SandBox/Canvas Menu Pause");
		CanvasChoixVehicule = GameObject.Find("Accessoires SandBox/Canvas Choix Véhicule");
		CarViewer = GameObject.Find("Accessoires SandBox/CarViewer");
		*/


	}

	private void RetourPause()
	{
		MenuPause.SetActive(false);		
		MenuChoix.SetActive(false);
		AffichageVehicule.SetActive(true);
		Time.timeScale = 1f;
	}

	private void ModifierVoiturePause()
	{
		MenuPause.SetActive(false);
		
		MenuChoix.SetActive(true);
		CarViewer.SetActive(true);

		Time.timeScale = 1f;
	}

	private void OptionPause()
	{

	}

	private void MenuPrincipalePause()
	{

	}

	private void QuitterPause()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
	}
}
