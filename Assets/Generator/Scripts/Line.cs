using UnityEngine;

// Used for splitting the partitions and orientation
public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates) 
    {
        this.Orientation = orientation;
        this.Coordinates = coordinates;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }
}

public enum Orientation 
{
    Horizontal = 0,
    Vertical = 1
}