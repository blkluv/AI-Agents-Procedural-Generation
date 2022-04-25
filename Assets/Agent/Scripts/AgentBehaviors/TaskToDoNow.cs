using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

// Currently not implemented - handles task updates based on time
public class TaskToDoNow : BTNode
{
    private Transform _transform;
    NavMeshAgent agent;

    public TaskToDoNow(Transform transform)
    {
        _transform = transform;
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override BTNodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        agent.SetDestination(target.position);
        state = BTNodeState.RUNNING;
        return state;
    }
}
