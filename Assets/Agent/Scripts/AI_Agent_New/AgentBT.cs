using UnityEngine;
using UnityEngine.AI;

public class AgentBT : MonoBehaviour
{
    public UtilityAI utilityAI;
    public NavMeshAgent navMeshAgent;

    void Update()
    {
        string currentPriority = utilityAI.EvaluatePriorities();

        switch (currentPriority)
        {
            case "Eat":
                // Execute Eat behavior
                break;
            case "Sleep":
                // Execute Sleep behavior
                break;
            case "Work":
                // Execute Work behavior
                break;
                // Add other cases as needed
        }
    }
}
