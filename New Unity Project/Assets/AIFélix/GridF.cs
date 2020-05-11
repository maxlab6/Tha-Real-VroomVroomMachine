using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridF : MonoBehaviour
{
    public Transform StartPosition;
    public LayerMask WallMask;
    public LayerMask TrackMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;
    public bool afficher;

    NodeF[,] grid;
    public List<NodeF> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public NodeF NodeFromWorldPosition(Vector3 _sPos)
    {
        float xPoint = ((_sPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((_sPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];


    }

    private void CreateGrid()
    {
        grid = new NodeF[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = false;
                bool Track = false;

                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = true;
                }
                if (Physics.CheckSphere(worldPoint, nodeRadius, TrackMask))
                {
                    Track = true;
                }

                grid[x, y] = new NodeF(Wall, Track, worldPoint, x, y);
            }
        }
    }

    public List<NodeF> GetNeighboringNodes(NodeF cNode)
    {
        List<NodeF> NeighboringNodes = new List<NodeF>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = cNode.gridX + x;
                int checkY = cNode.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    NeighboringNodes.Add(grid[checkX, checkY]);
                }

            }
        }
            return NeighboringNodes;
    }

    private void OnDrawGizmos()
    {
        if (afficher)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                foreach (NodeF n in grid)
                {
                    if (n.IsTrack)
                    {
                        Gizmos.color = Color.white;
                    }
                    else if (n.IsWall)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }

                    if (FinalPath != null)
                    {
                        if (FinalPath.Contains(n))
                        {
                            Gizmos.color = Color.red;
                        }
                    }

                    Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance));
                }
            }
        }
    }

    public List<NodeF> returnFinalPath()
    {
        return FinalPath;
    }
}
