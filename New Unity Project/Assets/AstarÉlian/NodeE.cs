using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe qui définit les sommets, dérivée d'un array de l'arbre organisationel. 
public class NodeE : IHeapItem<NodeE>
{
    public bool drivable;               //Détermine si le sommet est atteignable. 
    public Vector3 worldPosition;       //Position (x,y,z) dans le monde du sommet. 
    public int gridX;                   //Position X dans la grille du sommet. 
    public int gridY;                   //Position Y dans la grille du sommet. 
    public int movementPenalty;         //Cout de pénalité de mouvement du sommet. 

    public int gCost;                   //Cout g (Distance entre ancien sommet et sommet). 
    public int hCost;                   //Cout h (Distance entre sommet cible et sommet). 
    public NodeE parent;                //Le sommet parent. 
    int heapIndex;                      //L'endroit du sommet dans l'arbre. 

    //Fonction constructeur d'un sommet. 
    public NodeE(bool _drivable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        drivable = _drivable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    //Retourne le cout f (cout h + cout f). 
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Fonction qui retourne l'endroit du sommet dans l'arbre organisationel. 
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //Fonction qui compare le cout de deux sommets. 
    public int CompareTo(NodeE nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
