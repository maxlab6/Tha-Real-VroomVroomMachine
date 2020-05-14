using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//S'occupe des demande de pathfinding
public class PathRequestManager_Max : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();     //File d'attente pour les demande pour trouver le chemin
    PathRequest currentPathRequest;                                     //demande pour trouver le chemin du moment

    static PathRequestManager_Max instance;                             //instance de pathrequestManager
    Pathfinding_Max pathfinding;                                        //permet d'accéder au fonction de pathfinding

    bool isProcessingPath;                                              //permet de savoir s'il est en train de trouver un chemin

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding_Max>();
    }

    //Demande de trouver un chemin entre 2 points (vector3) et l'action est le tableau du chemin 
    //et une bool pour savoir si le chemin est possibles
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    //Permet d'essayer de trouver le chemin le plus court
    void TryProcessNext()
    {
        //s'il n'est pas entrain de process un chemin et que la queue (file d'attente)
        //est plus grande que 0 (n'est pas vide)
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    //est appelé par Pathfinding_max lorsque le chemin à été trouvé et
    //si le chemin est possible renvoie le chemin et essais de trouver le prochain
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    //Struct contenant toute les information necessaire pour faire une demande pour trouver le chemin
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}
