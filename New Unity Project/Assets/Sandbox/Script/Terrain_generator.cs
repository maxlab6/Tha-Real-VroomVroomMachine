using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_generator : MonoBehaviour
{
    public int Hauteur;
    public int Longeur;
    public int Largeur;

    public float scale;

    private float offsetX;
    private float offsetZ;



    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = Longeur + 30;
        terrainData.size = new Vector3(Longeur, Hauteur, Largeur);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;

    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[Longeur, Largeur];
        for (int x = 0; x < Longeur; x++)
        {
            for (int z = 0; z < Largeur; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int z)
    {

        float xCoord = (float)x / Longeur * scale + offsetX;
        float zCoord = (float)z / Largeur * scale + offsetZ;
        return Mathf.PerlinNoise(xCoord, zCoord);



    }

}

