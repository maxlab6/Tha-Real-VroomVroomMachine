using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingF : MonoBehaviour
{
    public GridF grid;
    public Transform StartPosition;
    private Transform TargetPosition;


    private void Awake()
    {
        grid = GetComponent<GridF>();
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    public void FindPath(Vector3 sPos, Vector3 tPos)
    {
        NodeF StartNode = grid.NodeFromWorldPosition(sPos);
        NodeF TargetNode = grid.NodeFromWorldPosition(tPos);

        List<NodeF> OpenList = new List<NodeF>();
        HashSet<NodeF> ClosedList = new HashSet<NodeF>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            NodeF CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (NodeF NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.IsTrack || ClosedList.Contains(NeighborNode)) 
                {
                    continue;
                }

                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private int GetManhattenDistance(NodeF nodeA, NodeF nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

    private void GetFinalPath(NodeF sNode, NodeF eNode)
    {
        List<NodeF> FinalPath = new List<NodeF>();
        NodeF CurrentNode = eNode;

        while (CurrentNode != sNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();
        grid.FinalPath = FinalPath;
    }

    public List<NodeF> TrouverCheminCourt (Transform endTransform)
    {
        FindPath(StartPosition.position, endTransform.position);
        return grid.FinalPath;
    }
}
