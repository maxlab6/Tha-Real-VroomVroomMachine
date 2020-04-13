using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choix_Véhicule : MonoBehaviour
{
    public string Tour;
    //Menu
    public GameObject MenuPause;
    public GameObject MenuChoix;
    public GameObject Affichage;
    public GameObject CarViewer;
    //Dropdown
    public Dropdown Option_Vehicule;
    public Dropdown Option_Couleur_Principale;
    public Dropdown Option_Couleur_Secondaire;
    public Dropdown Option_Couleur_Roue;
    //Bouton

    public GameObject BoutonJouerStart;
    public GameObject BoutonJouerChoix;
    public GameObject BoutonRetourStart;
    public GameObject BoutonRetourChoix;

    public Text TitreChoixVehicule;

    //Véhicule
    public GameObject Pickup;
    public GameObject Vus;
    //RigidBody
    private Rigidbody Pickup_RigidBody;
    private Rigidbody Vus_RigidBody;
    //Véhicule Show
    public GameObject Pickup_Show;
    public GameObject Vus_Show;

    public GameObject Actif;
    public GameObject Choisi;

    //Materiaux
    public Material CouleurPrincipale;
    public Material CouleurSecondaire;
    public Material CouleurRoue;
    public Material CouleurAccentPrincipale;
    //Couleur Base
    private Color CouleurPrincipaleBase;
    private Color CouleurSecondaireBase;
    private Color CouleurRoueBase;
    //Couleur
    private Color Rouge;
    private Color Noir;
    private Color Vert;
    private Color Charcoal;
    private Color Or;

    public Transform Camera;
    public Transform LookAt;

    public Transform Position0;

    //Script Vus
    public Controle_Vus ScriptVus;
    public Friction_Roue_Vus ScriptFrictionVus_FL;
    public Friction_Roue_Vus ScriptFrictionVus_FR;
    public Friction_Roue_Vus ScriptFrictionVus_RL;
    public Friction_Roue_Vus ScriptFrictionVus_RR;

    //Script Pickup
    public Controle_Pickup ScriptPickup;
    public Friction_Roue_Pickup ScriptFrictionPickup_FL;
    public Friction_Roue_Pickup ScriptFrictionPickup_FR;
    public Friction_Roue_Pickup ScriptFrictionPickup_RL;
    public Friction_Roue_Pickup ScriptFrictionPickup_RR;


    void Start()
    {
        //Vus
        ScriptVus.enabled = false;
        ScriptFrictionVus_FL.enabled = false;
        ScriptFrictionVus_FR.enabled = false;
        ScriptFrictionVus_RL.enabled = false;
        ScriptFrictionVus_RR.enabled = false;

        //Pickup
        ScriptPickup.enabled = false;
        ScriptFrictionPickup_FL.enabled = false;
        ScriptFrictionPickup_FR.enabled = false;
        ScriptFrictionPickup_RL.enabled = false;
        ScriptFrictionPickup_RR.enabled = false;

        //Couleur Base
        CouleurPrincipaleBase = new Color32(255, 23, 0, 255);
        CouleurSecondaireBase = new Color32(44, 44, 44, 255);
        CouleurRoueBase = new Color32(168, 125, 34, 255);

        //Couleur
        Rouge = CouleurPrincipaleBase;
        Noir = new Color32(0, 0, 0, 255);
        Vert = new Color32(85, 107, 47, 255);
        Charcoal = CouleurSecondaireBase;
        Or = CouleurRoueBase;
        Tour = "Start";
    }

    void FixedUpdate()
    {     
        OptionVehicule();
        OptionCouleurPrincipale();
        OptionCouleurSecondaire();
        OptionCouleurRoue();
        MovementCamera();

        if(Tour == "Choix")
        {
            BoutonJouerStart.SetActive(false);
            BoutonJouerChoix.SetActive(true);
            BoutonRetourStart.SetActive(false);
            BoutonRetourChoix.SetActive(true);

            TitreChoixVehicule.text = "Modification du Véhicule";
            BoutonJouerChoix.GetComponent<Button>().onClick.AddListener(JouerChoix);
            BoutonRetourChoix.GetComponent<Button>().onClick.AddListener(RetourChoix);
        }
        else
        {
            BoutonJouerStart.SetActive(true);
            BoutonJouerChoix.SetActive(false);
            BoutonRetourStart.SetActive(true);
            BoutonRetourChoix.SetActive(false);

            TitreChoixVehicule.text = "Sélection du Véhicule";
            BoutonJouerStart.GetComponent<Button>().onClick.AddListener(JouerStart);
            BoutonRetourStart.GetComponent<Button>().onClick.AddListener(RetourStart);
        }


        if(Pickup.activeSelf == true)
        {            
            Actif = Pickup;
        }
        else if(Vus.activeSelf == true )
        {
            Actif = Vus;
        }

                  
    }

    private void OptionVehicule()
    {
        if(Option_Vehicule.value == 0)
        {   
            Vus_Show.SetActive(false);
            Pickup_Show.SetActive(true);
            Choisi = Pickup;
        }
        else if(Option_Vehicule.value == 1)
        {   
            Pickup_Show.SetActive(false);                  
            Vus_Show.SetActive(true);
            Choisi = Vus;
        }
        
    }

    private void OptionCouleurPrincipale()
    {
        if (Option_Couleur_Principale.value == 0)
        {         
            CouleurPrincipale.color = Rouge;
            CouleurAccentPrincipale.color = Rouge;

        }
        else if (Option_Couleur_Principale.value == 1)
        {           
            CouleurPrincipale.color = Noir;
            CouleurAccentPrincipale.color = Noir;
        }
        else if(Option_Couleur_Principale.value == 2)
        {
            CouleurPrincipale.color = Vert;
            CouleurAccentPrincipale.color = Vert;
        }
       
    }

    private void OptionCouleurSecondaire()
    {
        if (Option_Couleur_Secondaire.value == 0)
        {
            CouleurSecondaire.color = Charcoal;
        }
        else if (Option_Couleur_Secondaire.value == 1)
        {
            CouleurSecondaire.color = Noir;
        }
    }

    private void OptionCouleurRoue()
    {
        if(Option_Couleur_Roue.value == 0)
        {
            CouleurRoue.color = Or;
        }
        else if(Option_Couleur_Roue.value == 1)
        {
            CouleurRoue.color = Noir;
        }
    }

    private void MovementCamera()
    {
        if(Vus_Show.activeSelf == true)
        {
            Camera.LookAt(Vus_Show.transform);
        }
        else if(Pickup_Show.activeSelf == true)
        {
            Camera.LookAt(Pickup_Show.transform);
        }
        
        Camera.Translate(Vector3.right * Time.deltaTime*2);
    }

    private void JouerStart()
    {       
        PlacerVehicule(Choisi);
        ActiverScript(Choisi);
        Choisi.SetActive(true);
        MenuChoix.SetActive(false);
        CarViewer.SetActive(false);
        Affichage.SetActive(true);
        Tour = "Choix";
    }

    private void RetourStart()
    {
        MenuChoix.SetActive(false);
        CarViewer.SetActive(false);
    }

    private void JouerChoix()
    {
        if(Actif != Choisi)
        {
            Choisi.SetActive(true);
            ChangerVehicule(Choisi, Actif);
            ActiverScript(Choisi);
            Actif.SetActive(false);
        }      
        MenuChoix.SetActive(false);
        CarViewer.SetActive(false);
        Affichage.SetActive(true);
    }

    private void RetourChoix()
    {
        MenuChoix.SetActive(false);
        CarViewer.SetActive(false);
        MenuPause.SetActive(true);
        Time.timeScale = 0f;
    }

    private void PlacerVehicule(GameObject _vehicule)
    {
        _vehicule.GetComponent<Rigidbody>().MovePosition(new Vector3(Position0.position.x, Position0.position.y + 1f, Position0.position.z));
        _vehicule.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, 0f, 0f));
        _vehicule.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        _vehicule.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);                  
    }

    private void ActiverScript(GameObject _vehicule)
    {
        if(_vehicule == Pickup)
        {
            //Activer
            ScriptPickup.enabled = true;
            ScriptFrictionPickup_FL.enabled = true;
            ScriptFrictionPickup_FR.enabled = true;
            ScriptFrictionPickup_RL.enabled = true;
            ScriptFrictionPickup_RR.enabled = true;
            //Désactiver
            ScriptVus.enabled = false;
            ScriptFrictionVus_FL.enabled = false;
            ScriptFrictionVus_FR.enabled = false;
            ScriptFrictionVus_RL.enabled = false;
            ScriptFrictionVus_RR.enabled = false;

        }
        else if(_vehicule == Vus)
        {
            //Activer
            ScriptVus.enabled = true;
            ScriptFrictionVus_FL.enabled = true;
            ScriptFrictionVus_FR.enabled = true;
            ScriptFrictionVus_RL.enabled = true;
            ScriptFrictionVus_RR.enabled = true;
            //Désactiver
            ScriptPickup.enabled = false;
            ScriptFrictionPickup_FL.enabled = false;
            ScriptFrictionPickup_FR.enabled = false;
            ScriptFrictionPickup_RL.enabled = false;
            ScriptFrictionPickup_RR.enabled = false;
        }
         
    }

    private void ChangerVehicule(GameObject _Nouveau, GameObject _Ancien)
    {               
        _Nouveau.GetComponent<Rigidbody>().MovePosition(new Vector3(_Ancien.transform.position.x, _Ancien.transform.position.y+1f, _Ancien.transform.position.z));
        _Nouveau.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(_Ancien.transform.transform.localEulerAngles.x, _Ancien.transform.transform.localEulerAngles.y, _Ancien.transform.transform.localEulerAngles.z));
        _Nouveau.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
        _Nouveau.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f,0f,0f);      
    }
}
