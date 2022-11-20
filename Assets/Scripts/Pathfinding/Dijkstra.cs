using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Dijkstra : MonoBehaviour
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
    private bool bGotPath = false;
    
    private float _searchTimer = 0f;
    private float _searchInterval = 0.5f;

    public List<Node> CreatedNodes;
    public List<Edge> Neighbours = new List<Edge>();

    // Start is called before the first frame update
    void BeginSearch()
    {
        CreatedNodes = graph.Nodes;
        NumberOfNodes = graph.Nodes.Count;
        
        Debug.Log("Antall nodes: " + NumberOfNodes);
        Debug.Log("BEFORE");
        startNode = graph.Nodes[0];
        goalNode = graph.Nodes[NumberOfNodes-1];
        Debug.Log("ree");
        
        lastPos = startNode;
        startNode.SetColor(Color.magenta);
        goalNode.SetColor(Color.red);
    }

    void Search(Node thisNode)
    {
        if (thisNode==goalNode)
        {
            done = true;
            return;
        }

        foreach (Node NextNode in thisNode.connectedNodes)
        {
            float G = thisNode.timeToFinish;
            float H = GraphLogic.Distance(thisNode, NextNode);
            float F = G + H;
            
            var TempEdge = thisNode.GetEdge(NextNode);
            TempEdge.UpdateEdge(G, H, F);
                
            if (TempEdge.bOpen)
            {
                TempEdge.parent = thisNode;
                Neighbours.Add(TempEdge);
            }
        }
        
        Neighbours = Neighbours.OrderBy(p => p.F).ThenBy(n => n.H).ToList();
        Edge pm = Neighbours.ElementAt(0);
        pm.bOpen = false;
        thisNode = pm.parent;
        
        Neighbours.RemoveAt(0);

        foreach (Edge e in Neighbours)
        {
            e.SetColor(Color.blue);
        }
        
        pm.SetColor(Color.red);

        if(!pm.connectedNodes.Contains(thisNode)) Debug.LogError("Edge-node mismatch! " + thisNode.NodeID + " : " + pm.EdgeID);
        lastPos = pm.connectedNodes.Find(e => e != thisNode);
        lastPos.parent = thisNode;

    }
    
    void GetPath()
    {
        //RemoveAllMarkers();
        Node begin = lastPos;

        while ((startNode !=begin))
        {
            Debug.Log("NodeNr: " + begin.NodeID);
            var TempEdge = begin.GetEdge(begin.parent);
            Debug.Log("Found Edge: " + TempEdge.EdgeID);
            TempEdge.SetColor(Color.green);
            begin = begin.parent;

            if (begin.IsUnityNull())
            {
                break;
            }
        }
        
    }
    void Start()
    {
        if (graph._currentAlgorithm == DataInstance.Algorithm.Dijkstra)
        {
            BeginSearch();
        }
        else
        {
            done = true;
            bGotPath = true;
        }
    }

    private void Awake()
    {
        graph = FindObjectOfType<GraphLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
        if (Input.GetKeyDown(KeyCode.C) && !done) Search(lastPos);
        if (Input.GetKeyDown(KeyCode.M)) GetPath();

        if (!done && _searchTimer < _searchInterval)
        {
            _searchTimer += Time.deltaTime;
        }
        else if (!done)
        {
            _searchTimer = 0f;
            Search(lastPos);
        }
        
        if (done && !bGotPath)
        {
            GetPath();
            bGotPath = true;
        }
       
    }
}

