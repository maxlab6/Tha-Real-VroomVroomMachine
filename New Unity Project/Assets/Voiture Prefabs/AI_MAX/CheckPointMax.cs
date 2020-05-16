using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMax : MonoBehaviour
{
    public static bool atteintCheckPoint = false;

    //Lorsque un checkPoint est toucher, change la valeur 
    //de atteintCheckPoint pour true qui est static alors tout
    //les scripts y ont accès. Utiliser dans VoitureAi.cs
    private void OnTriggerEnter(Collider other)
    {
         atteintCheckPoint = true;
    }
}
