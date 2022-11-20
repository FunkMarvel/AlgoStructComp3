using System.Collections.Generic;
using System.Linq;
using Graph;
using Unity.VisualScripting;
using UnityEngine;
using Edge = Graph.Edge;

public class Dijkstra : MonoBehaviour
{
    public List<Node> CreatedNodes;
    public List<Edge> openEdges = new();
    private readonly float _searchInterval = 0.5f;

    private float _searchTimer;
    private bool bGotPath;
    private bool done;

    private Node goalNode;
    private GraphLogic graph;
    private Node lastPos;

    private int NumberOfNodes;
    private Node startNode;

    private void Awake()
    {
        graph = FindObjectOfType<GraphLogic>();
    }

    private void Start()
    {
        if (graph.currentAlgorithm == DataInstance.Algorithm.Dijkstra)
        {
            BeginSearch();
        }
        else
        {
            done = true;
            bGotPath = true;
        }
    }

    // Update is called once per frame
    private void Update()
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

    // Start is called before the first frame update
    private void BeginSearch()
    {
        CreatedNodes = graph.Nodes;
        NumberOfNodes = graph.Nodes.Count;

        Debug.Log("Antall nodes: " + NumberOfNodes);
        Debug.Log("BEFORE");
        startNode = graph.Nodes[0];
        goalNode = graph.Nodes[NumberOfNodes - 1];
        Debug.Log("ree");

        lastPos = startNode;
        startNode.SetColor(Color.magenta);
        goalNode.SetColor(Color.red);
    }

    private void Search(Node thisNode)
    {
        if (thisNode == goalNode)
        {
            done = true;
            return;
        }

        foreach (var NextNode in thisNode.connectedNodes)
        {
            var G = thisNode.timeToFinish;
            var H = GraphLogic.Distance(thisNode, NextNode);
            var F = G + H;

            var TempEdge = thisNode.GetEdge(NextNode);
            TempEdge.UpdateEdge(G, H, F);

            if (TempEdge.bOpen)
            {
                TempEdge.parent = thisNode;
                openEdges.Add(TempEdge);
            }
        }

        openEdges = openEdges.OrderBy(p => p.F).ThenBy(n => n.H).ToList();
        var pm = openEdges.ElementAt(0);
        pm.bOpen = false;
        thisNode = pm.parent;

        openEdges.RemoveAt(0);

        foreach (var e in openEdges) e.SetColor(Color.blue);

        pm.SetColor(Color.red);

        if (!pm.connectedNodes.Contains(thisNode))
            Debug.LogError("Edge-node mismatch! " + thisNode.NodeID + " : " + pm.EdgeID);
        lastPos = pm.connectedNodes.Find(e => e != thisNode);
        lastPos.parent = thisNode;
    }

    private void GetPath()
    {
        //RemoveAllMarkers();
        var begin = lastPos;

        while (startNode != begin)
        {
            Debug.Log("NodeNr: " + begin.NodeID);
            var TempEdge = begin.GetEdge(begin.parent);
            Debug.Log("Found Edge: " + TempEdge.EdgeID);
            TempEdge.SetColor(Color.green);
            begin = begin.parent;

            if (begin.IsUnityNull()) break;
        }
    }
}