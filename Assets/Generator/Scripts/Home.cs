using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// Root class for Home creation
public class Home : MonoBehaviour
{
    public NavMeshSurface meshSurface;

    public int homewidth, homeLength;
    public int roomWidth, roomLength;
    public int corridorWidth;
    public int maxIterations;
    
    [Range(0.0f,0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f,1.0f)]
    public float roomTopCornerModifier;
    [Range(0,2)]
    public int roomOffset;
    
    // Game object stuff
    public Material material;
    public GameObject wallHorizontal, wallVertical, AIAgent;
    private int numberOfAgents = 0;
    int increment;

    // UI Stuff
    public Text UI;

    // A dictionary that contains the need and the associated room for the need
    public static Dictionary<string, GameObject> room_needs = new Dictionary<string, GameObject>()
    {
        { "relax", null},
        { "sleep", null},
        { "eat", null},
        { "work", null},
        { "shower", null},

    };

    public string[] room_names = { "living", "bedroom", "kitchen", "workroom", "bathroom" };
    public float[] roomSizes; // info variable for sorting the rooms based on the size

    List<Vector3Int> doorHorizontalPos;
    List<Vector3Int> doorVerticalPos;
    List<Vector3Int> wallHorizontalPos;
    List<Vector3Int> wallVerticalPos;

    // Start is called before the first frame update
    void Start()
    {
        CreateHome();
        meshSurface.BuildNavMesh(); // Bake NavMesh for each procedurally generated house
    }

    public void CreateHome()
    {
        roomSizes = new float[5];
        increment = 0;
        DestroyHome(); // Destroys all the game objects
        HomeArea homeArea = new HomeArea(homewidth, homeLength);

        var allRooms = homeArea.CalculatePartitions(maxIterations, roomWidth, roomLength, roomBottomCornerModifier, roomTopCornerModifier, roomOffset, corridorWidth);
        
        // Parent object for all the walls
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;

        // Door and wall position vectors
        doorHorizontalPos = new List<Vector3Int>();
        doorVerticalPos = new List<Vector3Int>();
        wallHorizontalPos = new List<Vector3Int>();
        wallVerticalPos = new List<Vector3Int>();

        // Creates mesh for all the rooms (rooms include corridors as well)
        for (int i = 0; i < allRooms.Count; i++) 
        {
            CreateMesh(allRooms[i].BottomLeftAreaCorner, allRooms[i].TopRightAreaCorner);
            increment++;
        }

        // debug info for room sizes
        Array.Sort(roomSizes);
        Array.Reverse(roomSizes);
        for (int n = 0; n < roomSizes.Length; n++)
        {
            Debug.Log("Room size: " + roomSizes[n]);
        }

        // Creates walls
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in wallHorizontalPos)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }

        foreach (var wallPostion in wallVerticalPos)
        {
            CreateWall(wallParent, wallPostion, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    // Mesh creation
    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftVertex = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightVertex = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftVertex = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightVertex = new Vector3(topRightCorner.x, 0, topRightCorner.y);
        float currentMeshSize = 0;

        Vector3[] vertices = new Vector3[]
        {
            topLeftVertex,
            topRightVertex,
            bottomLeftVertex,
            bottomRightVertex
        };

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++) 
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,1,2,2,1,3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        if (increment < room_names.Length)
        {
            // The parent room holder
            GameObject houseFloor = new GameObject(room_names[increment]);
            houseFloor.transform.localPosition = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2, 0, (bottomLeftCorner.y + topRightCorner.y) / 2);
            houseFloor.transform.parent = transform;

            // Floor
            GameObject floorMesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
            floorMesh.transform.position = Vector3.zero;
            floorMesh.GetComponent<MeshFilter>().mesh = mesh;
            floorMesh.GetComponent<MeshRenderer>().material = material;
            floorMesh.transform.parent = houseFloor.transform;
            //Debug.Log(houseFloor.name + ": " + (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.x) * (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.z));

            if (increment < roomSizes.Length) 
            {
                currentMeshSize = (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.x) * (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.z);
                roomSizes[increment] = currentMeshSize;
            } 

            // temp waypoints - currently using the room parent holder location as the waypoint
            GameObject wayPoint = new GameObject("WayPoint");
            if (increment < room_names.Length) 
            {
                string k = room_needs.ElementAt(increment).Key;
                room_needs[k] = houseFloor;
            }

            wayPoint.transform.localPosition = houseFloor.transform.position;
            wayPoint.transform.parent = houseFloor.transform;

            // Instantiate a single agent in the first created room
            if (numberOfAgents < 1)
            {
                Instantiate(AIAgent, houseFloor.transform.position, Quaternion.identity, houseFloor.transform);
                numberOfAgents++;
            }
        }
        else 
        {
            // all corridor objects
            GameObject houseFloor = new GameObject("Corridor");
            houseFloor.transform.localPosition = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2, 0, (bottomLeftCorner.y + topRightCorner.y) / 2);
            houseFloor.transform.parent = transform;

            GameObject floorMesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
            floorMesh.transform.position = Vector3.zero;
            floorMesh.GetComponent<MeshFilter>().mesh = mesh;
            floorMesh.GetComponent<MeshRenderer>().material = material;
            floorMesh.transform.parent = houseFloor.transform;
            Debug.Log(houseFloor.name+ ": " + (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.x) * (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.z));
            
            if(increment < roomSizes.Length)
                roomSizes[increment] = (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.x) * (floorMesh.GetComponent<MeshFilter>().sharedMesh.bounds.size.z);

            GameObject wayPoint = new GameObject("WayPoint");
            wayPoint.transform.localPosition = houseFloor.transform.position;
            wayPoint.transform.parent = houseFloor.transform;
        }

        // add horizontal and vertical positions for the walls
        for (int row = (int)bottomLeftVertex.x; row < (int)bottomRightVertex.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftVertex.z);
            AddWallPosition(wallPosition, wallHorizontalPos, doorHorizontalPos);
        }
        for (int row = (int)topLeftVertex.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightVertex.z);
            AddWallPosition(wallPosition, wallHorizontalPos, doorHorizontalPos);
        }
        for (int col = (int)bottomLeftVertex.z; col < (int)topLeftVertex.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftVertex.x, 0, col);
            AddWallPosition(wallPosition, wallVerticalPos, doorVerticalPos);
        }
        for (int col = (int)bottomRightVertex.z; col < (int)topRightVertex.z; col++)
        {
            var wallPosition = new Vector3(bottomRightVertex.x, 0, col);
            AddWallPosition(wallPosition, wallVerticalPos, doorVerticalPos);
        }
    }

    private void AddWallPosition(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);

        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyHome()
    {
        while (transform.childCount != 0) 
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
