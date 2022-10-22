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
                Vector3 spawnPos = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
                Nodes.Add( Instantiate(NodeObject, spawnPos, Quaternion.identity).GetComponent<Node>());
                Nodes[i].position = spawnPos;
                Nodes[i].timeToFinish = 1;
            }
        }
    }
}
