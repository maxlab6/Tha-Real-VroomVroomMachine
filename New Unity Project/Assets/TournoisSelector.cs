using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TournoisSelector : MonoBehaviour, IPointerEnterHandler
{
    private GameObject imagePisteG;
    private GameObject imagePisteC;
    private GameObject imagePisteD;

    public Sprite spriteEli1;
    public Sprite spriteEli2;
    public Sprite spriteJo1;
    public Sprite spriteJo2;
    public Sprite spriteFel1;
    public Sprite spriteFel2;
    public Sprite random;

    public void Start()
    {
        imagePisteG = GameObject.Find("ImagePisteG");
        imagePisteC = GameObject.Find("ImagePisteC");
        imagePisteD = GameObject.Find("ImagePisteD");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
        if (gameObject.name == "Tournois1")
        {
            imagePisteG.GetComponent<Image>().overrideSprite = spriteEli1;
            imagePisteC.GetComponent<Image>().overrideSprite = spriteJo1;
            imagePisteD.GetComponent<Image>().overrideSprite = spriteFel1;
            Debug.Log("Done");
        }
       else if(gameObject.name == "Tournois2")
        {
            imagePisteG.GetComponent<Image>().overrideSprite = spriteEli2;
            imagePisteC.GetComponent<Image>().overrideSprite = spriteJo2;
            imagePisteD.GetComponent<Image>().overrideSprite = spriteFel2;
        }
        else if(gameObject.name == "Aléatoire")
        {
            imagePisteG.GetComponent<Image>().overrideSprite = random;
            imagePisteC.GetComponent<Image>().overrideSprite = random;
            imagePisteD.GetComponent<Image>().overrideSprite = random;
        }
    }
    
        
    
}
