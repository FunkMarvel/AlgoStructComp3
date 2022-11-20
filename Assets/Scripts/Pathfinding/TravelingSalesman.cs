using System.Collections.Generic;
using System.Linq;
using Graph;
using UnityEngine;

public class TravelingSalesman : MonoBehaviour
{
    public GraphLogic graph;

    private void Awake()
    {
        graph = FindObjectOfType<GraphLogic>();

        graph.Nodes = graph.Nodes.OrderBy(Node => Node.NodeID).ToList();

        var costMatrix = new List<List<float>>();
        for (var i = 0; i < graph.numberOfNodes; i++)
        {
            var column = new List<float>();
            column.Capacity = graph.numberOfNodes;

            for (var j = 0; j < graph.numberOfNodes; j++)
            {
                var edge = graph.Nodes[i].GetEdge(graph.Nodes[j]);
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
    }

    // Update is called once per frame
    private void Update()
    {
    }
}