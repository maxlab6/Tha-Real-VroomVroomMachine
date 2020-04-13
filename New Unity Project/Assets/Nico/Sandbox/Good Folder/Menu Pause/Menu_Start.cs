using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Start : MonoBehaviour
{
    public Transform Position0;

    //Dropdown
    public Dropdown Option_Vehicule;
    public Dropdown Option_Couleur_Principale;
    public Dropdown Option_Couleur_Secondaire;
    public Dropdown Option_Couleur_Roue;

    //Bouton
    public Button BoutonJouer;

    //Materiaux
    public Material CouleurPrincipale;
    public Material CouleurSecondaire;
    public Material CouleurRoue;
    public Material CouleurAccentPrincipale;

    //Véhicule
    public GameObject Pickup;
    public GameObject Vus;

    //Véhicule Show
    public GameObject Pickup_Show;
    public GameObject Vus_Show;

    //RigidBody
    private Rigidbody Pickup_RigidBody;
    private Rigidbody Vus_RigidBody;

    //Couleur Base
    private Color CouleurPrincipaleBase;
    private Color CouleurSecondaireBase;
    private Color CouleurRoueBase;
    private Color CouleurAccentPrincipaleBase;

    //Couleur
    private Color Rouge;
    private Color Noir;
    private Color Vert;
    private Color Charcoal;
    private Color Or;

    public Transform Camera;
    public Transform LookAt;

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
        TrouverObjets();

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

        Pickup.SetActive(false);
        Vus.SetActive(false);

        Pickup_Show.SetActive(false);
        Vus_Show.SetActive(false);
        
        Option_Vehicule.value = 0;
        Option_Couleur_Principale.value = 0;
        Option_Couleur_Secondaire.value = 0;
        Option_Couleur_Roue.value = 0;
    }

    void FixedUpdate()
    {
        OptionVehicule();
        OptionCouleurPrincipale();
        OptionCouleurSecondaire();
        OptionCouleurRoue();
        MovementCamera();
        BoutonJouer.onClick.AddListener(PlacerVehicule);
    }

    private void TrouverObjets()
    {
        //RigidBody
        Pickup_RigidBody = Pickup.GetComponent<Rigidbody>();
        Vus_RigidBody = Vus.GetComponent<Rigidbody>();
     
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
    }

    private void OptionVehicule()
    {
        if (Option_Vehicule.value == 0)
        {
            Vus_Show.SetActive(false);
            Pickup_Show.SetActive(true);

        }
        else if (Option_Vehicule.value == 1)
        {
            Pickup_Show.SetActive(false);
            Vus_Show.SetActive(true);
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
        else if (Option_Couleur_Principale.value == 2)
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
        if (Option_Couleur_Roue.value == 0)
        {
            CouleurRoue.color = Or;
        }
        else if (Option_Couleur_Roue.value == 1)
        {
            CouleurRoue.color = Noir;
        }
    }

    private void MovementCamera()
    {
        if (Vus_Show.activeSelf == true)
        {
            Camera.LookAt(Vus_Show.transform);
        }
        else if (Pickup_Show.activeSelf == true)
        {
            Camera.LookAt(Pickup_Show.transform);
        }

        Camera.Translate(Vector3.right * Time.deltaTime * 2);
    }

    private void PlacerVehicule()
    {
        if(Option_Vehicule.value == 0)
        {   
            Pickup_RigidBody.MovePosition(new Vector3(Position0.position.x, Position0.position.y + 2f, Position0.position.z));
            Pickup_RigidBody.MoveRotation(Quaternion.Euler(0f, 0f, 0f));
            Pickup.SetActive(true);
            Pickup_RigidBody.velocity = new Vector3(0f, 0f, 0f);
            Pickup_RigidBody.angularVelocity = new Vector3(0f, 0f, 0f);

            ScriptPickup.enabled = true;
            ScriptFrictionPickup_FL.enabled = true;
            ScriptFrictionPickup_FR.enabled = true;
            ScriptFrictionPickup_RL.enabled = true;
            ScriptFrictionPickup_RR.enabled = true;

        }
        else if(Option_Vehicule.value == 1)
        {
            Vus_RigidBody.MovePosition(new Vector3(Position0.position.x,Position0.position.y + 2f,Position0.position.z ));
            Vus_RigidBody.MoveRotation(Quaternion.Euler(0f, 0f, 0f));
            Vus.SetActive(true);
            Vus_RigidBody.velocity = new Vector3(0f, 0f, 0f);
            Vus_RigidBody.angularVelocity = new Vector3(0f, 0f, 0f);

            ScriptVus.enabled = true;
            ScriptFrictionVus_FL.enabled = true;
            ScriptFrictionVus_FR.enabled = true;
            ScriptFrictionVus_RL.enabled = true;
            ScriptFrictionVus_RR.enabled = true;
        }
    }
}
