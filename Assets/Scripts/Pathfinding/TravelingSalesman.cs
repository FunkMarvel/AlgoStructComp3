using System.Collections.Generic;
using System.Linq;
using Graph;
using UnityEngine;

public class TravelingSalesman : MonoBehaviour
{
    private GraphLogic _graph;
    private bool _done = false;
    private bool _bGotPath = false;

    private void Awake()
    {
        _graph = FindObjectOfType<GraphLogic>();

        _graph.nodes = _graph.nodes.OrderBy(Node => Node.NodeID).ToList();

        var costMatrix = new List<List<float>>();
        for (var i = 0; i < _graph.NumberOfNodes; i++)
        {
            var column = new List<float>();
            column.Capacity = _graph.NumberOfNodes;

            for (var j = 0; j < _graph.NumberOfNodes; j++)
            {
                var edge = _graph.nodes[i].GetEdge(_graph.nodes[j]);
                if (i == j)
                    column[j] = 0f;
                else if (edge)
                    column[j] = edge.timeToFinish;
                else
                    column[j] = float.PositiveInfinity;
            }

            costMatrix.Add(column);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (_graph.currentAlgorithm == DataInstance.Algorithm.TSP)
        {
            // BeginSearch();
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
}