using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Roue_Pickup : MonoBehaviour
{
    public WheelCollider WC_FL;
    public WheelCollider WC_FR;
    public WheelCollider WC_RL;
    public WheelCollider WC_RR;

    private WheelHit hit;

    private string TypeSol;

    private void FixedUpdate()
    {
        DetectionSol(WC_FL);
        DetectionSol(WC_FR);
        DetectionSol(WC_RL);
        DetectionSol(WC_RR);
    }



    private void DetectionSol(WheelCollider _collider)
    {
        _collider.GetGroundHit(out hit);


        TypeSol = hit.collider.gameObject.tag;


        WheelFrictionCurve W_F_F;
        WheelFrictionCurve W_F_S;

        W_F_F = _collider.forwardFriction;
        W_F_S = _collider.sidewaysFriction;

        if (TypeSol == "Asphalte")
        {
            W_F_F.extremumSlip = 0.8f;
            W_F_F.extremumValue = 1f;
            W_F_F.asymptoteSlip = 0.8f;
            W_F_F.asymptoteValue = 0.5f;
            W_F_F.stiffness = 1.2f;

            W_F_S.extremumSlip = 0.4f;
            W_F_S.extremumValue = 1f;
            W_F_S.asymptoteSlip = 0.5f;
            W_F_S.asymptoteValue = 0.75f;
            W_F_S.stiffness = 1.1f;
        }
        else if (TypeSol == "Herbe")
        {
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
        else if (TypeSol == "Rampe")
        {
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

        _collider.forwardFriction = W_F_F;
        _collider.sidewaysFriction = W_F_S;

    }
}
