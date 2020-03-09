using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Voiture : MonoBehaviour
{
    public Transform CarToFollow;

    public Camera CameraMultiple;
    public Camera CameraDriverAvancer;
    public Camera CameraDriverReculer;

    private Vector3 CamPos;

    private float orientation;

    private bool CamTop;
    private bool CamChase;


    private void Start()
    {
        CamTop = false;
        CamChase = true;

        CameraMultiple.enabled = true;
        CameraDriverAvancer.enabled = false;
        CameraDriverReculer.enabled = false;

    }

    private void FixedUpdate()
    {
        //Appel la fonction pour le choix de camera
        ChoixCamera();

        //Détermine si le véhicule avance ou recule.
        orientation = Controle_Voiture.vitesse;



        if (CameraMultiple.enabled == true)
        {
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
                    transform.position = Vector3.Lerp(transform.position, CamPos, 8f * Time.deltaTime);

                    //Rotation de la camera           
                    var rot = transform.rotation.eulerAngles;
                    rot.x = 30f;
                    rot.y = CarToFollow.transform.localEulerAngles.y;
                    rot.z = 0;
                    transform.rotation = Quaternion.Euler(rot);
                }
                else
                {
                    //Camera de poursuite reculon

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x + (15f * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = 10f + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z + (15f * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    transform.position = Vector3.Lerp(transform.position, CamPos, 8f * Time.deltaTime);

                    //Rotation de la camera           
                    var rot = transform.rotation.eulerAngles;
                    rot.x = 30f;
                    rot.y = CarToFollow.transform.localEulerAngles.y + 180;
                    rot.z = 0;
                    transform.rotation = Quaternion.Euler(rot);
                }


            }
            else if (CamTop == true)
            {
                //Camera avec une vue du haut.

                //Position de la camera
                CamPos.x = CarToFollow.position.x;
                CamPos.y = 30f + CarToFollow.position.y;
                CamPos.z = CarToFollow.position.z;
                transform.position = Vector3.Lerp(transform.position, CamPos, 8f * Time.deltaTime);

                //Rotation de la camera           
                var rot = transform.rotation.eulerAngles;
                rot.x = 90f;
                rot.y = CarToFollow.transform.localEulerAngles.y;
                rot.z = 0;
                transform.rotation = Quaternion.Euler(rot);


            }
        }
        else
        {
            //Active la camera conducteur avancer
            if (orientation > -10f)
            {
                CameraDriverAvancer.enabled = true;
                CameraDriverReculer.enabled = false;
            }
            //Active la camera conducteur reculer
            else
            {
                CameraDriverAvancer.enabled = false;
                CameraDriverReculer.enabled = true;
            }
        }
    }

    void ChoixCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera vue poursuite
            CameraMultiple.enabled = true;
            CameraDriverAvancer.enabled = false;
            CameraDriverReculer.enabled = false;
            CamChase = true;
            CamTop = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Camera vue du haut
            CameraMultiple.enabled = true;
            CameraDriverAvancer.enabled = false;
            CameraDriverReculer.enabled = false;
            CamChase = false;
            CamTop = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Camera vue conducteur
            CameraMultiple.enabled = false;
            CamChase = false;
            CamTop = false;
        }

    }
}
