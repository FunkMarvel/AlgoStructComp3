using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Graph
{
    public class GraphLogic : MonoBehaviour
    {
        public GameObject NodeObject;
        public GameObject EdgeObject;
        public List<Node> Nodes;
        public List<Edge> Edges;

        public float edgeThicknessPercentage = 0.05f * 100;

        public int numberOfNodes { get; private set; }
        public int numberOfEdges { get; private set; }
        public int numberOfEdgesPerNode { get; private set; }

        public float graphBoundingCubeLength;

        private void Awake()
        {
            GenerateGraph(10,5);
        }

        public void GenerateGraph(int size, int edgesPerNode)
        {
            numberOfNodes = size;
            numberOfEdgesPerNode = edgesPerNode;

            Nodes = new List<Node>(numberOfNodes);
            
            // create specified number of nodes
            for (int i = 0; i < numberOfNodes; i++)
            {
                Vector3 spawnPos = RandPos();
                Nodes.Add(Instantiate(NodeObject, spawnPos, Quaternion.identity).GetComponent<Node>());
                Nodes[i].SetNumberOfConnectedNodes(edgesPerNode);
                Nodes[i].position = spawnPos;
                Nodes[i].NodeID = i;
            }
            
            Shuffle(Nodes); // shuffle and connect nodes to create spanning tree
            for (int i = 1; i < numberOfNodes; i++)
            {
                CreateEdge(Nodes[i], Nodes[i-1]);
            }
            
            Shuffle(Nodes);
            for (int i = 0; i < numberOfNodes; i++)
            {
                for (int j = 0; j < numberOfNodes; j++)
                {
                    if (i == j) break;
                    if (Nodes[i].connectedNodes.Count >= numberOfEdgesPerNode) break;
                    if (Nodes[i].connectedNodes.Contains(Nodes[j])) break;
                    
                    CreateEdge(Nodes[i], Nodes[j]);
                }
            }
        }

        private Vector3 RandPos()
        {
            float bound = graphBoundingCubeLength * 0.5f;
            Vector3 spawnPos = new Vector3(
                Random.Range(-bound, bound),
                Random.Range(-bound, bound),
                Random.Range(-bound, bound)
            );
            return spawnPos;
        }

        /// <summary>
        /// Shuffles the order of nodes in the graph
        /// </summary>
        private static void Shuffle<T>(IList<T> nodeList) {
            for (var i = 0; i < nodeList.Count - 1; ++i) {
                var r = UnityEngine.Random.Range(i, nodeList.Count);
                (nodeList[i], nodeList[r]) = (nodeList[r], nodeList[i]);
            }
        }

        private void CreateEdge(Node a, Node b)
        {
            if (a == b) return;
            numberOfEdges++;
            
            a.connectedNodes.Add(b);
            b.connectedNodes.Add(a);
            
            Edges.Add(Instantiate(EdgeObject, a.position, Quaternion.identity).GetComponent<Edge>());
            Edges[numberOfEdges-1].ConnectEdge(a, b, edgeThicknessPercentage);
            Edges[numberOfEdges-1].EdgeID = numberOfEdges-1;
        }

        public Edge GetEdge(Node firstNode, Node secondNode)
        {
            if (firstNode == secondNode) return null;
            for (int i = 0; i < numberOfEdges; i++)
            {
                if ((Edges[i].connectedNodes[0] == firstNode && Edges[i].connectedNodes[1] == secondNode) ||
                    (Edges[i].connectedNodes[0] == secondNode && Edges[i].connectedNodes[1] == firstNode))
                {
                    return Edges[i];
                }
            }
            return null;
        }

        public static float Distance(Node A, Node B)
        {
            return (A.position - B.position).magnitude;
        }
    }
}
