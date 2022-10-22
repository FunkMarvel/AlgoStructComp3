using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Node : MonoBehaviour
    {
        public int NodeID { get; set; } = 0;
        public Vector3 position { get; set; } = Vector3.zero;
        public int timeToFinish { get; set; } = 0;
        public List<Node> connectedNodes;

        private Transform _objectTransform;

        private MeshFilter _mesh;

        public Node()
        {
            connectedNodes = new List<Node>(1);
        }
        // Start is called before the first frame update
        void Start()
        {
            _objectTransform = GetComponent<Transform>();
            _mesh = GetComponent<MeshFilter>();
        }
        
    }
}