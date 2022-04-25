using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskGoToLiving : BTNode
{
    private Transform _transform;
    NavMeshAgent agent;

    public TaskGoToLiving(Transform transform)
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
                    AgentNeedsUpdate.agentState = AgentState.RELAXING;
                }
            }
        }

        if (AgentNeedsUpdate.agentState == AgentState.RELAXING)
        {
            AgentNeedsUpdate.stress = 0;
            ClearData("target");
        }

        state = BTNodeState.RUNNING;
        return state;
    }
}
