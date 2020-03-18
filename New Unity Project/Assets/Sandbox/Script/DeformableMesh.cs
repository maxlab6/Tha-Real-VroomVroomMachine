using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{
    public float maximumDepth;

    public Vector3[] originalVerticies;
    public Vector3[] modifiedVerticies;

    private GeneratePlaneMesh plane;
    public void MeshRegenerated()
    {
        plane = GetComponent<GeneratePlaneMesh>();

    }
}
