using System;
using System.Collections.Generic;
using UnityEngine;

// Abstract class to build the tree structure
public abstract class Node
{
    private List<Node> childNodesList;

    public List<Node> GetChildNodesList { get => childNodesList; }

    public bool Visited { get; set; }

    public Vector2Int BottomLeftAreaCorner { get; set; }

    public Vector2Int BottomRightAreaCorner { get; set; }

    public Vector2Int TopLeftAreaCorner { get; set; }

    public Vector2Int TopRightAreaCorner { get; set; }

    public Node Parent { get; set; }

    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode) 
    {
        childNodesList = new List<Node>();
        this.Parent = parentNode;

        if (parentNode != null) 
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childNodesList.Add(node);
    }

    public void RemoveChild(Node node) 
    {
        childNodesList.Remove(node);
    }
}

