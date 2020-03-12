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
    public Vector3 PositionCameraPoursuiteAvancer;
    public Vector3 RotationCameraPoursuiteAvancer;

    public Vector3 PositionCameraPoursuiteReculer;
    public Vector3 RotationCameraPoursuiteReculer;

    public Vector3 PositionCameraTop;
    public Vector3 RotationCameraTop;

    public float VitesseTransition;
    public float VitesseChangementCamera;
    private float VitesseVoiture;

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

        //Obtient la vitesse du Script Controle_Voiture
        VitesseVoiture = Controle_Voiture.vitesse;

        if (CameraMultiple.enabled == true)
        {
            //Initialisation variable de rotation
            var rot = transform.rotation.eulerAngles;

            //Vérifie quelle caméra est active
            if (CamChase == true)
            {

                if (Input.GetKey(KeyCode.V))
                {
                    //Camera de poursuite reculon

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x + (PositionCameraPoursuiteReculer.x * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteReculer.y + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z + (PositionCameraPoursuiteReculer.z * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = RotationCameraPoursuiteReculer.x;
                    rot.y = RotationCameraPoursuiteReculer.y + CarToFollow.transform.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteReculer.z;                    
                }
                else if (VitesseVoiture > VitesseChangementCamera)
                {
                    //Camera de poursuite avancer

                    //Camera de poursuite reculon

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x - (PositionCameraPoursuiteAvancer.x * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteAvancer.y + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z - (PositionCameraPoursuiteAvancer.z * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera                               
                    rot.x = RotationCameraPoursuiteAvancer.x;
                    rot.y = RotationCameraPoursuiteAvancer.y + CarToFollow.transform.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteAvancer.z;
                }
                else
                {
                    //Camera de poursuite avancer

                    //Position de la camera
                    CamPos.x = CarToFollow.position.x + (PositionCameraPoursuiteReculer.x * Mathf.Sin(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteReculer.y + CarToFollow.position.y;
                    CamPos.z = CarToFollow.position.z + (PositionCameraPoursuiteReculer.z * Mathf.Cos(CarToFollow.transform.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = RotationCameraPoursuiteReculer.x;
                    rot.y = RotationCameraPoursuiteReculer.y + CarToFollow.transform.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteReculer.z;
                }
            }
            else if (CamTop == true)
            {
                //Camera avec une vue du haut.

                //Position de la camera
                CamPos.x = CarToFollow.position.x + PositionCameraTop.x;
                CamPos.y = CarToFollow.position.y + PositionCameraTop.y;
                CamPos.z = CarToFollow.position.z + PositionCameraTop.z;

                //Rotation de la camera           
                rot.x = RotationCameraTop.x;
                rot.y = RotationCameraTop.y + CarToFollow.transform.localEulerAngles.y;
                rot.z = RotationCameraTop.z; ;
            }


            //Attribue les valeurs défini à la position et la rotation.
            transform.position = Vector3.Lerp(transform.position, CamPos, VitesseTransition * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rot);
        }
        else if (CameraMultiple.enabled == false)
        {
            if (Input.GetKey(KeyCode.V))
            {
                CameraConducteurAvant.enabled = false;
                CameraConducteurArriere.enabled = true;
            }
            else if(VitesseVoiture > VitesseChangementCamera)
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
