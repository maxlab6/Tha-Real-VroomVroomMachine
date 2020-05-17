using System.Collections;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

//Classe qui contient tout les éléments afin de trouver le chemin le plus court. 
public class PathfindingE : MonoBehaviour
{

    PathRequesterE requester;               //Objet du gestionnaire de requete. 
    GridE grid;                             //Objet de la grille de sommets. 

    //Fonction qui s'exécute est tout premier. 
    void Awake()
    {
        //Trouve la grille de sommets et le gestionnaire de requete dans l'objet. 
        requester = GetComponent<PathRequesterE>();
        grid = GetComponent<GridE>();
    }

    //Fonction du d'initialisation pour trouvé le chemin le plus court entre deux point dans le monde (x,y,z). 
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    //Coroutine qui trouve le chemin le plus court. 
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Permet de débogue le temps pour trouver le chemin. 
        Stopwatch sw = new Stopwatch();
        sw.Start();

        //Array de position des sommets dans le chemin le plus court. 
        Vector3[] waypoints = new Vector3[0];

        bool pathSuccess = false;

        //Les sommets de départ et de fin sont trouvés avec la fonction NodeFromWorldPoint dans la classe de la grille. 
        NodeE startNode = grid.NodeFromWorldPoint(startPos);
        NodeE targetNode = grid.NodeFromWorldPoint(targetPos);

        //Commence à trouvé le chemin le plus court si les deux sommets sont atteignables. 
        if (startNode.drivable && targetNode.drivable)
        {
            //Array de sommets pas encore explorés 
            HeapE<NodeE> openSet = new HeapE<NodeE>(grid.MaxSize);

            //Array de sommets explorés 
            HashSet<NodeE> closedSet = new HashSet<NodeE>();
            openSet.Add(startNode);

            //Boucle qui cherche le chemin le plus court. 
            while (openSet.Count > 0)
            {
                NodeE currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                //Si le chemin est trouvé 
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    UnityEngine.Debug.Log("Élian AI: Chemin trouvé:" + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                //Calcule le cout des sommets voisins 
                foreach (NodeE neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.drivable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostTONeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostTONeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        //Calcule le cout de chaque sommets voisins. 
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

        //Une fois que le chemin est trouvé ça retrouve le bon chemin. 
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requester.FinishedProcessingPath(waypoints, pathSuccess);
    }

    //Fonction qui permet de retrouvé le chemin trouvé par A* en faisant le chemin inverse de l'algorithme. 
    Vector3[] RetracePath(NodeE startNode, NodeE targetNode)
    {
        List<NodeE> path = new List<NodeE>();
        NodeE currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //Simplifie le chemin le plus court... 
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    //Fonction qui simplifie le chemin le plus court en enlevant les sommets qui ne change pas la direction du chemin. 
    Vector3[] SimplifyPath(List<NodeE> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        //Pour chaque sommets trouvé dans le chemin le plus court. 
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

            //Compare si la direction change entre chaque sommet. 
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //Fonction qui trouve la distance entre deux sommets. 
    int GetDistance(NodeE nodeA, NodeE nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + (10 * (distX - distY));
        return 14 * distX + (10 * (distY - distX));

    }
}


