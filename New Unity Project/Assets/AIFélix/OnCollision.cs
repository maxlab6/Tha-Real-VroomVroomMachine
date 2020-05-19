//Auteur : Félix Doyon
//Ce script sert à savoir lorsque l'IA rentre en collision avec un check point pour incrémenter le compteur dans ControleVoitureIAFelix

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public bool isColliding = false; //Si l'auto est dans le check point

    private void OnTriggerEnter(Collider other) //Lorsque l'auto entre dans le check point
    {
        if (other.tag == "car") //Compare le tag de l'élément qui rentre dans le check point    ERREUR : le projet est déja compiler à l'heure qu'il est mais il serait sencé que ce soit le tag "IAFelix" et non "car"
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other) //Compare le tag de l'élément qui sort pour savoir s'il s'agit belle et bien de l'IA
    {
        if (other.tag == "car")
        {
            isColliding = false;
        }
    }

    public bool getIsColliding() //Retourne la valeur dans le script ControleVoitureIAFelix
    {
        return isColliding;
    }
}
