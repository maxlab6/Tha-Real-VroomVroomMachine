using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenunChoixPiste : MonoBehaviour
{
    public Image imageC;
    public Image imageG;
    public Image imageD;

    public Sprite spriteJ1;
    public Sprite spriteJ2;
    public Sprite spriteF1;
    public Sprite spriteE1;
    public Sprite spriteE2;

    public static int compteur = 0;
    public static int valMax = 4;

    public void buttonD()
    {
        if (getCompteur() == 0)
        {
            setCompteur(valMax);
        }
        else
        {
            setCompteur(getCompteur()-1);
        }

        print(getCompteur().ToString());

        setImage(getCompteur());

    }

    public void buttonG()
    {
        if (getCompteur() == valMax)
        {
            setCompteur(0);
        }
        else
        {
            setCompteur(getCompteur()+1);
        }

        print(getCompteur().ToString());

        setImage(getCompteur());

    }

    public void setCompteur(int c)
    {
        //GameObject.Find("GameObjectCompteur").GetComponent<scriptCompteur>().setCompteur(c);
        compteur = c;
    }
    public int getCompteur()
    {
        //return GameObject.Find("GameObjectCompteur").GetComponent<scriptCompteur>().getCompteur();
        return compteur;
    }

    public void setImage(int c)
    {
        switch (c)
        {
            case 0:
            imageG.overrideSprite = spriteJ2;
            imageC.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ1;
                break;
            case 1:
            imageG.overrideSprite = spriteE1;
            imageC.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteF1;
                break;
            case 2:
            imageG.overrideSprite = spriteE2;
            imageC.overrideSprite = spriteE1;
            imageD.overrideSprite = spriteJ2;
                break;
            case 3:
            imageG.overrideSprite = spriteJ1;
            imageC.overrideSprite = spriteE2;
            imageD.overrideSprite = spriteE1;
                break;
            case 4:
            imageG.overrideSprite = spriteF1;
            imageC.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteE2;
                break;
            default:
                break;
        }

        
    }

    public void buttonChoisir()
    {
        int c = getCompteur();
        switch (c)
        {
            case 0:
                SceneManager.LoadScene("PisteFelix", LoadSceneMode.Single);
                break;

            case 1:
                SceneManager.LoadScene("PisteJoV2", LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene("PisteÉlian1", LoadSceneMode.Single);
                break;
            case 3:
                SceneManager.LoadScene("PisteÉlian2", LoadSceneMode.Single);
                break;
            case 4:
                SceneManager.LoadScene("MapJo", LoadSceneMode.Single);
                break;
        }
    }
}
