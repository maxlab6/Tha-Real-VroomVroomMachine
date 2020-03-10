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

    public int compteur = 0;
    public static int valMax = 2;

    public void buttonG()
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
            imageC.overrideSprite = spriteF1;
            imageG.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteJ1;
        }
        if (compteur == 1)
        {
            imageC.overrideSprite = spriteJ2;
            imageG.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteF1;
        }
        if (compteur == 2)
        {
            imageC.overrideSprite = spriteJ1;
            imageG.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ2;
        }

    }

    public void buttonD()
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
            imageC.overrideSprite = spriteF1;
            imageG.overrideSprite = spriteJ2;
            imageD.overrideSprite = spriteJ1;
        }
        if (compteur == 1)
        {
            imageC.overrideSprite = spriteJ2;
            imageG.overrideSprite = spriteJ1;
            imageD.overrideSprite = spriteF1;
        }
        if (compteur == 2)
        {
            imageC.overrideSprite = spriteJ1;
            imageG.overrideSprite = spriteF1;
            imageD.overrideSprite = spriteJ2;
        }

    }

}
