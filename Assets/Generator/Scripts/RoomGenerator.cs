using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int roomWidth;
    private int roomLength;

    public RoomGenerator(int maxIterations, int roomWidth, int roomLength)
    {
        this.maxIterations = maxIterations;
        this.roomWidth = roomWidth;
        this.roomLength = roomLength;
    }

    public List<RoomNode> GenerateRooms(List<Node> roomSpace, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
    {
        List<RoomNode> roomNodesList = new List<RoomNode>();

        foreach (var space in roomSpace) 
        {
            Vector2Int bottomLeftPoint = RoomStructure.GenerateBottomLeftCorner(space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            Vector2Int topRightPoint = RoomStructure.GenerateTopRightCorner(space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);

            space.BottomLeftAreaCorner = bottomLeftPoint;
            space.TopRightAreaCorner = topRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(topRightPoint.x, bottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(bottomLeftPoint.x, topRightPoint.y);

            roomNodesList.Add((RoomNode)space);
        }

        return roomNodesList;
    }
}