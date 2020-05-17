using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    //Pickup
    public GameObject Pickup;
    public Camera PickupDriverFrontView;
    public Camera PickupDriverRearView;

    //Vus    
    public GameObject Vus;
    public Camera VusDriverFrontView;
    public Camera VusDriverRearView;

    //Position Véhicule Actif
    private float vitesse;
    

    //Camera Controler
    public Camera CameraMultiple;
    private Camera CameraConducteurAvant;
    private Camera CameraConducteurArriere;

    private Vector3 CamPos;
    public Vector3 PositionCameraPoursuiteAvancer;
    public Vector3 RotationCameraPoursuiteAvancer;

    public Vector3 PositionCameraPoursuiteReculer;
    public Vector3 RotationCameraPoursuiteReculer;

    public Vector3 PositionCameraTop;
    public Vector3 RotationCameraTop;

    public float VitesseTransition;
    public float VitesseChangementCamera;

    private bool CamTop;
    private bool CamChase;


    // Start is called before the first frame update
    void Start()
    {
        //TrouverObjet();
        CamChase = true;
        CamTop = false;

        CameraMultiple.enabled = true;
        //CameraConducteurAvant.enabled = false;
        //CameraConducteurArriere.enabled = false;
        PickupDriverFrontView.enabled = false;
        PickupDriverRearView.enabled = false;
        VusDriverFrontView.enabled = false;
        VusDriverRearView.enabled = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(Pickup.activeSelf == true)
        {
            PositionCamera(Pickup.transform);
            CameraConducteurAvant = PickupDriverFrontView;
            CameraConducteurArriere = PickupDriverRearView;
            vitesse = Controle_Pickup.vitesse;
        }
        else if(Vus.activeSelf == true)
        {
            PositionCamera(Vus.transform);
            CameraConducteurAvant = VusDriverFrontView;
            CameraConducteurArriere = VusDriverRearView;
            vitesse = Controle_Vus.vitesse;
        }
    }

    private void TrouverObjet()
    {
        VusDriverFrontView = GameObject.Find("SandBox/Vus Jeu/Driver_Front_View").GetComponent<Camera>();
        VusDriverRearView = GameObject.Find("SandBox/Vus Jeu/Driver_Rear_View").GetComponent<Camera>();

        PickupDriverFrontView = GameObject.Find("SandBox/Pickup Jeu/Driver_Front_View").GetComponent<Camera>();
        PickupDriverRearView = GameObject.Find("SandBox/Pickup Jeu/Driver_Rear_View").GetComponent<Camera>();
    }

    private void PositionCamera(Transform _positionVehicule)
    {
        //Détermine la bonne caméra
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
                    CamPos.x = _positionVehicule.position.x + (PositionCameraPoursuiteReculer.x * Mathf.Sin(_positionVehicule.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteReculer.y + _positionVehicule.position.y;
                    CamPos.z = _positionVehicule.position.z + (PositionCameraPoursuiteReculer.z * Mathf.Cos(_positionVehicule.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = RotationCameraPoursuiteReculer.x;
                    rot.y = RotationCameraPoursuiteReculer.y + _positionVehicule.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteReculer.z;
                }
                else if (vitesse > VitesseChangementCamera)
                {
                    //Camera de poursuite avancer

                    //Position de la camera
                    CamPos.x = _positionVehicule.position.x - (PositionCameraPoursuiteAvancer.x * Mathf.Sin(_positionVehicule.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteAvancer.y + _positionVehicule.position.y;
                    CamPos.z = _positionVehicule.position.z - (PositionCameraPoursuiteAvancer.z * Mathf.Cos(_positionVehicule.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera                               
                    rot.x = RotationCameraPoursuiteAvancer.x;
                    rot.y = RotationCameraPoursuiteAvancer.y + _positionVehicule.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteAvancer.z;
                }
                else
                {
                    //Camera de poursuite Reculon

                    //Position de la camera
                    CamPos.x = _positionVehicule.position.x + (PositionCameraPoursuiteReculer.x * Mathf.Sin(_positionVehicule.eulerAngles.y * Mathf.PI / 180));
                    CamPos.y = PositionCameraPoursuiteReculer.y + _positionVehicule.position.y;
                    CamPos.z = _positionVehicule.position.z + (PositionCameraPoursuiteReculer.z * Mathf.Cos(_positionVehicule.eulerAngles.y * Mathf.PI / 180));

                    //Rotation de la camera           
                    rot.x = RotationCameraPoursuiteReculer.x;
                    rot.y = RotationCameraPoursuiteReculer.y + _positionVehicule.localEulerAngles.y;
                    rot.z = RotationCameraPoursuiteReculer.z;
                }
            }
            else if (CamTop == true)
            {
                //Camera avec une vue du haut.

                //Position de la camera
                CamPos.x = _positionVehicule.position.x + PositionCameraTop.x;
                CamPos.y = _positionVehicule.position.y + PositionCameraTop.y;
                CamPos.z = _positionVehicule.position.z + PositionCameraTop.z;

                //Rotation de la camera           
                rot.x = RotationCameraTop.x;
                rot.y = RotationCameraTop.y + _positionVehicule.localEulerAngles.y;
                rot.z = RotationCameraTop.z;
            }


            //Attribue les valeurs défini à la position et la rotation.
            CameraMultiple.transform.position = Vector3.Lerp(CameraMultiple.transform.position, CamPos, VitesseTransition * Time.deltaTime);
            CameraMultiple.transform.rotation = Quaternion.Euler(rot);
        }
        else if (CameraMultiple.enabled == false)
        {
            if (Input.GetKey(KeyCode.V))
            {
                CameraConducteurAvant.enabled = false;
                CameraConducteurArriere.enabled = true;
            }
            else if (vitesse > VitesseChangementCamera)
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

}
