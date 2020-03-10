using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Multiple : MonoBehaviour
{
    public Transform CarToFollow;

    public Camera CameraMultiple;
    public Camera CameraConducteurAvant;
    public Camera CameraConducteurArriere;

    private Vector3 CamPos;

    private float orientation;

    private bool CamTop;
    private bool CamChase;


    private void Start()
    {
        CamTop = false;
        CamChase = true;

        CameraMultiple.enabled = true;
        CameraConducteurAvant.enabled = false;
        CameraConducteurArriere.enabled = false;

    }

    private void FixedUpdate()
    {
        //Appel la fonction pour le choix de camera
        ChoixCamera();

         //Détermine si le véhicule avance ou recule.
         orientation = Controle_Voiture.vitesse;
        
        if (CameraMultiple.enabled == true)
        {
            //Initialisation variable de rotation
            var rot = transform.rotation.eulerAngles;

            //Vérifie quelle caméra est active
            if (CamChase == true)
            {
                if (orientation > -10f)
                {
                    //Camera de poursuite avancer

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x - (15f * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = 10f + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z - (15f * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera                               
                    rot.x = 30f;
                    rot.y = CarToFollow.transform.localEulerAngles.y;
                    rot.z = 0;                
                }
                else
                {
                    //Camera de poursuite reculon

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x + (15f * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = 10f + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z + (15f * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = 30f;
                    rot.y = CarToFollow.transform.localEulerAngles.y + 180;
                    rot.z = 0;
                }
            }
            else if (CamTop == true)
            {
                //Camera avec une vue du haut.

                //Position de la camera
                CamPos.x = CarToFollow.position.x;
                CamPos.y = 30f + CarToFollow.position.y;
                CamPos.z = CarToFollow.position.z;

                //Rotation de la camera           
                rot.x = 90f;
                rot.y = CarToFollow.transform.localEulerAngles.y;
                rot.z = 0;
            }


            //Attribue les valeurs défini à la position et la rotation.
            transform.position = Vector3.Lerp(transform.position, CamPos, 8f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rot);
        }
        else if (CameraMultiple.enabled == false)
        {
            if(orientation > -10f)
            {
                CameraConducteurAvant.enabled = true;
                CameraConducteurArriere.enabled = false;
            }
            else
            {
                CameraConducteurAvant.enabled = false;
                CameraConducteurArriere.enabled = true;
            }
        }
    }


    void ChoixCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera vue poursuite
            CamChase = true;
            CamTop = false;

            CameraMultiple.enabled = true;        
            CameraConducteurAvant.enabled = false;
            CameraConducteurArriere.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Camera vue du haut
            CamChase = false;
            CamTop = true;

            CameraMultiple.enabled = true;
            CameraConducteurAvant.enabled = false;
            CameraConducteurArriere.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Camera vue conducteur
            CamChase = false;
            CamTop = false;

            CameraMultiple.enabled = false;
        }

    }
}
