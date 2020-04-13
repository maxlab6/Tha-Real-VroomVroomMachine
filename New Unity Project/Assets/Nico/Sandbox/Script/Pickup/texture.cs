using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class texture : MonoBehaviour
{
    public Transform playerTransform;

    public Terrain Terrain01;
    public Terrain Terrain02;
    public Terrain Terrain03;
    public Terrain Terrain04;
    public Terrain Terrain05;
    public Terrain Terrain06;
    public Terrain Terrain07;
    public Terrain Terrain08;
    public Terrain Terrain09;
    public Terrain Terrain10;
    public Terrain Terrain11;
    public Terrain Terrain12;
    public Terrain Terrain13;
    public Terrain Terrain14;
    public Terrain Terrain15;
    public Terrain Terrain16;

    public Terrain t;



    public int posX;
    public int posZ;
    public float[] textureValues;

    void Start()
    {
        playerTransform = gameObject.transform;
        t = Terrain.activeTerrain;
    }

    void FixedUpdate()
    {
        if(playerTransform.position.x > 0 && playerTransform.position.x <501 && playerTransform.position.z > 0 && playerTransform.position.z < 501)
        {
            t = Terrain01;
        }
        else if(playerTransform.position.x > 500 && playerTransform.position.x < 1001 && playerTransform.position.z > 0 && playerTransform.position.z < 501)
        {
            t = Terrain02;
        }
        else if(playerTransform.position.x > 1000 && playerTransform.position.x < 1501 && playerTransform.position.z > 0 && playerTransform.position.z < 501)
        {
            t = Terrain03;
        }
        else if (playerTransform.position.x > 1500 && playerTransform.position.x < 2001 && playerTransform.position.z > 0 && playerTransform.position.z < 501)
        {
            t = Terrain04;
        }
        else if (playerTransform.position.x > 0 && playerTransform.position.x < 501 && playerTransform.position.z > 500 && playerTransform.position.z < 1001)
        {
            t = Terrain05;
        }
        else if (playerTransform.position.x > 500 && playerTransform.position.x < 1001 && playerTransform.position.z > 500 && playerTransform.position.z < 1001)
        {
            t = Terrain06;
        }
        else if (playerTransform.position.x > 1000 && playerTransform.position.x < 1501 && playerTransform.position.z > 500 && playerTransform.position.z < 1001)
        {
            t = Terrain07;
        }
        else if (playerTransform.position.x > 1500 && playerTransform.position.x < 2001 && playerTransform.position.z > 500 && playerTransform.position.z < 1001)
        {
            t = Terrain08;
        }
        else if (playerTransform.position.x > 0 && playerTransform.position.x < 501 && playerTransform.position.z > 1000 && playerTransform.position.z < 1501)
        {
            t = Terrain09;
        }
        else if (playerTransform.position.x > 500 && playerTransform.position.x < 1001 && playerTransform.position.z > 1000 && playerTransform.position.z < 1501)
        {
            t = Terrain10;
        }
        else if (playerTransform.position.x > 1000 && playerTransform.position.x < 1501 && playerTransform.position.z > 1000 && playerTransform.position.z < 1501)
        {
            t = Terrain11;
        }
        else if (playerTransform.position.x > 1500 && playerTransform.position.x < 2001 && playerTransform.position.z > 1000 && playerTransform.position.z < 1501)
        {
            t = Terrain12;
        }
        else if (playerTransform.position.x > 0 && playerTransform.position.x < 501 && playerTransform.position.z > 1500 && playerTransform.position.z < 2001)
        {
            t = Terrain13;
        }
        else if (playerTransform.position.x > 500 && playerTransform.position.x < 1001 && playerTransform.position.z > 1500 && playerTransform.position.z < 2001)
        {
            t = Terrain14;
        }
        else if (playerTransform.position.x > 1000 && playerTransform.position.x < 1501 && playerTransform.position.z > 1500 && playerTransform.position.z < 2001)
        {
            t = Terrain15;
        }
        else if (playerTransform.position.x > 1500 && playerTransform.position.x < 2001 && playerTransform.position.z > 1500 && playerTransform.position.z < 2001)
        {
            t = Terrain16;
        }

        GetTerrainTexture();
    }

    public void GetTerrainTexture()
    {
        ConvertPosition(playerTransform.position);
        CheckTexture();
    }

    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;

        Vector3 mapPosition = new Vector3(terrainPosition.x / t.terrainData.size.x, 0,terrainPosition.z / t.terrainData.size.z);

        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ,1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];
        textureValues[4] = aMap[0, 0, 4];
    }
}
