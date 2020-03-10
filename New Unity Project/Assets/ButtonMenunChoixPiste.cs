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

    public static int valMax = 4;

    public void buttonG()
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

    public void buttonD()
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
        GameObject.Find("GameObjectCompteur").GetComponent<scriptCompteur>().setCompteur(c);
    }
    public int getCompteur()
    {
        return GameObject.Find("GameObjectCompteur").GetComponent<scriptCompteur>().getCompteur();
    }

    public void setImage(int c)
    {
        if (c == 0)
        {
            imageG.overrideSprite = spriteJ2;
            imageC.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ1;
        }
        if (c == 1)
        {
            imageG.overrideSprite = spriteE1;
            imageC.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteF1;
        }
        if (c == 2)
        {
            imageG.overrideSprite = spriteE2;
            imageC.overrideSprite = spriteE1;
            imageD.overrideSprite = spriteJ2;
        }
        if (c == 3)
        {
            imageG.overrideSprite = spriteJ1;
            imageC.overrideSprite = spriteE2;
            imageD.overrideSprite = spriteE1;
        }
        if (c == 4)
        {
            imageG.overrideSprite = spriteF1;
            imageC.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteE2;
        }

        
    }

    public void buttonChoisir()
    {
        if (getCompteur() == 0)
        {
            print("hold on");
            SceneManager.LoadScene("PisteFelix", LoadSceneMode.Additive);
            print("ca marche pas");
        }
    }
}
