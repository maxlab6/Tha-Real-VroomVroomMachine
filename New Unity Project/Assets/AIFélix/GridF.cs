//Auteur : Félix Doyon
//Ce script sert à créer la grille et contient les fonctions relatives aux nodes

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridF : MonoBehaviour
{
    public Transform StartPosition; //Position de départ (position de l'auto
    public LayerMask WallMask; //Layer des murs
    public LayerMask TrackMask; //Layer de la route
    public Vector2 gridWorldSize; //Grosseur de la grille
    public float nodeRadius; //Rayon d'une node
    public float Distance; //Distance entre les node (0 dans le jeu)
    public bool afficher; //Si on affiche ou non la grille

    NodeF[,] grid; //Liste des nodes
    public List<NodeF> FinalPath; //Liste des nodes du chemin finale

    float nodeDiameter; //Diamètre d'une node
    int gridSizeX, gridSizeY; //Nombre de node de la grille en x et en y

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); //Trouver le nombre de node en x et en y
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public NodeF NodeFromWorldPosition(Vector3 _sPos) //Trouve la node à une position
    {
        float xPoint = ((_sPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((_sPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];


    }

    private void CreateGrid() //Cette fonction créé la grille
    {
        grid = new NodeF[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; //Trouve la position en bas à gauche de la grille

        for (int y = 0; y < gridSizeY; y++) //Les deux for créé une node à chaque carré
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); //Trouve la position que la node doit avoir
                bool Wall = false;
                bool Track = false;

                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask)) //Regarde si la node à le layer mur ou piste
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

    public List<NodeF> GetNeighboringNodes(NodeF cNode) //Retourne les nodes adjacentes à la node envoyé en argument
    {
        List<NodeF> NeighboringNodes = new List<NodeF>(); //Liste des nodes adjacentes

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) //Si la node est à 0 en x et en y, c'est qu'il s'agit d'elle même
                {
                    continue;
                }

                int checkX = cNode.gridX + x; //Les deux variables sont pour vérifier qu'il s'agit bien d'une node qui n'est pas à l'extérieur de la grille
                int checkY = cNode.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    NeighboringNodes.Add(grid[checkX, checkY]); //Ajoute la node en question dans la liste des nodes adjacentes
                }

            }
        }
            return NeighboringNodes;
    }

    private void OnDrawGizmos() //Fonction pour afficher le chemin court visuellement
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

    public List<NodeF> returnFinalPath() //Retourne le chemin court (utilisé pour le script PathFindingF)
    {
        return FinalPath;
    }
}
