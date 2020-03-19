using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptCompteur : MonoBehaviour
{
    public int compteur = 0;

    public int getCompteur()
    {
        return compteur;
    }

    public void setCompteur(int c)
    {
        compteur = c;
    }
}
