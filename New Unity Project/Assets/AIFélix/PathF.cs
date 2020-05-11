using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathF : MonoBehaviour
{
    public Color lineColor;

    private List<Transform> checkPoints = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        Transform[] pathTransform = GetComponentsInChildren<Transform>();

        checkPoints = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != transform)
            {
                checkPoints.Add(pathTransform[i]);
            }
        }

        for (int i = 0; i < checkPoints.Count; i++)
        {
            Vector3 checkPointActuel = checkPoints[i].position;
            Vector3 checkPointPrecedent = Vector3.zero;

            if (i > 0)
            {
                checkPointPrecedent = checkPoints[i - 1].position;
            }
            else if (i == 0 && checkPoints.Count > 1)
            {
                checkPointPrecedent = checkPoints[checkPoints.Count - 1].position;
            }

            Gizmos.DrawLine(checkPointPrecedent, checkPointActuel);
        }
    }
}
