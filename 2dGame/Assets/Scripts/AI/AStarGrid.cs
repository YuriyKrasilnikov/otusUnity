using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{

    public bool onlyDisplayPathGizmos;

    //public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float noteRadius;
    public Vector2 overlapBoxSize;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = noteRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        StartCoroutine(CreateGrid());
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    IEnumerator CreateGrid()
    {
        while (true)
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 wordBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = wordBottomLeft + Vector3.right * (x * nodeDiameter + noteRadius) + Vector3.up * (y * nodeDiameter + noteRadius);

                    // !!! сделать функцию, которая возвращает булевое значение в Node
                    bool walkable = !(Physics2D.OverlapBox(worldPoint, overlapBoxSize, 90, unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
            yield return null;
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX>=0 && checkX < gridSizeX && checkY >=0 && checkY< gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {

        //Debug.Log($"worldPosition {worldPosition}");

        worldPosition = worldPosition - transform.position;

        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2 ) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX + noteRadius) * percentX);
        int y = Mathf.RoundToInt((gridSizeY + noteRadius) * percentY);

        //Debug.Log($"x {x}, y {y}");

        return grid[x, y];
    }


    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        
        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (0.9f * nodeDiameter));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;

                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (0.9f * nodeDiameter));
                }
            }
        }
    }
}
