using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public Transform Terrain;
    public LayerMask walkableMask;
    public LayerMask start_Finish_Mask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Start ()
    {
        nodeDiameter = nodeRadius * 2;

        transform.position = new Vector3(transform.position.x,Terrain.position.y + nodeRadius + 0.01f, transform.position.z);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x<gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                int surface;
                
                


                if (Physics.CheckSphere(worldPoint, nodeRadius, start_Finish_Mask) == true)
                {   
                    //Detect start finish line
                    surface = 2;
                }
                else if(Physics.CheckSphere(worldPoint, nodeRadius, walkableMask) == true)
                {
                    //Detect Road
                    surface = 1;                          
                }                
                else
                {
                    //detect the rest
                    surface = 3;
                }
                   grid[x, y] = new Node(surface, worldPoint);  

                
                
                
            }
        }
    }

    
    public Node Node_From_World_Point(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }






    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid!=null)
        {
            Node playerNode = Node_From_World_Point(player.position);

            foreach(Node n in grid)
            {

                if(n.surface == 1)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
                else if(n.surface == 2)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
                /*
                if(playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                */
            
               
                
            }
        }


    }
    
}
