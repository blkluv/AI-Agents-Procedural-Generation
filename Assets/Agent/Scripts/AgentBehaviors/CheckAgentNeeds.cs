using BehaviorTree;
using UnityEngine;

// Checks the agent needs as per the sequence and sets the agents next room target
public class CheckAgentNeeds : BTNode
{
    private string _need;
    private int _needValue;
    private GameObject _room;

    public CheckAgentNeeds(string need, int value, GameObject room) 
    {
        _need = need; // need name
        _needValue = value; // value that determines the urgency of the need
        _room = room; // Gameobject of the room where the need can be fulfilled
    }

    public override BTNodeState Evaluate()
    {
        object target = GetData("target"); // Gets the target gameobject - room

        if (target == null) 
        {
            if (_need == "sleep" && _needValue <= 15) // energy <= 0
            {
                parent.parent.SetData("target", _room.transform);
                _needValue = AgentNeedsUpdate.energy;
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "wake" && _needValue >= 100) // energy >= 100
            {
                parent.parent.SetData("target", _room.transform);
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "shower" && _needValue <= 5) // hygiene <= 0
            {
                parent.parent.SetData("target", _room.transform);
                _needValue = AgentNeedsUpdate.hygiene;
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "eat" && _needValue >= 70) // hunger >= 100
            {
                parent.parent.SetData("target", _room.transform);
                _needValue = AgentNeedsUpdate.hunger;
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "drink" && _needValue >= 50) // thirsty >= 100
            {
                parent.parent.SetData("target", _room.transform);
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "work" && _needValue <= 0) // stress <= 0
            {
                parent.parent.SetData("target", _room.transform);
                _needValue = AgentNeedsUpdate.energy;
                state = BTNodeState.SUCCESS;
                return state;
            }

            if (_need == "relax" && _needValue >= 70) // stress >= 100
            {
                parent.parent.SetData("target", _room.transform);
                _needValue = AgentNeedsUpdate.stress;
                state = BTNodeState.SUCCESS;
                return state;
            }

            state = BTNodeState.FAILURE;
            return state;
        }
        state = BTNodeState.SUCCESS;
        return state;
    }
}
