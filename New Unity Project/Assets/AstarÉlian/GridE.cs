using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridE : MonoBehaviour
{
    public bool drawGrid;
    public LayerMask undrivableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] drivableRegions;
    LayerMask drivableMask;
    Dictionary<int, int> drivableRegionsDictionnary = new Dictionary<int, int>();

    NodeE[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in drivableRegions)
        {
            drivableMask.value = drivableMask |= region.terrainMask.value;
            drivableRegionsDictionnary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        } 

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        Debug.Log(gridSizeX);
        Debug.Log(gridSizeY);
        grid = new NodeE[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;


        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool drivable = !(Physics.CheckSphere(worldPoint, nodeRadius,undrivableMask));

                int movementPenalty = 0;

                if (drivable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if(Physics.Raycast(ray,out hit, 100, drivableMask))
                    {
                        drivableRegionsDictionnary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[i, j] = new NodeE(drivable, worldPoint, i, j, movementPenalty);
            }
        }
    }

    public List<NodeE> GetNeighbours(NodeE node)
    {
        List<NodeE> neighbours = new List<NodeE>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }


    public NodeE NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + (gridWorldSize.x / 2)) / gridWorldSize.x;
        float percentY = (worldPosition.z + (gridWorldSize.y / 2)) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }


    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        Gizmos.color = Color.red;


            if (grid != null && drawGrid == true)
            {
                foreach (NodeE n in grid)
                {
                    Gizmos.color = (n.drivable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        
        }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

}
