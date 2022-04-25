using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

// The Agent behavior tree
public class SimBT : BTree
{
    public static float speed = 2f; // speed to move the agent - when using transform methods

    public static string[] tasks = { 
        "sleep", 
        "shower", 
        "eat", 
        "work", 
        "relax" 
    };

    protected override BTNode SetupTree()
    {
        BTNode root = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new CheckAgentNeeds("shower", AgentNeedsUpdate.hygiene, Home.room_needs["shower"]),
                new TaskGoToBathroom(transform),
            }),

            new Sequence(new List<BTNode>
            {
                new CheckAgentNeeds("eat", AgentNeedsUpdate.hunger, Home.room_needs["eat"]),
                new TaskGoToKitchen(transform),
            }),

            new Sequence(new List<BTNode>
            {
                new CheckAgentNeeds("work", AgentNeedsUpdate.stress, Home.room_needs["work"]),
                new TaskGoToWork(transform),
            }),

            new Sequence(new List<BTNode>
            {
                new CheckAgentNeeds("relax", AgentNeedsUpdate.stress, Home.room_needs["relax"]),
                new TaskGoToLiving(transform),
            }),

            new Sequence(new List<BTNode>
            {
                new CheckAgentNeeds("sleep", AgentNeedsUpdate.energy, Home.room_needs["sleep"]),
                new TaskGoToBedroom(transform),
            }),

            new Roam(transform, Home.room_needs[tasks[Random.Range(0,tasks.Length)]]),
        });
        return root;
    }
}
