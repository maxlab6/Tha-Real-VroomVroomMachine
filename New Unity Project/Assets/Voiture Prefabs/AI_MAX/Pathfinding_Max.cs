using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;


//c'est ici que le pathfinding se fait et que l'algorithme A* est utilisé
public class Pathfinding_Max : MonoBehaviour
{

    PathRequestManager_Max requestManager;
    Grille_Max grille;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager_Max>();
        grille = GetComponent<Grille_Max>();
    }

    
    public void StartFindPath(Vector3 positionDepart, Vector3 positionDArrive)
    {
        StartCoroutine(TrouverChemin(positionDepart, positionDArrive));
    }

    //Fonction qui permet de trouver le chemin jusqu'au point d'arriver
    IEnumerator TrouverChemin(Vector3 positionDepart, Vector3 positionDArrive)
    {
        Vector3[] waypoints = new Vector3[0]; 
        bool pathSuccess = false;             

        Sommet sommetDepart = grille.SommetdansMonde(positionDepart);
        Sommet sommetDArrive = grille.SommetdansMonde(positionDArrive);

        //Si le point de départ et le point d'arriver peu n'ont pas de mur
        //(est walkable) alors fait le code sinon ne fait pas le code et
        //pathSuccess reste false
        if (sommetDepart.peuPasser && sommetDArrive.peuPasser)
        {
            Heap_Max<Sommet> openSet = new Heap_Max<Sommet>(grille.MaxSize); //Contient toutes les case qui ne sont pas dans la closedList dans le heap (arbre binaire)
            HashSet<Sommet> closedSet = new HashSet<Sommet>();               //case ou les distances ont été calculé
            openSet.Ajouter(sommetDepart);

            //Tant qu'il reste de case à vérifier, continuer a vérifier les autres cases
            while (openSet.Count > 0)
            {
                Sommet sommet = openSet.RemoveFirst();
                closedSet.Add(sommet);

                //Si le sommetActuelle = au sommetDArrive, ça veux dire qu'il a atteint
                //l'objectif et n'a donc plus besoin de véçrifier les autres cases
                if (sommet == sommetDArrive)
                {
                    pathSuccess = true;
                    break;
                }

                //Pour tout les voisins du sommet analysé 
                foreach (Sommet voisin in grille.GetVoisins(sommet))
                {
                    //Si le sommet n'est pas walkable (peuPasPasser) ou qui a déjà été vérifier
                    //(est dans closedSet) alors skip cette itération et passe à la prochaine
                    if (!voisin.peuPasser || closedSet.Contains(voisin))
                    {
                        continue;
                    }

                    int newCostToNeighbour = sommet.gCost + GetDistance(sommet, voisin) + voisin.movementPenalite;

                    //si le chemin entre la case actuelle et le voisin analysé est plus court
                    //ou que le voisin n'est pas dans l'openSet (n'a pas déjà été analysé)
                    if (newCostToNeighbour < voisin.gCost || !openSet.Contains(voisin))
                    {
                        voisin.gCost = newCostToNeighbour;
                        voisin.hCost = GetDistance(voisin, sommetDArrive);
                        voisin.parent = sommet;

                        //si il n'était pas dans open set, le rajoute
                        //sinon fais juste changer les valeurs
                        if (!openSet.Contains(voisin))
                            openSet.Ajouter(voisin);
                        else
                        {
                            openSet.UpdateItems(voisin);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetrouverChemin(sommetDepart, sommetDArrive);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    //En gros part du point d'arrivé et ajoute son parent a une liste,répète jusqu'a
    //se qu'on ait atteint le point de départ et inverse le chemin s'occuper
    //aussi d'utiliser la simplification du chemin
    Vector3[] RetrouverChemin(Sommet sommetDepart, Sommet sommetDArrive)
    {
        List<Sommet> chemin = new List<Sommet>();
        Sommet sommetActuelle = sommetDArrive;

        while (sommetActuelle != sommetDepart)
        {
            chemin.Add(sommetActuelle);
            sommetActuelle = sommetActuelle.parent;
        }

        Vector3[] waypoints = SimplifierChemin(chemin);
        Array.Reverse(waypoints);
        return waypoints;

    }

    //Permet de simplifier le chemin en regardant si la direction est la même entre
    //deux sommet, si oui, ignore le point jusqu'a ce qu'il y ait un changement de direction
    Vector3[] SimplifierChemin(List<Sommet> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].grilleX - path[i].grilleX, path[i - 1].grilleY - path[i].grilleY);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i].worldPosition);
            }
            oldDirection = newDirection;
        }

        return waypoints.ToArray();
    }

    //Permet d'obtenir le distance dans la grille entre deux sommets
    int GetDistance(Sommet sommetA, Sommet sommetB)
    {
        int distanceX = Mathf.Abs(sommetA.grilleX - sommetB.grilleX);
        int distanceY = Mathf.Abs(sommetA.grilleY - sommetB.grilleY);

        //Si la distance En X est plus grande qu'en Y, alors le nombre
        //de mouvement en diagonal est le nombre de mouvement en Y
        //et les mouvements a l'horizontal son le restant des cases
        //a parcourir en x. Sinon, c'est l'inverse
        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
