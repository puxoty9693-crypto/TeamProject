using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private float arrivalTolerance = 0.05f;    // 도착지 허용오차

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        //2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // NPC간 충돌 제거
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    /// <summary>
    /// Order agent to set target's transform.position
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    public bool MoveTo(Vector3 destination)
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return false;      // null Component 에러 방지

        agent.isStopped = false;
        return agent.SetDestination(destination);
    }
    

    /// <summary>
    /// Order agent to reset target's transform.position
    /// Stop Movement
    /// </summary>
    public void Stop()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    public bool HasArrived()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return false;

        if (agent.pathPending) return false;        //

        float distance = agent.stoppingDistance + arrivalTolerance;

        return agent.remainingDistance <= distance;

    }

    public void ResetMovement(Vector3 position)
    {
        transform.position = position;
        
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        agent.ResetPath();
        agent.isStopped = false;
        agent.Warp(position);

    }

}
