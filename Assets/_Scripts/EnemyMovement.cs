using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    public GameObject GoalPosition;
    private NavMeshAgent myNavMeshAgent;   
    private Enemy enemy;
    private EnemyAnimation enemyAnimation;
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        SetGoalDestination();
    }

    void Update()
    {
        Debug.Log("is stopped: " + myNavMeshAgent.isStopped);
        if (enemy.IsDead)
        {
            myNavMeshAgent.enabled = false;
        }

        NavMeshPath path = new NavMeshPath();
        myNavMeshAgent.CalculatePath(GoalPosition.transform.position, path);

        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                ReachGoalDestination();
                break;
            case NavMeshPathStatus.PathPartial:
                Debug.Log("Warning: path cannot reach destination");
                enemy.FindNearEnemy();
                break;
            case NavMeshPathStatus.PathInvalid:
                break;
        }

        //float distanceToTarget = Vector3.Distance(transform.position, myNavMeshAgent.destination);
        //if (distanceToTarget < destinationReachedTreshold)
        //{
        //    enemyAnimation.PlayIdle();
        //}
    }

    public void ReachDestination(Vector3 pos)
    {
        myNavMeshAgent.isStopped = false;
        enemyAnimation.PlayWalk();
        myNavMeshAgent.SetDestination(pos);
    }

    public void SetGoalDestination()
    {
        GoalPosition = GameObject.FindGameObjectWithTag("Goal");       
        ReachGoalDestination();
    }

    public void ReachGoalDestination()
    {
        ReachDestination(GoalPosition.transform.position);
    }

    public bool IsReachDestination(Vector3 pos)
    {        
        float distanceToTarget = Vector3.Distance(transform.position, pos);
        if (distanceToTarget < myNavMeshAgent.stoppingDistance)
            return true;
        else
            return false;

    }

    public bool IsMoving()
    {
        return myNavMeshAgent.isStopped;
    }

    public void StopMoving()
    {
        myNavMeshAgent.isStopped = true;
        //enemyAnimation.PlayIdle();
    }
    // TODO: metodo per modificare la movement speed
}
