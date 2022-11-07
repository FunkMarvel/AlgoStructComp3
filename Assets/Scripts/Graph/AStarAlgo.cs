using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Graph;


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

    private List<PathMarker> open = new List<PathMarker>();
    private List<PathMarker> closed = new List<PathMarker>();
    
    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    private PathMarker goalNode;
    private PathMarker startNode;

    private PathMarker lastPos;
    private bool done = false;

    public List<Node> CreatedNodes;

    // Start is called before the first frame update
    void BeginSearch()
    {
        
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }
        
        
        
       float G = 
        
        
        
    }
    void Start()
    {
        
    }

    private void Awake()
    {
        var graph = FindObjectOfType<GraphLogic>();
        CreatedNodes = graph.Nodes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
