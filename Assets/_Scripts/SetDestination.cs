using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetDestination : MonoBehaviour {

    NavMeshAgent myNavMeshAgent;
    public GameObject target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Goal");
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myNavMeshAgent.SetDestination(target.transform.position);
    }

    void Update()
    {        
    }
}
