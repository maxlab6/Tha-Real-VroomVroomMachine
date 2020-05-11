using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compteur : MonoBehaviour
{
    public static int compteur = 0;

    private void OnCollisionEnter(Collision collision)
    {
        compteur++;
    }
}
