using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouvementImage : MonoBehaviour
{
    public Sprite spritePiste1;
    public Sprite spritePiste2;
    public Sprite spritePiste3;
    public Image imagePiste1;
    public Image imagePiste2;
    public Image imagePiste3;

    void Update()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>().sprite == spritePiste1)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = spritePiste2;
        }
        if (this.gameObject.GetComponent<SpriteRenderer>().sprite == spritePiste2)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = spritePiste3;
        }
        if (this.gameObject.GetComponent<SpriteRenderer>().sprite == spritePiste3)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = spritePiste1;
        }
    }
}
