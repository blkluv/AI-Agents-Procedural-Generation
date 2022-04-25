using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorGenerator
{
    // Create connections between the rooms
    public List<Node> CreateCorridor(List<RoomNode> allRoomNodes, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> roomsCheck = new Queue<RoomNode>(allRoomNodes.OrderByDescending(node => node.TreeLayerIndex).ToList());
        while (roomsCheck.Count > 0) 
        {
            var node = roomsCheck.Dequeue();
            if (node.GetChildNodesList.Count == 0) 
            {
                continue;
            }

            CorridorNode corridor = new CorridorNode(node.GetChildNodesList[0], node.GetChildNodesList[1], corridorWidth);
            corridorList.Add(corridor);
        }

        return corridorList;
    }
}