using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Graph
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Node : MonoBehaviour
    {
        public int NodeID = -1;
        public Vector3 position { get; set; } = Vector3.zero;
        public float timeToFinish { get; set; } = 0f;
        public List<Node> connectedNodes;
        public Dictionary<Node, Edge> connectedEdges;

        private Transform _objectTransform;

        private MeshFilter _mesh;
        private MaterialPropertyBlock _materialProperies;
        public Renderer renderer { get; set; }
        public NodeType type { get; set; } = NodeType.Blue;

        public Node parent  = null;

        public enum NodeType
        {
            Orange,
            Purple,
            Beige,
            Green,
            Cyan,
            Yellow,
            Brown,
            Blue
        }

        private static Dictionary<NodeType, float> _timesTofinish = new Dictionary<NodeType, float>()
        {
            { NodeType.Orange, 26f },
            { NodeType.Purple, 22f },
            { NodeType.Beige, 18f },
            { NodeType.Green, 15f },
            { NodeType.Cyan, 12f },
            { NodeType.Yellow, 10f },
            { NodeType.Brown , 8f },
            { NodeType.Blue, 5f }
        };
        
        public static Dictionary<NodeType, Color> colors = new Dictionary<NodeType, Color>()
        {
            { NodeType.Orange, new Color(237f/255f, 125f/255f, 49f/255f) },
            { NodeType.Purple, new Color(112f/255f, 48f/255f, 160f/255f) },
            { NodeType.Beige, new Color(255f/255f, 230f/255f, 153f/255f) },
            { NodeType.Green, new Color(112f/255f, 173f/255f, 71f/255f) },
            { NodeType.Cyan, new Color(0f/255f, 176f/255f, 240f/255f) },
            { NodeType.Yellow, new Color(255f/255f, 192f/255f, 0f/255f) },
            { NodeType.Brown , new Color(132f/255f, 60f/255f, 12f/255f) },
            { NodeType.Blue, new Color(68f/255f, 114f/255f, 196f/255f) }
        };

        public Node()
        {
            connectedNodes = new List<Node>();
            connectedEdges = new Dictionary<Node, Edge>();
        }
        // Start is called before the first frame update
        void Start()
        {
            _objectTransform = GetComponent<Transform>();
            _mesh = GetComponent<MeshFilter>();
        }

        
        private void Awake()
        {
            var index = Random.Range(0, 8);
            type = (NodeType)index;

            timeToFinish = _timesTofinish[type];
            
            _materialProperies = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            renderer.GetPropertyBlock(_materialProperies);
            _materialProperies.SetColor("_Color", colors[type]);

            renderer.SetPropertyBlock(_materialProperies);
        }

        public void SetNumberOfConnectedNodes(int numberOfConnectedNodes)
        {
            connectedNodes.Capacity = numberOfConnectedNodes;
        }

        public void SetColor(Color NewColor)
        {
            _materialProperies = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            renderer.GetPropertyBlock(_materialProperies);
            _materialProperies.SetColor("_Color", NewColor);

            renderer.SetPropertyBlock(_materialProperies);
        }

        public void ResetColor()
        {
            _materialProperies = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            renderer.GetPropertyBlock(_materialProperies);
            _materialProperies.SetColor("_Color", colors[type]);

            renderer.SetPropertyBlock(_materialProperies);
        }

        public Edge GetEdge(Node targetNode)
        {
            if (connectedEdges.ContainsKey(targetNode)) return connectedEdges[targetNode];
            return null;
        }
        
        public static bool operator==(Node someNode, Node otherNode)
        {
            return someNode.NodeID == otherNode.NodeID;
        }
        public static bool operator !=(Node someNode, Node otherNode)
        {
            return !(someNode == otherNode);
        }
    }
}
