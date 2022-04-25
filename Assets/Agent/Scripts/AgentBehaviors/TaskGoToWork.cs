using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskGoToWork : BTNode
{
    private Transform _transform;
    NavMeshAgent agent;

    public TaskGoToWork(Transform transform) 
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
                    AgentNeedsUpdate.agentState = AgentState.WORKING;
                }
            }
        }

        if (AgentNeedsUpdate.agentState == AgentState.WORKING)
        {
            AgentNeedsUpdate.stress = 100;
            AgentNeedsUpdate.energy = 5;
            ClearData("target");
        }

        state = BTNodeState.RUNNING;
        return state;
    }
}
