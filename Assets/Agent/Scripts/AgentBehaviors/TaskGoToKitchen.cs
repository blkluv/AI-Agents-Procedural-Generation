using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskGoToKitchen : BTNode
{
    private Transform _transform;
    NavMeshAgent agent;

    public TaskGoToKitchen(Transform transform)
    {
        _transform = transform;
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override BTNodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        agent.SetDestination(target.position);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.velocity.sqrMagnitude == 0)
                {
                    AgentNeedsUpdate.agentState = AgentState.EATING;
                }
            }
        }

        if (AgentNeedsUpdate.agentState == AgentState.EATING)
        {
            AgentNeedsUpdate.hunger = 0;
            ClearData("target");
        }

        state = BTNodeState.RUNNING;
        return state;
    }
}
