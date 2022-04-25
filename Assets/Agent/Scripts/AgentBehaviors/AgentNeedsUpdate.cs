using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Contains all the need stats of the agent
public class AgentNeedsUpdate : MonoBehaviour
{
    public static AgentState agentState;

    // Agent Stats
    [HideInInspector] public static int energy = 100; // 100 means fully energetic
    [HideInInspector] public static int hygiene = 0; // 100 = clean
    [HideInInspector] public static int hunger = 70; // 0 = not hungry, 100 = hungry
    [HideInInspector] public static int thirst = 0; // same as above
    [HideInInspector] public static int stress = 0; // same as above
    public static string currentTask;

    public Material material;

    // UI variables
    Text hungerText;
    Text energyText;
    Text thirstText;
    Text stressText;
    Text hygieneText;
    Text currentRoomText;
    GameObject holder;

    private void Start()
    {
        material = transform.GetChild(0).GetComponent<Renderer>().material;

        //holder = GameObject.FindGameObjectWithTag("Holder");
        //hungerText = holder.transform.GetChild(0).GetComponent<Text>();
        //energyText = holder.transform.GetChild(1).GetComponent<Text>();
        //thirstText = holder.transform.GetChild(2).GetComponent<Text>();
        //stressText = holder.transform.GetChild(3).GetComponent<Text>();
        //hygieneText = holder.transform.GetChild(4).GetComponent<Text>();
        //currentRoomText = holder.transform.GetChild(5).GetComponent<Text>();

        //agentState = AgentState.AWAKE;
        //currentTask = "work";
    }

    private void Update()
    {
        // Change agent color based on the state
        switch (agentState)
        {
            case AgentState.SLEEPING:
                material.color = Color.green;
                break;
            case AgentState.SHOWERING:
                material.color = Color.blue;
                break;
            case AgentState.EATING:
                material.color = Color.yellow;
                break;
            case AgentState.WORKING:
                material.color = Color.blue;
                break;
            case AgentState.RELAXING:
                material.color = Color.white;
                break;
            default:
                material.color = Color.red;
                break;
        }

        //hungerText.text = "Hunger: " + hunger.ToString();
        //energyText.text = "Energy: " + energy.ToString();
        //thirstText.text = "Thirst: " + thirst.ToString();
        //stressText.text = "Stress: " + stress.ToString();
        //hygieneText.text = "Hygiene: " + hygiene.ToString();

        //if (energy > 70)
        //{
        //    currentTask = "work";
        //}

        //if (stress > 90)
        //{
        //    currentTask = "relax";
        //}

        //if (thirst > 80)
        //{
        //    currentTask = "drink";
        //}

        //if (hunger > 60)
        //{
        //    currentTask = "eat";
        //}

        //if (hygiene < 5)
        //{
        //    currentTask = "shower";
        //}

        //if (energy < 10)
        //{
        //    currentTask = "sleep";
        //}
    }

    // Stats handling methods - not used in the demo
    public static void RunAllTaskMethods()
    {
        Sleep();
        Idle();
        Shower();
        Eat();
        Drink();
        Work();
        Relax();
    }

    public static void CheckStatBounds()
    {
        if (energy >= 100)
            energy = 100;
        if (energy <= 0)
            energy = 0;

        if (hygiene >= 100)
            hygiene = 100;
        if (hygiene <= 0)
            hygiene = 0;

        if (hunger >= 100)
            hunger = 100;
        if (hunger <= 0)
            hunger = 0;

        if (thirst >= 100)
            thirst = 100;
        if (thirst <= 0)
            thirst = 0;

        if (stress >= 100)
            stress = 100;
        if (stress <= 0)
            stress = 0;
    }

    public static void Sleep()
    {
        if (agentState == AgentState.SLEEPING)
        {
            CheckStatBounds();
            energy += 5;
            hygiene -= 1;
            hunger += 3;
            thirst += 5;
            stress -= 5;
        }
    }

    // Awake and not doing anything
    public static void Idle()
    {
        if (agentState == AgentState.AWAKE)
        {
            CheckStatBounds();
            energy -= 1;
            hygiene -= 1;
            hunger += 1;
            thirst += 2;
            stress -= 1;
        }
    }

    public static void Shower()
    {
        if (agentState == AgentState.SHOWERING)
        {
            CheckStatBounds();
            hygiene += 10;
            stress -= 2;
        }
    }

    public static void Eat()
    {
        if (agentState == AgentState.EATING)
        {
            CheckStatBounds();
            hunger -= 10;
        }
    }

    public static void Drink()
    {
        if (agentState == AgentState.DRINKING)
        {
            CheckStatBounds();
            thirst -= 10;
        }
    }

    public static void Work()
    {
        if (agentState == AgentState.WORKING)
        {
            CheckStatBounds();
            energy -= 10;
            hygiene -= 3;
            hunger += 3;
            thirst += 1;
            stress += 5;
        }
    }

    public static void Relax()
    {
        if (agentState == AgentState.RELAXING)
        {
            CheckStatBounds();
            energy += 2;
            stress -= 10;
        }
    }
}

public enum AgentState
{
    AWAKE,
    SLEEPING,
    SHOWERING,
    EATING,
    WORKING,
    DRINKING,
    RELAXING
}
