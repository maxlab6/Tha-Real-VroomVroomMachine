//Auteur : Félix Doyon
//Ce script contient l'algorithme A* et sert à calculer le chemin le plus court

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingF : MonoBehaviour
{
    public GridF grid; //La grille utilisé pour l'algorithme
    public Transform StartPosition; //La position de départ
    private Transform TargetPosition; //La position d'arrivé


    private void Awake()
    {
        grid = GetComponent<GridF>();
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }

    public void FindPath(Vector3 sPos, Vector3 tPos) //Cette fonction sert à calculé le chemin le plus court avec l'algorithme A* 
    {
        NodeF StartNode = grid.NodeFromWorldPosition(sPos); //La node de départ et d'arrivé calculé avec la fonction dans le script grid
        NodeF TargetNode = grid.NodeFromWorldPosition(tPos);

        List<NodeF> OpenList = new List<NodeF>(); //La liste des nodes non calculé
        HashSet<NodeF> ClosedList = new HashSet<NodeF>(); //La liste des nodes calculé

        OpenList.Add(StartNode); //Ajoute la node  de départ dans la liste des node non calculé

        while (OpenList.Count > 0) //Tant qu'il reste des nodes à calculer
        {
            NodeF CurrentNode = OpenList[0]; //Prend la prmière node de la liste pour la calculer
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i]; //Désigne la node avec le cout F (coût G et H) comme la node actuel pour la calculer
                }
            }

            OpenList.Remove(CurrentNode); //Met la node actuel dans la liste fermé puisqu'elle va être calculé
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode) //Si la node actuel est la node d'arrivé, l'algorithme va être terminé et va donc procéder à la recherche du chemin finale
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (NodeF NeighborNode in grid.GetNeighboringNodes(CurrentNode)) //Calcule tout les nodes adjacentes à la node actuel
            {
                if (!NeighborNode.IsTrack || ClosedList.Contains(NeighborNode)) //Ignore la node adjacente si elle est un mur
                {
                    continue;
                }

                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode); //Calcule le cout pour se rendre à la node adjacente

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode; //Désigne la node parent de la node adjacente comme étant la node actuel pour retrouver le chemin finale

                    if (!OpenList.Contains(NeighborNode)) //Si la node adjacente n'est pas dans la liste ouverte, elle va être ajouté
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private int GetManhattenDistance(NodeF nodeA, NodeF nodeB) //Calcule le coût de déplacement pour se rendre entre deux node
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

    private void GetFinalPath(NodeF sNode, NodeF eNode) //Calcule le chemin finale à partir des nodes parents
    {
        List<NodeF> FinalPath = new List<NodeF>();
        NodeF CurrentNode = eNode; //Commence par la dernière node

        while (CurrentNode != sNode) //Tant que la node actuel n'est pas la node de départ
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent; //Désigne la node parent comme étant la node actuel
        }

        FinalPath.Reverse(); //inverse le chemin pour qu'il commence par la node de départ et non la node d'arrivé
        grid.FinalPath = FinalPath;
    }

    public List<NodeF> TrouverCheminCourt (Transform endTransform) //Retourne le chemin finale (utilisé dans le script ControleVoitureIAFelix)
    {
        FindPath(StartPosition.position, endTransform.position);
        return grid.FinalPath;
    }
}
