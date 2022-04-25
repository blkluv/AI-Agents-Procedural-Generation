using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A corridor node class that determines the orientation of the corridor based on the rooms to connect
public class CorridorNode : Node
{
    private Node corridorStart;
    private Node corridorEnd;
    private int corridorWidth;
    private int distanceFromWall = 1;

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        this.corridorStart = node1;
        this.corridorEnd = node2;
        this.corridorWidth = corridorWidth;
        GenerateCorridor();
    }

    private void GenerateCorridor() 
    {
        var corridorPosition = ValidateBridge();
        
        switch (corridorPosition)
        {
            case PositionOrientation.Up:
                ConnectRoomsVertical(this.corridorStart, this.corridorEnd);
                break;
            case PositionOrientation.Down:
                ConnectRoomsVertical(this.corridorEnd, this.corridorStart);
                break;
            case PositionOrientation.Right:
                ConnectRoomsHorizontal(this.corridorStart, this.corridorEnd);
                break;
            case PositionOrientation.Left:
                ConnectRoomsHorizontal(this.corridorEnd, this.corridorStart);
                break;
            default:
                break;
        }
    }

    private void ConnectRoomsHorizontal(Node corridorStart, Node corridorEnd)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = RoomStructure.ExtractTerminalNodes(corridorStart);
        Node rightStructure = null;
        List<Node> rightStructureChildren = RoomStructure.ExtractTerminalNodes(corridorEnd);

        var sortLeft = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if (sortLeft.Count == 1)
        {
            leftStructure = sortLeft[0];
        }
        else 
        {
            int max_X = sortLeft[0].TopRightAreaCorner.x;
            sortLeft = sortLeft.Where(children => Math.Abs(max_X - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortLeft.Count);
            leftStructure = sortLeft[index];
        }

        var checkNeighboursRight = rightStructureChildren.Where(child => GetValueY(leftStructure.TopRightAreaCorner, leftStructure.BottomRightAreaCorner, child.TopLeftAreaCorner, child.BottomLeftAreaCorner) != 1).OrderBy(child => child.BottomRightAreaCorner.x).ToList();

        if (checkNeighboursRight.Count <= 0)
        {
            rightStructure = corridorEnd;
        }
        else 
        {
            rightStructure = checkNeighboursRight[0];
        }

        int y = GetValueY(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner, rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        while (y == -1 && sortLeft.Count > 1) 
        {
            sortLeft = sortLeft.Where(child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortLeft[0];
            y = GetValueY(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner, rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        }
        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + this.corridorWidth);
    }

    private int GetValueY(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        // 
        if (rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y) 
        {
            return RoomStructure.CalculateMidPoint(leftNodeDown + new Vector2Int(0, distanceFromWall), leftNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;
        }

        //
        if (rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y) 
        {
            return RoomStructure.CalculateMidPoint(rightNodeDown + new Vector2Int(0, distanceFromWall), rightNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;
        }

        //
        if (leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y) 
        {
            return RoomStructure.CalculateMidPoint(rightNodeDown + new Vector2Int(0, distanceFromWall), leftNodeUp - new Vector2Int(0, distanceFromWall)).y;
        }

        //
        if (leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y) 
        {
            return RoomStructure.CalculateMidPoint(leftNodeDown + new Vector2Int(0, distanceFromWall), rightNodeUp - new Vector2Int(0, distanceFromWall + this.corridorWidth)).y;
        }

        return -1;
    }

    private void ConnectRoomsVertical(Node corridorStart, Node corridorEnd)
    {
        Node bottomStructure = null;
        List<Node> bottomStructureChildren = RoomStructure.ExtractTerminalNodes(corridorStart);
        Node topStructure = null;
        List<Node> topStructureChildren = RoomStructure.ExtractTerminalNodes(corridorEnd);

        var sortBottom = bottomStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

        if (sortBottom.Count == 1)
        {
            bottomStructure = bottomStructureChildren[0];
        }
        else
        {
            int max_Y = sortBottom[0].TopLeftAreaCorner.y;
            sortBottom = sortBottom.Where(child => Mathf.Abs(max_Y - child.TopLeftAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortBottom.Count);
            bottomStructure = sortBottom[index];
        }

        var checkNeighboursTop = topStructureChildren.Where
            (child => GetValueX(bottomStructure.TopLeftAreaCorner, bottomStructure.TopRightAreaCorner, 
            child.BottomLeftAreaCorner, child.BottomRightAreaCorner) != -1).OrderBy(child => child.BottomRightAreaCorner.y).ToList();
        
        if (checkNeighboursTop.Count == 0)
        {
            topStructure = corridorEnd;
        }
        else
        {
            topStructure = checkNeighboursTop[0];
        }

        int x = GetValueX(
            bottomStructure.TopLeftAreaCorner, 
            bottomStructure.TopRightAreaCorner, 
            topStructure.BottomLeftAreaCorner, 
            topStructure.BottomRightAreaCorner);

        while (x == -1 && sortBottom.Count > 1)
        {
            sortBottom = sortBottom.Where(child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortBottom[0];
            x = GetValueX(
                bottomStructure.TopLeftAreaCorner, 
                bottomStructure.TopRightAreaCorner, 
                topStructure.BottomLeftAreaCorner, 
                topStructure.BottomRightAreaCorner);
        }

        BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x + this.corridorWidth, topStructure.BottomLeftAreaCorner.y);
    }

    private int GetValueX(Vector2Int bottomNodeLeft, Vector2Int bottomNodeRight, Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        if (topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return RoomStructure.CalculateMidPoint(bottomNodeLeft + new Vector2Int(distanceFromWall, 0), bottomNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        }
        if (topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return RoomStructure.CalculateMidPoint(topNodeLeft + new Vector2Int(distanceFromWall, 0), topNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        }
        if (bottomNodeLeft.x >= (topNodeLeft.x) && bottomNodeLeft.x <= topNodeRight.x)
        {
            return RoomStructure.CalculateMidPoint(bottomNodeLeft + new Vector2Int(distanceFromWall, 0), topNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        }
        if (bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return RoomStructure.CalculateMidPoint(topNodeLeft + new Vector2Int(distanceFromWall, 0), bottomNodeRight - new Vector2Int(this.corridorWidth + distanceFromWall, 0)).x;
        }

        return -1;
    }

    private PositionOrientation ValidateBridge()
    {
        Vector2 centerA = ((Vector2)corridorStart.TopRightAreaCorner + corridorStart.BottomLeftAreaCorner) / 2;
        Vector2 centerB = ((Vector2)corridorEnd.TopRightAreaCorner + corridorEnd.BottomLeftAreaCorner) / 2;
        float angle = CalculateAngle(centerA, centerB);

        if ((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return PositionOrientation.Right;
        }

        else if ((angle > 45 && angle < 135))
        {
            return PositionOrientation.Up;
        }

        else if ((angle > -135 && angle < -45))
        {
            return PositionOrientation.Down;
        }
        else 
        {
            return PositionOrientation.Left;
        }
    }

    private float CalculateAngle(Vector2 centerA, Vector2 centerB)
    {
        return Mathf.Atan2(centerB.y - centerA.y, centerB.x - centerA.x) * Mathf.Rad2Deg;
    }
}