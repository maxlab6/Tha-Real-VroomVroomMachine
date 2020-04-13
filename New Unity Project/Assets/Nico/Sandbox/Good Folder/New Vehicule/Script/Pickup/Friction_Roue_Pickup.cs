using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction_Roue_Pickup : MonoBehaviour
{
    public WheelCollider WheelCollider;
    private Transform WheelTransform;

    //Déterminer Terrain
    private Terrain Terrain01;
    private Terrain Terrain02;
    private Terrain Terrain03;
    private Terrain Terrain04;
    private Terrain Terrain05;
    private Terrain Terrain06;
    private Terrain Terrain07;
    private Terrain Terrain08;
    private Terrain Terrain09;
    private Terrain Terrain10;
    private Terrain Terrain11;
    private Terrain Terrain12;
    private Terrain Terrain13;
    private Terrain Terrain14;
    private Terrain Terrain15;
    private Terrain Terrain16;
    public Terrain t;
    private int posX;
    private int posZ;

    //Détection Objet
    private WheelHit hit;
    private string ObjetToucher;

    //Detection Texture
    private int numeroTexture;
    public int nombreTexture;



    void Start()
    {
        WheelTransform = gameObject.transform;

        Terrain01 = GameObject.Find("SandBox/Map/Land Plot/Terrain_1").GetComponent<Terrain>();
        Terrain02 = GameObject.Find("SandBox/Map/Land Plot/Terrain_2").GetComponent<Terrain>();
        Terrain03 = GameObject.Find("SandBox/Map/Land Plot/Terrain_3").GetComponent<Terrain>();
        Terrain04 = GameObject.Find("SandBox/Map/Land Plot/Terrain_4").GetComponent<Terrain>();
        Terrain05 = GameObject.Find("SandBox/Map/Land Plot/Terrain_5").GetComponent<Terrain>();
        Terrain06 = GameObject.Find("SandBox/Map/Land Plot/Terrain_6").GetComponent<Terrain>();
        Terrain07 = GameObject.Find("SandBox/Map/Land Plot/Terrain_7").GetComponent<Terrain>();
        Terrain08 = GameObject.Find("SandBox/Map/Land Plot/Terrain_8").GetComponent<Terrain>();
        Terrain09 = GameObject.Find("SandBox/Map/Land Plot/Terrain_9").GetComponent<Terrain>();
        Terrain10 = GameObject.Find("SandBox/Map/Land Plot/Terrain_10").GetComponent<Terrain>();
        Terrain11 = GameObject.Find("SandBox/Map/Land Plot/Terrain_11").GetComponent<Terrain>();
        Terrain12 = GameObject.Find("SandBox/Map/Land Plot/Terrain_12").GetComponent<Terrain>();
        Terrain13 = GameObject.Find("SandBox/Map/Land Plot/Terrain_13").GetComponent<Terrain>();
        Terrain14 = GameObject.Find("SandBox/Map/Land Plot/Terrain_14").GetComponent<Terrain>();
        Terrain15 = GameObject.Find("SandBox/Map/Land Plot/Terrain_15").GetComponent<Terrain>();
        Terrain16 = GameObject.Find("SandBox/Map/Land Plot/Terrain_16").GetComponent<Terrain>();
        t = Terrain.activeTerrain;
    }

    void FixedUpdate()
    {
        DetectionObjet();
        DetectionTerrain();
        ConversionPositionRoues();
        DeterminerTexture();
        FrictionRoue();

    }

    private void DetectionObjet()
    {
        WheelCollider.GetGroundHit(out hit);
        ObjetToucher = hit.collider.gameObject.tag;
    }

    private void DetectionTerrain()
    {
        if (WheelTransform.position.x > 0 && WheelTransform.position.x < 501 && WheelTransform.position.z > 0 && WheelTransform.position.z < 501)
        {
            t = Terrain01;
        }
        else if (WheelTransform.position.x > 500 && WheelTransform.position.x < 1001 && WheelTransform.position.z > 0 && WheelTransform.position.z < 501)
        {
            t = Terrain02;
        }
        else if (WheelTransform.position.x > 1000 && WheelTransform.position.x < 1501 && WheelTransform.position.z > 0 && WheelTransform.position.z < 501)
        {
            t = Terrain03;
        }
        else if (WheelTransform.position.x > 1500 && WheelTransform.position.x < 2001 && WheelTransform.position.z > 0 && WheelTransform.position.z < 501)
        {
            t = Terrain04;
        }
        else if (WheelTransform.position.x > 0 && WheelTransform.position.x < 501 && WheelTransform.position.z > 500 && WheelTransform.position.z < 1001)
        {
            t = Terrain05;
        }
        else if (WheelTransform.position.x > 500 && WheelTransform.position.x < 1001 && WheelTransform.position.z > 500 && WheelTransform.position.z < 1001)
        {
            t = Terrain06;
        }
        else if (WheelTransform.position.x > 1000 && WheelTransform.position.x < 1501 && WheelTransform.position.z > 500 && WheelTransform.position.z < 1001)
        {
            t = Terrain07;
        }
        else if (WheelTransform.position.x > 1500 && WheelTransform.position.x < 2001 && WheelTransform.position.z > 500 && WheelTransform.position.z < 1001)
        {
            t = Terrain08;
        }
        else if (WheelTransform.position.x > 0 && WheelTransform.position.x < 501 && WheelTransform.position.z > 1000 && WheelTransform.position.z < 1501)
        {
            t = Terrain09;
        }
        else if (WheelTransform.position.x > 500 && WheelTransform.position.x < 1001 && WheelTransform.position.z > 1000 && WheelTransform.position.z < 1501)
        {
            t = Terrain10;
        }
        else if (WheelTransform.position.x > 1000 && WheelTransform.position.x < 1501 && WheelTransform.position.z > 1000 && WheelTransform.position.z < 1501)
        {
            t = Terrain11;
        }
        else if (WheelTransform.position.x > 1500 && WheelTransform.position.x < 2001 && WheelTransform.position.z > 1000 && WheelTransform.position.z < 1501)
        {
            t = Terrain12;
        }
        else if (WheelTransform.position.x > 0 && WheelTransform.position.x < 501 && WheelTransform.position.z > 1500 && WheelTransform.position.z < 2001)
        {
            t = Terrain13;
        }
        else if (WheelTransform.position.x > 500 && WheelTransform.position.x < 1001 && WheelTransform.position.z > 1500 && WheelTransform.position.z < 2001)
        {
            t = Terrain14;
        }
        else if (WheelTransform.position.x > 1000 && WheelTransform.position.x < 1501 && WheelTransform.position.z > 1500 && WheelTransform.position.z < 2001)
        {
            t = Terrain15;
        }
        else if (WheelTransform.position.x > 1500 && WheelTransform.position.x < 2001 && WheelTransform.position.z > 1500 && WheelTransform.position.z < 2001)
        {
            t = Terrain16;
        }
    }

    private void ConversionPositionRoues()
    {
        Vector3 terrainPosition = WheelTransform.transform.position - t.transform.position;
        Vector3 mapPosition = new Vector3(terrainPosition.x / t.terrainData.size.x, 0, terrainPosition.z / t.terrainData.size.z);
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    private void DeterminerTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        for (int x = 0; x < nombreTexture; x++)
        {
            if (aMap[0, 0, x] == 1)
            {
                numeroTexture = x + 1;
            }
        }
    }

    private void FrictionRoue()
    {
        WheelFrictionCurve W_F_F;
        WheelFrictionCurve W_F_S;

        W_F_F = WheelCollider.forwardFriction;
        W_F_S = WheelCollider.sidewaysFriction;


        if (ObjetToucher == "Rampe")
        {
            //Objet Rampe
            W_F_F.extremumSlip = 0.8f;
            W_F_F.extremumValue = 1f;
            W_F_F.asymptoteSlip = 0.8f;
            W_F_F.asymptoteValue = 0.5f;
            W_F_F.stiffness = 2f;

            W_F_S.extremumSlip = 0.4f;
            W_F_S.extremumValue = 1f;
            W_F_S.asymptoteSlip = 0.5f;
            W_F_S.asymptoteValue = 0.75f;
            W_F_S.stiffness = 2f;
        }
        else
        {
            if (numeroTexture == 1)
            {
                //Texture Herbe
                W_F_F.extremumSlip = 0.8f;
                W_F_F.extremumValue = 1f;
                W_F_F.asymptoteSlip = 0.8f;
                W_F_F.asymptoteValue = 0.5f;
                W_F_F.stiffness = 1f;

                W_F_S.extremumSlip = 0.4f;
                W_F_S.extremumValue = 1f;
                W_F_S.asymptoteSlip = 0.5f;
                W_F_S.asymptoteValue = 0.75f;
                W_F_S.stiffness = 1f;
            }
            else
            {
                //Pas de texture
                W_F_F.extremumSlip = 0.8f;
                W_F_F.extremumValue = 1f;
                W_F_F.asymptoteSlip = 0.8f;
                W_F_F.asymptoteValue = 0.5f;
                W_F_F.stiffness = 0.001f;

                W_F_S.extremumSlip = 0.4f;
                W_F_S.extremumValue = 1f;
                W_F_S.asymptoteSlip = 0.5f;
                W_F_S.asymptoteValue = 0.75f;
                W_F_S.stiffness = 0.001f;
            }
        }

        WheelCollider.forwardFriction = W_F_F;
        WheelCollider.sidewaysFriction = W_F_S;
    }
}
