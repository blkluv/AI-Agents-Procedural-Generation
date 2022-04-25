using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

// A random roam behavior that makes the agent roam around the house from one room to another room
public class Roam : BTNode
{
    GameObject _waypoints;
    Transform _transform;

    private int currentWaypointIndex;
    bool currentWaypointReached = false;

    private float _waitTime = 2f;
    private float _waitCounter = 0;
    private bool _waiting = false;

    NavMeshAgent agent;

    public Roam(Transform transform, GameObject waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        agent = transform.GetComponent<NavMeshAgent>();
    }

    public override BTNodeState Evaluate() 
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime) 
            {
                _waiting = false;
                currentWaypointReached = false;
            }
        }
        else 
        {
            agent.SetDestination(_waypoints.transform.position);

            //if (_waypoints[currentWaypointIndex] != null)
            //{
            //    Debug.Log(_waypoints[currentWaypointIndex].transform.position);
            //    Transform targetWaypoint = _waypoints[currentWaypointIndex].transform;

            //if (!currentWaypointReached)
            //{
            //    agent.SetDestination(_waypoints[currentWaypointIndex].transform.position);

            //    if (agent.velocity.magnitude <= 0)
            //    {
            //        currentWaypointReached = true;
            //        _waitCounter = 0f;
            //        _waiting = true;
            //        currentWaypointIndex = (currentWaypointIndex + 1) % _waypoints.Length;
            //    }
            //}
            //} 
        }

        state = BTNodeState.RUNNING;
        return state;
    }
}
