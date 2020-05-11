using System.Collections;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class PathfindingE : MonoBehaviour
{

    PathRequesterE requester;
    GridE grid;


    void Awake()
    {
        requester = GetComponent<PathRequesterE>();
        grid = GetComponent<GridE>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        NodeE startNode = grid.NodeFromWorldPoint(startPos);
        NodeE targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.drivable && targetNode.drivable)
        {
            HeapE<NodeE> openSet = new HeapE<NodeE>(grid.MaxSize);
            HashSet<NodeE> closedSet = new HashSet<NodeE>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                NodeE currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    UnityEngine.Debug.Log("Path trouvé:" + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (NodeE neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.drivable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostTONeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostTONeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostTONeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requester.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(NodeE startNode, NodeE targetNode)
    {
        List<NodeE> path = new List<NodeE>();
        NodeE currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<NodeE> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(NodeE nodeA, NodeE nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + (10 * (distX - distY));
        return 14 * distX + (10 * (distY - distX));

    }
}


