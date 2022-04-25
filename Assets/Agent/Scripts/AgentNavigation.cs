using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Class to handle agent navigation using Navmenh and waypoints (Rooms) - Functionality currently under control of behavior tree
public class AgentNavigation : MonoBehaviour
{
    ////GameObject[] waypoints = Home.wayPoints;
    //private int currentWaypointIndex = 0;
    //private float speed = 5f;

    //NavMeshAgent agent;

    //private void Start()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //private void Update()
    //{
    //    Transform targetWaypoint = waypoints[currentWaypointIndex].transform;
    //    Debug.Log(targetWaypoint.position);
    //    agent.SetDestination(waypoints[currentWaypointIndex].transform.position);

    //    if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
    //    {
    //        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            
    //    }
    //    else 
    //    {
            
    //        //transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
    //        //transform.LookAt(targetWaypoint.position);
    //    }
    //}

    //public int GetWaypointsCount() 
    //{
    //    return waypoints.Length;
    //}
}
