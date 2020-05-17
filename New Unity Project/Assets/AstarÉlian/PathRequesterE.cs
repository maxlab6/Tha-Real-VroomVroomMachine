using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Classe du gestionnaire de demande pour trouver le chemin le plus court entre deux sommets. 
public class PathRequesterE : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();     //Array de requete de chemin le plus court. 
    PathRequest currentPathRequest;                                     //La requete de chemin le plus court qui est présentement traitée. 

    static PathRequesterE instance;                                     //L'objet de la classe. 
    PathfindingE pathfinding;                                           //Créer un objet qui trouve le chemin le plus court. 

    bool isProcessingPath;                                              //Determine si l'objet est entrain de chercher le chemin le plus court. 

    //Fonction qui s'exécute est tout premier. 
    void Awake()
    {
        //Initialisation de l'instance 
        instance = this;

        //Trouve l'objet qui chercher le chemin le plus court dans le GameObject. 
        pathfinding = GetComponent<PathfindingE>();
    }

    //Fonction qui demande de trouvé le chemin le plus court. 
    public static void RequestPath(Vector3 pathStart, Vector3 pathTarget, Action<Vector3[], bool> callback)
    {
        //Créer une nouvelle demande 
        PathRequest newRequest = new PathRequest(pathStart, pathTarget, callback);

        //Met la demande dans la file (le array). 
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    //Fonction qui traite la prochaine demande dans la file (le array). 
    void TryProcessNext()
    {
        //Si aucune autre demande est entrain d'être traitée... 
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            //Traite la demande, alors il demande à "pathfinding" de trouvé le chemin le plus court. 
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathTarget);
        }
    }

    //Fonction quand la demande est fini d'être traité. 
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        //Retourne le résultat de la demande. 
        currentPathRequest.callback(path, success);
        isProcessingPath = false;

        //Commence la prochaine demande dans la file (le array). 
        TryProcessNext();
    }

    //Structure qui définie qu'est ce qu'une requete de chemin le plus court. 
    struct PathRequest
    {
        public Vector3 pathStart;                   //La position initiale d'un chemin. 
        public Vector3 pathTarget;                  //La position cible d'un chemin. 
        public Action<Vector3[], bool> callback;    //Action qui permet de retourner le chemin le plus courtt et de savoir si le chemin existe. 

        //Fonction de demande de chemin le plus court. 
        public PathRequest(Vector3 _start, Vector3 _target, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathTarget = _target;
            callback = _callback;
        }
    }
}
