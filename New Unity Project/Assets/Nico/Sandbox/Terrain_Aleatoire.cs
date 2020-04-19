﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_Aleatoire : MonoBehaviour
{
    private int Longeur = 2000;
    private int Largeur = 2048;
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

        if (ChangerTerrain == true)
        {
            RandomValue();
            Controle_Pickup.BoutonChanger = false;
        }

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = 2000;
        terrainData.size = new Vector3(Longeur, HauteurRandom, Largeur);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[Longeur, Largeur];
        for(int x = 0;x<1536; x++)
        {
            for (int z = 0; z < 2000; z++)
            {
                heights[x, z] = 0;
            }
        }

        for (int x = 1536; x < 2000; x++)
        {
            for (int z = 0; z < 1536; z++)
            {
                heights[x, z] = 0;
            }
            /*
            for (int z = 1536; z < 2000; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
            */
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