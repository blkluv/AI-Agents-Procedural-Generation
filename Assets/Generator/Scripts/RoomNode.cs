using UnityEngine;

// Holds room info in a node
public class RoomNode: Node
{
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, Node parentNode, int index) : base(parentNode) 
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y); // setting this as per the x,y of the above
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, topRightAreaCorner.y); // setting this as per the x,y of the above
        this.TreeLayerIndex = index;
    }

    public int GetWidth { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }
    public int GetLength { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }
}