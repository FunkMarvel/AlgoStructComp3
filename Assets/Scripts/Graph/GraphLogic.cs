using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Graph
{
    public class GraphLogic : MonoBehaviour
    {
        public GameObject nodeObject;
        public GameObject edgeObject;
        public List<Node> nodes;
        public List<Edge> edges;
        public Node tspStart;
        public Node tspEnd;

        public float edgeThicknessPercentage = 0.05f * 100;

        public float graphBoundingCubeLength;
        public DataInstance.Algorithm currentAlgorithm = DataInstance.Algorithm.AStar;
        private DataInstance _dataInstance;

        public int NumberOfNodes { get; private set; }
        public int NumberOfEdges { get; private set; }
        public int NumberOfEdgesPerNode { get; private set; }


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
                    GenerateGraph(20, 3);
                    Debug.Log("DataExist default: " + _dataInstance.NumberOfNodes + " " +
                              _dataInstance.NumberOfEdgesPerNode);
                }

                if (_dataInstance.ChosenAlgorithm != null)
                    currentAlgorithm = (DataInstance.Algorithm)_dataInstance.ChosenAlgorithm;
            }
            else
            {
                GenerateGraph(20, 3);
                Debug.Log("DataNotExist default: 20 3");
            }
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("Main Menu");
        }

        public void GenerateGraph(int size, int edgesPerNode)
        {
            NumberOfNodes = size;
            NumberOfEdgesPerNode = edgesPerNode;

            nodes = new List<Node>(NumberOfNodes);

            // create specified number of nodes
            for (var i = 0; i < NumberOfNodes; i++)
            {
                var spawnPos = RandPos();
                nodes.Add(Instantiate(nodeObject, spawnPos, Quaternion.identity).GetComponent<Node>());
                nodes[i].SetNumberOfConnectedNodes(edgesPerNode);
                nodes[i].position = spawnPos;
                nodes[i].NodeID = i;
            }

            Shuffle(nodes); // shuffle and connect nodes to create spanning tree
            for (var i = 1; i < NumberOfNodes; i++) CreateEdge(nodes[i], nodes[i - 1]);

            Shuffle(nodes);
            for (var i = 0; i < NumberOfNodes; i++)
            for (var j = 0; j < NumberOfNodes; j++)
            {
                if (i == j) break;
                if (nodes[i].connectedNodes.Count >= NumberOfEdgesPerNode) break;
                if (nodes[j].connectedNodes.Count >= NumberOfEdgesPerNode) continue;
                if (nodes[i].connectedNodes.Contains(nodes[j])) continue;

                CreateEdge(nodes[i], nodes[j]);
            }
        }

        private Vector3 RandPos()
        {
            var bound = graphBoundingCubeLength * 0.5f;
            var spawnPos = new Vector3(
                Random.Range(-bound, bound),
                Random.Range(-bound, bound),
                Random.Range(-bound, bound)
            );
            return spawnPos;
        }

        /// <summary>
        ///     Shuffles the order of nodes in the graph
        /// </summary>
        private static void Shuffle<T>(IList<T> nodeList)
        {
            for (var i = 0; i < nodeList.Count - 1; ++i)
            {
                var r = Random.Range(i, nodeList.Count);
                (nodeList[i], nodeList[r]) = (nodeList[r], nodeList[i]);
            }
        }

        private void CreateEdge(Node a, Node b)
        {
            if (a == b) return;
            NumberOfEdges++;

            a.connectedNodes.Add(b);
            b.connectedNodes.Add(a);

            edges.Add(Instantiate(edgeObject, a.position, Quaternion.identity).GetComponent<Edge>());
            edges[NumberOfEdges - 1].ConnectEdge(a, b, edgeThicknessPercentage);
            edges[NumberOfEdges - 1].edgeID = NumberOfEdges - 1;
            a.connectedEdges[b] = edges[NumberOfEdges - 1];
            b.connectedEdges[a] = edges[NumberOfEdges - 1];
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