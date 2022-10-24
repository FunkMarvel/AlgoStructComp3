using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Graph
{
    public class GraphLogic : MonoBehaviour
    {
        public GameObject NodeObject;
        public List<Node> Nodes;

        public int numberOfNodes { get; private set; }
        public int numberOfEdges { get; private set; }
        public int numberOfEdgesPerNode { get; private set; }

        public float graphBoundingCubeLength;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void Awake()
        {
            GenerateGraph(5,2);
        }

        public void GenerateGraph(int size, int edgesPerNode)
        {
            numberOfNodes = size;
            numberOfEdgesPerNode = edgesPerNode;

            Nodes = new List<Node>(numberOfNodes);

            for (int i = 0; i < numberOfNodes; i++)
            {
                Vector3 spawnPos = RandPos();
                Nodes.Add(Instantiate(NodeObject, spawnPos, Quaternion.identity).GetComponent<Node>());
                Nodes[i].SetNumberOfConnectedNodes(edgesPerNode);
                Nodes[i].position = spawnPos;
                Nodes[i].timeToFinish = 1;
                Nodes[i].NodeID = i;
            }
            
            Shuffle(Nodes);
            for (int i = 1; i < numberOfNodes; i++)
            {
                CreateEdge(Nodes[i], Nodes[i-1]);
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
        /// Shuffles the element order of the specified list.
        /// </summary>
        private static void Shuffle<T>(IList<T> nodeList) {
            for (var i = 0; i < nodeList.Count - 1; ++i) {
                var r = UnityEngine.Random.Range(i, nodeList.Count);
                (nodeList[i], nodeList[r]) = (nodeList[r], nodeList[i]);
            }
        }

        private void CreateEdge(Node a, Node b)
        {
            a.connectedNodes.Add(b);
            b.connectedNodes.Add(a);
        }
    }
}
