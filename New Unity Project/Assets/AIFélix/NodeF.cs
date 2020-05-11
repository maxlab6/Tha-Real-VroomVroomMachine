using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeF
{
    public int gridX;
    public int gridY;

    public bool IsWall;
    public bool IsTrack;
    public Vector3 Position;

    public NodeF Parent;

    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public NodeF (bool _isWall, bool _isTrack, Vector3 _Position, int _gridX, int _gridY)
    {
        IsWall = _isWall;
        IsTrack = _isTrack;
        Position = _Position;
        gridX = _gridX;
        gridY = _gridY;
    }
}
