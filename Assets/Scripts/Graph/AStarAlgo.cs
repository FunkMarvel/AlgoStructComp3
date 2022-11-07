using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Graph;
using Unity.VisualScripting;
using Edge = Graph.Edge;


public class PathMarker
{
    public GraphLogic location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(GraphLogic l, float g, float h, float f, GameObject marker, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return location.Equals(((PathMarker)obj).location);
    }

    public override int GetHashCode()
    {
        return 0;
    }
}
public class AStarAlgo : MonoBehaviour
{
    public GraphLogic GraphLogic;
    public Material closedMaterial;
    public Material openMaterial;
    
    
    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    private GraphLogic graph;
    
    private Node goalNode;
    private Node startNode;

    private int NumberOfNodes;
    private Node lastPos;
    private bool done = false;

    public List<Node> CreatedNodes;

    // Start is called before the first frame update
    void BeginSearch()
    {
        startNode = graph.Nodes[0];
        goalNode = graph.Nodes[NumberOfNodes-1];
    }

    void Search(Node thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }

        foreach (Node edges in thisNode.connectedNodes)
        {
            float G = GraphLogic.Distance(thisNode, edges) + thisNode.timeToFinish;
            float H = Graph.GraphLogic.Distance(edges, goalNode);
            float F = G + H;
            
             


        }
        
        
        
       // float G = GraphLogic.Distance(thisNode,Edge)


    }
    void Start()
    {
        
    }

    private void Awake()
    {
        graph = FindObjectOfType<GraphLogic>();
        CreatedNodes = graph.Nodes;
        NumberOfNodes = graph.Nodes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
