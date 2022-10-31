# AI-Agents-Procedural-Generation

Video Documentation of this project, URL - https://youtu.be/obu4n33KHIw

Interactive Agents and Procedural Generation
Demo documentation
References for code

NavMesh - Unity's NavMesh Components is being used to bake the navmesh and navigate the agent

Generator
* All the code was created from scratch. But I have referred to the provided materials and also a tutorial series to create the generator
* Link - https://www.youtube.com/watch?v=VnqN0v95jtU&list=PLcRSafycjWFfEPbSSjGMNY-goOZTuBPMW
* Files
   * Home - The main class that creates the home
   * HomeArea - Calculates the partitions and Generates the rooms in the space using the Binary space partitioning algorithm
   * BinarySpacePartitioner - Implements the algorithm
   * Room Generator - Handles the generation of the rooms and stores them in a list
   * Node and RoomNode - Node classes that hold the room data
   * RoomStructure and Line - Defines the room structure and its orientation (Horizontal or vertical)
   * CorridorGenerator and Corridor - Generates the corridors and holds the corridor data in a Corridor node


Agent
* Files
   * BTNode - Implements the node data structure
   * BTree - Abstract class for the agent behaviour trees
   * Selector and Sequence - Handle the flow and execution of the behaviours/actions
   * SimBT - The main behaviour class that defines the agent behaviour
   * Roam - An agent behaviour tree node that makes the agent roam around the house using rooms as waypoints and navmesh to navigate
   * CheckAgentNeeds - A BT Node that checks the agent needs as per the sequence and sets the agents next target/task
   * TaskGoTo - Defines the tasks for each room
      * Bathroom
      * Bedroom
      * Kitchen
      * Living
      * Work
      * DoNow
   * AgentNeedsUpdate - Currently handles the state of the agent that enables the agent to update the stats as per the state. This also handles the colour change as per the state
      * Sleeping state
      * Working state
      * Eating state
      * Showering state
      * Relaxing state
