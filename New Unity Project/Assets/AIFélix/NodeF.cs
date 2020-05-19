//Auteur : Félix Doyon
//Ce script permet de créer les nodes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeF
{
    public int gridX; //Position en x et en y en nombre de node
    public int gridY;

    public bool IsWall; //Si la node est un mur ou la piste
    public bool IsTrack;
    public Vector3 Position; //Position en x et en y 

    public NodeF Parent; //Node parent pour retrouver le chemin le plus court

    public int gCost; //THÉORIE : le nom des coût (f, g et h) peut changer selon les sources
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public NodeF (bool _isWall, bool _isTrack, Vector3 _Position, int _gridX, int _gridY) //Constructeur de NodeF
    {
        IsWall = _isWall;
        IsTrack = _isTrack;
        Position = _Position;
        gridX = _gridX;
        gridY = _gridY;
    }
}
