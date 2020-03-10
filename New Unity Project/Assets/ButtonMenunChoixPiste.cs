using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int compteur = 0;
    public static int valMax = 4;

    public void buttonD()
    {
        if (compteur == 0)
        {
            compteur = valMax;
        }
        else
        {
            compteur -= 1;
        }

        if (compteur == 0)
        {
            imageG.overrideSprite = spriteJ2;
            imageC.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ1;
        }
        if (compteur == 1)
        {
            imageG.overrideSprite = spriteE1;
            imageC.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteF1;
        }
        if (compteur == 2)
        {
            imageG.overrideSprite = spriteE2;
            imageC.overrideSprite = spriteE1;
            imageD.overrideSprite = spriteJ2;
        }
        if (compteur == 3)
        {
            imageG.overrideSprite = spriteJ1;
            imageC.overrideSprite = spriteE2;
            imageD.overrideSprite = spriteE1;
        }
        if (compteur == 4)
        {
            imageG.overrideSprite = spriteF1;
            imageC.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteE2;
        }

    }

    public void buttonG()
    {
        if (compteur == valMax)
        {
            compteur = 0;
        }
        else
        {
            compteur += 1;
        }

        if (compteur == 0)
        {
            imageG.overrideSprite = spriteJ2;
            imageC.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ1;
        }
        if (compteur == 1)
        {
            imageG.overrideSprite = spriteE1;
            imageC.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteF1;
        }
        if (compteur == 2)
        {
            imageG.overrideSprite = spriteE2;
            imageC.overrideSprite = spriteE1;
            imageD.overrideSprite = spriteJ2;
        }
        if (compteur == 3)
        {
            imageG.overrideSprite = spriteJ1;
            imageC.overrideSprite = spriteE2;
            imageD.overrideSprite = spriteE1;
        }
        if (compteur == 4)
        {
            imageG.overrideSprite = spriteF1;
            imageC.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteE2;
        }

    }

}
