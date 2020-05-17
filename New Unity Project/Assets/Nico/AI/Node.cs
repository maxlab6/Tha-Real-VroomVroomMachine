using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int surface;
    public Vector3 worldPosition;


    public Node (int _surface, Vector3 _worldPos)
    {
        surface = _surface;
        worldPosition = _worldPos;
    }
}
