using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    List<PathRequest> pathRequestsList = new List<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Transform pathStart, Transform pathEnd, Action<Vector3[]> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.TryProcess(newRequest);
    }

    void TryProcess(PathRequest newRequest)
    {
        StartCoroutine(ProcessingPath(newRequest));
    }

    IEnumerator ProcessingPath(PathRequest request)
    {
        while (request.pathStart != null && request.pathEnd != null) {
            Vector3[] waypoints = pathfinding.StartFindPath(request.pathStart.position, request.pathEnd.position);
            request.callback(waypoints);
            yield return null;
        }
    }



    struct PathRequest
    {
        public Transform pathStart;
        public Transform pathEnd;
        public Action<Vector3[]> callback;

        public PathRequest(Transform _start, Transform _end, Action<Vector3[]> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}
