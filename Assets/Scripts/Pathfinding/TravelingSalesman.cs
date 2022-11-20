using System.Collections.Generic;
using System.Linq;
using Graph;
using Unity.VisualScripting;
using UnityEngine;

public class TravelingSalesman : MonoBehaviour
{
    public enum TSPMethod
    {
        NearestNeighbour
    }

    public TSPMethod currentMethod = TSPMethod.NearestNeighbour;

    public float minCost;

    private readonly float _searchInterval = 0.5f;
    private bool _bGotPath;
    private Node _currentNode;
    private bool _done;

    private GraphLogic _graph;
    private float _searchTimer;

    private Node _startNode;
    public List<List<float>> costMatrix;

    private void Awake()
    {
        _graph = FindObjectOfType<GraphLogic>();

        _graph.nodes = _graph.nodes.OrderBy(Node => Node.NodeID).ToList();

        costMatrix = new List<List<float>>();
        costMatrix.Capacity = _graph.NumberOfNodes;

        for (var i = 0; i < _graph.NumberOfNodes; i++)
        {
            var column = new List<float>();
            column.Capacity = _graph.NumberOfNodes;

            for (var j = 0; j < _graph.NumberOfNodes; j++)
            {
                if (i == j)
                {
                    column.Add(0f);
                    continue;
                }

                var edge = _graph.nodes[i].GetEdge(_graph.nodes[j]);

                if (edge)
                    column.Add(edge.timeToFinish);
                else
                    column.Add(float.MaxValue);
            }

            costMatrix.Add(column);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_graph.currentAlgorithm == DataInstance.Algorithm.TSP)
        {
            BeginSearch();
        }
        else
        {
            _done = true;
            _bGotPath = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void BeginSearch()
    {
        switch (currentMethod)
        {
            case TSPMethod.NearestNeighbour:
                _startNode = _graph.nodes[0];
                _startNode.SetColor(Color.red);
                NearestNeighbourIterate(_startNode);
                break;
        }

        _startNode.GetEdge(_startNode.parent).UpdateEdge(minCost, 0f, 0f);

        foreach (var edge in _graph.edges)
        {
            Debug.Log("Making edge " + edge.edgeID + " invisible");
            if (edge.bOpen)
            {
                edge.IsVisible(false);
                Debug.Log("invi done");
            }
        }
    }

    private void NearestNeighbourIterate(Node someNode)
    {
        if (someNode.parent.IsUnityNull())
        {
        }
        else if (someNode.parent != _startNode && someNode == _startNode)
        {
            var tempEdge = someNode.GetEdge(someNode.parent);
            // tempEdge.SetColor(Color.green);
            tempEdge.bOpen = false;
            Debug.Log("reached node: " + someNode.NodeID);
            return;
        }

        var neighbours = costMatrix[someNode.NodeID];

        var minVal = float.MaxValue;
        var minIndex = -1;

        for (var i = 0; i < neighbours.Count; i++)
            if (neighbours[i] < minVal && someNode.NodeID != i)
            {
                if (_graph.nodes[i].bVisited) continue;
                minVal = neighbours[i];
                minIndex = i;
            }

        if (minIndex < 0)
        {
            _done = true;
            minIndex = 0;
        }

        ;

        minCost += costMatrix[someNode.NodeID][minIndex];

        var tempNode = someNode;
        someNode = _graph.nodes[minIndex];
        someNode.parent = tempNode;
        tempNode.bVisited = true;

        Debug.Log("moving from node " + tempNode.NodeID + " to node " + someNode.NodeID + " and is done?: " + _done);

        tempNode.GetEdge(someNode).SetColor(Color.red);

        NearestNeighbourIterate(someNode);

        if (_done && someNode == _startNode)
        {
            _bGotPath = true;
            Debug.Log("End on node: " + someNode.NodeID);
            return;
        }

        if (_done && !someNode.parent.IsUnityNull())
        {
            var tempEdge = someNode.GetEdge(someNode.parent);
            tempEdge.SetColor(Color.green);
            tempEdge.bOpen = false;
            Debug.Log("backtracking on node: " + someNode.NodeID);
        }
    }
}