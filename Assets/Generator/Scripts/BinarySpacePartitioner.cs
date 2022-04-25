using UnityEngine;
using System.Collections.Generic;
using System;

public class BinarySpacePartitioner
{
    RoomNode root;
    public static int[] areas;
    int increment = 0;

    public RoomNode Root { get => root; }

    public BinarySpacePartitioner(int homeWidth, int homeLength)
    {
        this.root = new RoomNode(new Vector2Int(0, 0), new Vector2Int(homeWidth, homeLength), null, 0);
    }

    // Makes the number of rooms with room widht and length 
    public List<RoomNode> MakeRooms(int maxIterations, int roomWidth, int roomLength) 
    {
        areas = new int[maxIterations];
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> roomsList = new List<RoomNode>();
        graph.Enqueue(this.root);
        roomsList.Add(this.root);
        int iterations = 0;

        while (iterations < maxIterations && graph.Count > 0) 
        {
            increment = iterations;

            RoomNode currentNode = graph.Dequeue();

            if (currentNode.GetWidth >= roomWidth * 2 || currentNode.GetLength >= roomLength * 2) 
            {
                SplitSpace(currentNode, roomsList, roomLength, roomWidth, graph);
            }

            iterations++;
        }

        return roomsList;
    }

    // Split the main space
    private void SplitSpace(RoomNode currentNode, List<RoomNode> roomsList, int roomLength, int roomWidth, Queue<RoomNode> graph)
    {
        Line line = GetLine(
            currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomWidth,
            roomLength);

        RoomNode node1, node2;
        if (line.Orientation == Orientation.Horizontal)
        {
            // Create nodes that make up the 2 corners of the room for horizontal orientation
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner, new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y), currentNode, currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y), currentNode.TopRightAreaCorner, currentNode, currentNode.TreeLayerIndex + 1);
        }

        else 
        {
            // Create nodes that make up the 2 corners of the room for vertical orientation
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner, new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y), currentNode, currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y), currentNode.TopRightAreaCorner, currentNode, currentNode.TreeLayerIndex + 1);
        }

        AddNodes(roomsList, graph, node1);
        AddNodes(roomsList, graph, node2);
    }

    private void AddNodes(List<RoomNode> roomsList, Queue<RoomNode> graph, RoomNode node)
    {
        roomsList.Add(node);
        areas[increment] = node.GetWidth * node.GetLength;
        graph.Enqueue(node);
    }

    // Divide the partition based on the orientation - horizontal/vertical
    private Line GetLine(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidth, int roomLength)
    {
        Orientation orientation;

        bool isWidthValid = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidth;
        bool isLengthValid = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLength;

        if (isLengthValid && isWidthValid)
        {
            orientation = (Orientation)UnityEngine.Random.Range(0, 2);
        }
        else if (isWidthValid)
        {
            orientation = Orientation.Vertical;
        }
        else 
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation,
            OrientationVector(orientation, bottomLeftAreaCorner, topRightAreaCorner, roomWidth, roomLength));
    }

    // Get x,y values for the orientation vector
    private Vector2Int OrientationVector(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidth, int roomLength)
    {
        Vector2Int coordinates = Vector2Int.zero;

        if (orientation == Orientation.Horizontal) 
        {
            coordinates = new Vector2Int(0, UnityEngine.Random.Range((bottomLeftAreaCorner.y + roomLength), (topRightAreaCorner.y - roomLength)));
        }
        else 
        {
            coordinates = new Vector2Int(UnityEngine.Random.Range((bottomLeftAreaCorner.x + roomWidth), (topRightAreaCorner.x - roomWidth)), 0);
        }

        return coordinates;
    }
}