using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public bool isFlyingCreature = false;
    public float speedFly = 50f;
    public Vector3 FlyPosition;
    private GameObject GoalPosition;
    private Vector3 checkPoint;
    private NavMeshAgent myNavMeshAgent;
    private Enemy enemy;
    private EnemyAnimation enemyAnimation;
    private GameManager gameManager;
    //private ManagerUI managerUI;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        SetGoalDestination();
        gameManager = GameManager.instance;
        //managerUI = gameManager.GetComponent<ManagerUI>();
    }

    public void CheckIfCanReachGoal()
    {
        NavMeshPath path = new NavMeshPath();
        myNavMeshAgent.CalculatePath(GoalPosition.transform.position, path);
        checkPoint = path.corners.LastOrDefault();

        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                //managerUI.HidePanelInformation();
                ReachGoalDestination();
                break;
            case NavMeshPathStatus.PathPartial:
                //managerUI.SetPanelInformationText("Warning! Path is blocked!");
                enemy.FindNearEnemy();
                break;
            case NavMeshPathStatus.PathInvalid:
                break;
        }
    }

    void Update()
    {
        if (gameManager.LoseFlag) return;

        if (enemy.IsDead)
        {
            if (!isFlyingCreature)
                myNavMeshAgent.enabled = false;
        }
        else
        {
            if (!isFlyingCreature)
                CheckIfCanReachGoal();
            else
                Fly();
        }

    }

    /// <summary>
    /// Set destination for nav agent
    /// </summary>
    /// <param name="pos"></param>
    public void ReachDestination(Vector3 pos)
    {
        myNavMeshAgent.isStopped = false;
        enemyAnimation.PlayWalk();

        if (checkPoint != myNavMeshAgent.destination)
        {
            myNavMeshAgent.SetDestination(pos);
        }
    }

    /// <summary>
    /// Set the goal position as destination for nav agent
    /// </summary>
    public void SetGoalDestination()
    {
        GoalPosition = GameObject.FindGameObjectWithTag("Goal");

        if (!isFlyingCreature)
            ReachGoalDestination();
        else
            enemyAnimation.PlayWalk();
    }

    public void ReachGoalDestination()
    {
        ReachDestination(GoalPosition.transform.localPosition);
    }

    /// <summary>
    /// Stop movements
    /// </summary>
    public void StopMoving()
    {
        if (myNavMeshAgent.isStopped) return;

        myNavMeshAgent.SetDestination(transform.position);
        myNavMeshAgent.isStopped = true;
        enemyAnimation.PlayIdle();
    }

    public void Fly()
    {
        // The step size is equal to speed times frame time.
        float step = speedFly * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, (GoalPosition.transform.position + FlyPosition), step);
        transform.LookAt(GoalPosition.transform);
    }

    /// <summary>
    /// Check if position is in range
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool InRange(Vector3 position)
    {
        float dist = Vector3.Distance(position, transform.position);

        return dist < enemy.rangeAttack;
    }

    /// <summary>
    /// Set nav agent destination
    /// </summary>
    /// <param name="pos"></param>
    public void SetDestination(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        myNavMeshAgent.CalculatePath(pos, path);
        checkPoint = path.corners.LastOrDefault();

        if (checkPoint != myNavMeshAgent.destination)
        {
            myNavMeshAgent.isStopped = false;
            enemyAnimation.PlayWalk();
            myNavMeshAgent.SetDestination(pos);
        }
    }

    public void SetSpeed(float speed)
    {
        myNavMeshAgent.speed += speed;
    }
}
