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
        public Node TSPStart;
        public Node TSPEnd;

        public float edgeThicknessPercentage = 0.05f * 100;

        public int numberOfNodes { get; private set; }
        public int numberOfEdges { get; private set; }
        public int numberOfEdgesPerNode { get; private set; }

        public float graphBoundingCubeLength;
        private DataInstance _dataInstance;
        public DataInstance.Algorithm _currentAlgorithm = DataInstance.Algorithm.AStar;


        private void Awake()
        {
            _dataInstance = FindObjectOfType<DataInstance>();
            if (_dataInstance)
            {
                if (_dataInstance.NumberOfNodes != null && _dataInstance.NumberOfEdgesPerNode != null)
                {
                    GenerateGraph((int)_dataInstance.NumberOfNodes, (int)_dataInstance.NumberOfEdgesPerNode);
                    Debug.Log("DataExist: " + _dataInstance.NumberOfNodes + " " + _dataInstance.NumberOfEdgesPerNode);
                }
                else
                {
                    GenerateGraph(20,3);
                    Debug.Log("DataExist default: " + _dataInstance.NumberOfNodes + " " + _dataInstance.NumberOfEdgesPerNode);
                }

                if (_dataInstance.ChosenAlgorithm != null)
                {
                    _currentAlgorithm = (DataInstance.Algorithm)_dataInstance.ChosenAlgorithm;
                }
            }
            else
            {
                GenerateGraph(20,3);
                Debug.Log("DataNotExist default: 20 3");
            }

            var edge = Nodes[0].GetEdge(Nodes[numberOfNodes - 1]);

           // if (edge) edge.SetColor(Color.red);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
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
                    if (Nodes[j].connectedNodes.Count >= numberOfEdgesPerNode) continue;
                    if (Nodes[i].connectedNodes.Contains(Nodes[j])) continue;
                    
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
            a.connectedEdges[b] = Edges[numberOfEdges - 1];
            b.connectedEdges[a] = Edges[numberOfEdges - 1];
        }

        public Edge GetEdge(Node firstNode, Node secondNode)
        {
            if (firstNode.connectedEdges.ContainsKey(secondNode)) return firstNode.connectedEdges[secondNode];
            return null;
        }

        public static float Distance(Node A, Node B)
        {
            return (A.position - B.position).magnitude;
        }
    }
}
