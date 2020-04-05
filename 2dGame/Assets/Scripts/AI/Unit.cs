using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    Vector3[] path;
    int targetIndex;

    Vector3 oldPosition;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    void Start()
    {
        PathRequestManager.RequestPath(transform, target, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath)
    {
        if (newPath.Length>0)
        {
            path = newPath;
            FollowPath();
        }
    }

    void FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        if((Vector3.Distance(transform.position, currentWaypoint) - Vector3.Distance(oldPosition, currentWaypoint)) > 0.1f)
        {
            if (targetIndex+1 >= path.Length)
            {
                return;
            }
            else
            {
                if ((Vector3.Distance(oldPosition, path[targetIndex+1]) - Vector3.Distance(transform.position, path[targetIndex+1]))>0.1f)
                {
                    targetIndex++;
                    currentWaypoint = path[targetIndex];
                }
            }
        }
        else
        {
            oldPosition = transform.position;
        }

        if(Math.Abs(currentWaypoint.x - transform.position.x) > 0.1f)
        {
            character.Move(Math.Sign(currentWaypoint.x - transform.position.x));
        }

        bool neadJump = (currentWaypoint.y - transform.position.y)>0.45;

        if (neadJump) {
            character.Jump();
        }
       
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for(int i = targetIndex; i<path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one*0.1f);
            }
        }
    }

    void OnGUI()
    {
        if (Application.isEditor)  // or check the app debug flag
        {
            GUI.Label(new Rect(20, 20, 100, 20), $"Velocity.x: {GetComponent<Rigidbody2D>().velocity.x.ToString()}");
        }

        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                GUI.Label(new Rect(20, 20*(i+1), 100, 20), $"path-{i} x{path[i].x}, y{path[i].x}");
            }
        }
    }

}
