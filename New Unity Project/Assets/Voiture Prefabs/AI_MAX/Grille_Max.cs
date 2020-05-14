using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grille_Max : MonoBehaviour
{
    public bool DisplayGridGizmos;           //Permet l'affichage de la grille en mode developpeur
    public LayerMask peuPasPasserMask;       //Layer peuPasPasser
    public Vector2 grosseurGrille;           //grosseur de la grille
    public float longueurSommet;             //longeur d'un sommet
    public TerrainType[] walkableRegion;     //Type de terrain apportant des pénalité de mouvement
    LayerMask peuPasserMask;                 //Layer peuPasser
    Dictionary<int, int> walkableregionDictionay = new Dictionary<int, int>();  //

    Sommet[,] grille;                        //grille de sommets (cases)

    float doubleLongueurSommet;              //double la longeur
    int grosseurGrilleX, grosseurGrilleY;    //grosseur de la grille en X et en Y

    //Ajustement selon la scene              
    int ajustementX = 0;                     //Ajustement de la grille en X selon la carte
    int ajustementY = 0;                     //Ajustement de la grille en Y selon la carte
    public string nomScene = "";             //Nom de la scene active

    void Awake()
    {

        //Prend le nom de la scene Active pour permettre les petit ajustement selon la map
        nomScene = SceneManager.GetActiveScene().name;

        if (nomScene == "PisteÉlian1") { ajustementX = -2; ajustementY = 1; }
        else if (nomScene == "PisteÉlian2") { ajustementX = 2; ajustementY = 2; }
        else if (nomScene == "PisteFelix") { ajustementX = 0; ajustementY = 0; }

        //permet d'avoir le nombre de case selon la grosseur de la grille et la grosseur d'une case
        doubleLongueurSommet = longueurSommet * 2;
        grosseurGrilleX = Mathf.RoundToInt(grosseurGrille.x / doubleLongueurSommet);
        grosseurGrilleY = Mathf.RoundToInt(grosseurGrille.y / doubleLongueurSommet);

        foreach (TerrainType region in walkableRegion)
        {
            peuPasserMask.value |= region.terrainMask.value;
            walkableregionDictionay.Add((int)Mathf.Log(region.terrainMask.value, 2.0f), region.terrainPenality);
        }

        CreerGrille();
    }

    //Grosseur de la grille
    public int MaxSize
    {
        get
        {
            return grosseurGrilleX * grosseurGrilleY;
        }
    }

    //Créer la grille en partant en bas a gauche de la grille créer, la case 0,0 est la premier case en bas a gauche
    void CreerGrille()
    {
        grille = new Sommet[grosseurGrilleX, grosseurGrilleY];
        Vector3 positionBasGaucheGrille = transform.position - Vector3.right * grosseurGrille.x / 2 - Vector3.forward * grosseurGrille.y / 2;

        for (int x = 0; x < grosseurGrilleX; x++)
        {
            for (int y = 0; y < grosseurGrilleY; y++)
            {
                //Vérifie si la case a le layer peuPasPasser
                Vector3 positionMonde = positionBasGaucheGrille + Vector3.right * (x * doubleLongueurSommet + longueurSommet) + Vector3.forward * (y * doubleLongueurSommet + longueurSommet);
                bool acessible = !(Physics.CheckSphere(positionMonde, longueurSommet, peuPasPasserMask));
                int movementPenality = 0;

                //s'il y a le layer peuPasPasser, alors 
                if (acessible)
                {
                    Ray ray = new Ray(positionMonde + Vector3.up * 50, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit, 100, peuPasserMask))
                    {
                        walkableregionDictionay.TryGetValue(hit.collider.gameObject.layer, out movementPenality);
                    }
                }

                grille[x, y] = new Sommet(acessible, positionMonde, x, y, movementPenality);
            }
        }
    }

    //Permet d'avoir toute les cases adjacente a la case envoyé
    public List<Sommet> GetVoisins(Sommet node)
    {
        List<Sommet> voisins = new List<Sommet>();

        // x  x  x
        // x  o  x  le o est la case actuelle et les x les voisins
        // x  x  x

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) //skip l'itération si x = 0 et y = 0 car c'est le sommet ( le o )
                    continue;

                int verifieX = node.grilleX + x;
                int verifieY = node.grilleY + y;

                //Permet de vérifie si la case est bien dans la grille et non en dehors, si la case est dans les  limites
                //alors elle est ajouté a la lsite des voisins
                if (verifieX >= 0 && verifieX < grosseurGrilleX && verifieY >= 0 && verifieY < grosseurGrilleY)
                {
                    voisins.Add(grille[verifieX, verifieY]);
                }
            }
        }
        return voisins;
    }

    //Permet d'avoir la position du target en convertissant la position monde en un position dans la grille.
    public Sommet SommetdansMonde(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + grosseurGrille.x / 2) / grosseurGrille.x;
        float percentY = (worldPosition.z + grosseurGrille.y / 2) / grosseurGrille.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((grosseurGrilleX - 1) * percentX);
        int y = Mathf.RoundToInt((grosseurGrilleY - 1) * percentY);
        return grille[x + ajustementX, y + ajustementY];
    }



    //Permet d'afficher la grille en mode developpeur
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(grosseurGrille.x, 1, grosseurGrille.y));


        if (grille != null && DisplayGridGizmos)
        {
            foreach (Sommet n in grille)
            {
                if (n.grilleX == 1 && n.grilleY == 1)
                {
                    Gizmos.color = Color.blue;
                }
                Gizmos.color = (n.peuPasser) ? Color.white : Color.red;

                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (doubleLongueurSommet - 0.1f));
            }
        }
    }
}

//Class permetant d'avoir le type de terrain
[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenality;
}

//Classe sommet (les cases) qui recoit
public class Sommet : IHeapItem<Sommet>
{

    public bool peuPasser;           //Permet de savoir si on peu passer
    public Vector3 worldPosition;   //Position dans le monde 
    public int grilleX;             //position en X dans la grille
    public int grilleY;             //Position en Y dans la grille
    public int movementPenalite;    //Penalité de mouvement relier à la case

    public int gCost;               //distance du point de départ
    public int hCost;               //distance du sommet d'arrivé
    public Sommet parent;           //Le parent de la case
    int heapIndex;                  //Indexe de l'arbre

    //constructeur
    public Sommet(bool peuPasser, Vector3 worldPos, int grilleX, int grilleY, int movementPenalite)
    {
        this.peuPasser = peuPasser;
        this.worldPosition = worldPos;
        this.grilleX = grilleX;
        this.grilleY = grilleY;
        this.movementPenalite = movementPenalite;
    }

    //Permet de get le fCost qui est la somme du gCost et du hCost
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Indexe du heap (tas)
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //Permet de comparer deux sommet ensemble (leur fCost)
    public int CompareTo(Sommet sommetAComparer)
    {
        //CompareTo renvoit 1 si le int est plus gros, 0 si égale
        //Et -1 si plus petit
        int compare = fCost.CompareTo(sommetAComparer.fCost);

        //Si leur fCost est égale, alors on utilise le hCost 
        //(distance entre le pointActuelle et le point à comparer)
        if (compare == 0)
        {
            compare = hCost.CompareTo(sommetAComparer.hCost);
        }

        //Compare to renvoie -1 si plus petit mais nous le plus petit a un plus grande
        //priorité, donc on veut qu'il renvoit 1
        return -compare;
    }
}
