using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouvementImage : MonoBehaviour
{
    /*public Sprite spritePisteC;
    public Sprite spritePisteG;
    public Sprite spritePisteD;
    public GameObject gOPisteC;
    public GameObject gOPisteG;
    public GameObject gOPisteD;*/
    public List<GameObject> listGO = new List<GameObject>;

    public List listComparaison = new ArrayList();

    void start()
    {
        listSprite.Add(".../Assets/Menu/MenuChoixPiste.Annotation 2020-02-24 101500.png");
    }

    void Update()
    {
        gOPisteC = GameObject.FindGameObjectWithTag("Image");
        spritePisteC = gOPisteC.GetComponent<Sprite>();
        //spritePisteC.texture.EncodeToPNG().Equals(listSprite.)

        gOPisteG = GameObject.FindGameObjectWithTag("Image1");
        spritePisteG = gOPisteG.GetComponent<Sprite>();

        gOPisteD = GameObject.FindGameObjectWithTag("Image2");
        spritePisteD = gOPisteD.GetComponent<Sprite>();

        gOPisteC.

    }
}
