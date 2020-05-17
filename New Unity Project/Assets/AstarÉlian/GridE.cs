using System.Collections.Generic; 
using UnityEngine; 
 
//Classe de la grille de sommets. 
public class GridE : MonoBehaviour 
{ 
    public bool drawGrid;                                                           //Variable qui décide si on affiche la grille dans l'éditeur Unity ou pas. 
    public LayerMask undrivableMask;                                                //Layer qui définit où est-ce que l'auto ne peut pas conduire (ex: murs invisibles). 
    public Vector2 gridWorldSize;                                                   //Grandeur de la grille. 
    public float nodeRadius;                                                        //Grandeur du rayon de chaque sommet. 
    public TerrainType[] drivableRegions;                                           //Permet de mettre plusieurs Layer qui représente où la voiture peut conduire. 
    LayerMask drivableMask;                                                         //Layer qui définit où est-ce que l'auto peut conduire (ex: route). 
    Dictionary<int, int> drivableRegionsDictionnary = new Dictionary<int, int>();   //Liste qui contient la définition de chaque Layer sur lequel l'auto peut conduire. 
    NodeE[,] grid;                                                                  //Array de sommets qui représente la grille en tant que telle. 
 
    float nodeDiameter;                                                             //Grandeur du diamètre de chaque sommet (rayon*2). 
    int gridSizeX, gridSizeY;                                                       //Nombre de sommets X. //Nombre de sommets Y. 
 
    //Fonction qui s'exécute est tout premier. 
    void Awake() 
    { 
        //Initialise le diamètre et assigne le nombre de sommets en X et Y besoin afin de faire la grille. 
        nodeDiameter = nodeRadius * 2; 
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); 
 
        //Assigne la pénalité préalablement choisit pour chaque sommets sur lequel l'auto peut conduire. 
        foreach (TerrainType region in drivableRegions) 
        { 
            drivableMask.value = drivableMask |= region.terrainMask.value; 
            drivableRegionsDictionnary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty); 
        } 
 
        //Appel la fonction pour créer la grille. 
        CreateGrid(); 
    } 
 
    //Retourne la grandeur maximal 
    public int MaxSize 
    { 
        get 
        { 
            return gridSizeX * gridSizeY; 
        } 
    } 
 
    //Fonction qui créer la grille de sommets 
    void CreateGrid() 
    { 
        grid = new NodeE[gridSizeX, gridSizeY]; 
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; 
 
 
        //Boucle afin d'initialiser chaque sommet dans la grille avec leur bonne position et pénalité. 
        for (int i = 0; i < gridSizeX; i++) 
        { 
            for (int j = 0; j < gridSizeY; j++) 
            { 
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius); 
                bool drivable = !(Physics.CheckSphere(worldPoint, nodeRadius, undrivableMask)); 
 
                int movementPenalty = 0; 
 
                //Applique la pénalité de movement selon le layer si applicable. 
                if (drivable) 
                { 
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down); 
                    RaycastHit hit; 
                    if (Physics.Raycast(ray, out hit, 100, drivableMask)) 
                    { 
                        drivableRegionsDictionnary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty); 
                    } 
                } 
 
                grid[i, j] = new NodeE(drivable, worldPoint, i, j, movementPenalty); 
            } 
        } 
    } 
 
    //Fonction qui retourne les sommets adjacents d'un sommets. 
    public List<NodeE> GetNeighbours(NodeE node) 
    { 
        List<NodeE> neighbours = new List<NodeE>(); 
 
        //Boucle qui prend une grille de sommet 3x3 avec le sommet donné au centre. 
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
 
    //Retourne le sommet correspondant à une position (x,y,z) dans le monde. 
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
 
    //Dessine dans l'éditeur Unity. 
    void OnDrawGizmos() 
    { 
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); 
        Gizmos.color = Color.red; 
 
        //Si la grille n'est pas vide et que l'option pour afficher la grille est activer... 
        if (grid != null && drawGrid == true) 
        { 
            //Affiche les sommets atteignable en blanc et les inatteignable en rouge. 
            foreach (NodeE n in grid) 
            { 
                Gizmos.color = (n.drivable) ? Color.white : Color.red; 
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f)); 
            } 
        } 
 
    } 
 
    //Classe correspondant au type de terrain. 
    [System.Serializable] 
    public class TerrainType 
    { 
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

}
