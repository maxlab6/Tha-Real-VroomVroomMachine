using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_generator : MonoBehaviour
{
    private int Longeur = 500;
    private int Largeur = 512;
    private int HauteurRandom;

    private float offsetX;
    private float offsetZ;
    private float ScaleRandom;
    private bool ChangerTerrain;
    
 

    private void Start()
    {
        RandomValue();
    }

    private void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private void FixedUpdate()
    {
        ChangerTerrain = Controle_Pickup.BoutonChanger;

        if(ChangerTerrain == true)
        {
            RandomValue();
            Controle_Pickup.BoutonChanger = false;
        }

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = 530;
        terrainData.size = new Vector3(Longeur, HauteurRandom, Largeur);
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

        float xCoord = (float)x / Longeur * ScaleRandom + offsetX;
        float zCoord = (float)z / Largeur * ScaleRandom + offsetZ;
        return Mathf.PerlinNoise(xCoord, zCoord);

    }

    private void RandomValue()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        HauteurRandom = Random.Range(1, 40);

        ScaleRandom = Random.Range(8f, (112 * Mathf.Pow(HauteurRandom, -0.65f)));

    }
}

