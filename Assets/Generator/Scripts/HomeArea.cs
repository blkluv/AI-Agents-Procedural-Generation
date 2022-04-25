using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class for calculating the partitions of the rooms
public class HomeArea
{
    private int homewidth;
    private int homeLength;

    List<RoomNode> allRoomNodes = new List<RoomNode>();

    public HomeArea(int homewidth, int homeLength)
    {
        this.homewidth = homewidth;
        this.homeLength = homeLength;
    }

    public List<Node> CalculatePartitions(int maxIterations, int roomWidth, int roomLength, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner BSP = new BinarySpacePartitioner(homewidth, homeLength);
        allRoomNodes = BSP.MakeRooms(maxIterations, roomWidth, roomLength);
        List<Node> roomSpace = RoomStructure.ExtractTerminalNodes(BSP.Root); // List for all the final rooms
        
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomWidth, roomLength);
        List<RoomNode> roomsList = roomGenerator.GenerateRooms(roomSpace, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allRoomNodes, corridorWidth);

        return new List<Node>(roomsList).Concat(corridorList).ToList();
    }
}
