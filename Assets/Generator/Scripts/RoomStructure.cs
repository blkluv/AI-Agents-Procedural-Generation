using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Define the room structure
public enum PositionOrientation
{
    Up,
    Down,
    Left,
    Right
}

// Makes a list of leaf nodes that make up the room
public static class RoomStructure
{
    public static List<Node> finalRoomsList = new List<Node>();

    // Traverses the node structure and checks for child nodes and extracts the terminal nodes
    public static List<Node> ExtractTerminalNodes(Node parentRoomNode) 
    {
        Queue<Node> nodes = new Queue<Node>();
        List<Node> finalNodesList = new List<Node>();

        if (parentRoomNode.GetChildNodesList.Count == 0) 
        {
            return new List<Node>() { parentRoomNode };
        }

        foreach (var child in parentRoomNode.GetChildNodesList) 
        {
            nodes.Enqueue(child);
        }

        while (nodes.Count > 0) 
        {
            var currentNode = nodes.Dequeue();

            if (currentNode.GetChildNodesList.Count == 0)
            {
                finalNodesList.Add(currentNode);
            }
            else 
            {
                foreach (var child in currentNode.GetChildNodesList) 
                {
                    nodes.Enqueue(child);
                }
            }
        }

        finalRoomsList = finalNodesList;

        return finalNodesList;
    }

    public static Vector2Int GenerateBottomLeftCorner(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float point, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * point)),
            Random.Range(minY, (int)(minY + (maxY - minY) * point)));
    }

    public static Vector2Int GenerateTopRightCorner(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float point, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * point), maxX),
            Random.Range((int)(minY + (maxY - minY) * point), maxY)
            );
    }

    public static Vector2Int CalculateMidPoint(Vector2Int vec1, Vector2Int vec2)
    {
        Vector2 sum = vec1 + vec2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}