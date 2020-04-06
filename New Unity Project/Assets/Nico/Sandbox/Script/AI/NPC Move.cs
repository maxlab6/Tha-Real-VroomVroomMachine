using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    [SerializeField]
    public Transform _destination;

    private NavMeshAgent _Agent;





    // Start is called before the first frame update
    void Start()
    {
        _Agent = this.GetComponent<NavMeshAgent>();

        setDestination();

    }

    private void setDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _Agent.SetDestination(targetVector);
        }
    }

    
}
