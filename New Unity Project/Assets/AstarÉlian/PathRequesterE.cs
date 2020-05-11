using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequesterE : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequesterE instance;
    PathfindingE pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathfindingE>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathTarget, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathTarget, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathTarget);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathTarget;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _target, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathTarget = _target;
            callback = _callback;
        }
    }
}
