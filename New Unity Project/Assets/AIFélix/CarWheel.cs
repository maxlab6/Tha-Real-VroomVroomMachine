// Auteur : Félix Doyon
// Ce script sert à faire bouger les roue du poiont de vue visuel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public WheelCollider targetWheel; //Wheel Collider attaché à la roue
    private Vector3 wheelPosition = new Vector3(); //Position de la roue
    private Quaternion wheelRotation = new Quaternion(); //Angle de la roue

    void Update() //Prend les ionformation du wheel collider et les applique au wheel render
    {
        targetWheel.GetWorldPose(out wheelPosition, out wheelRotation);
        transform.position = wheelPosition;
        transform.rotation = wheelRotation;
    }
}
