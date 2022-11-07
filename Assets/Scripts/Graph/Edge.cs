using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Graph
{
    [RequireComponent(typeof(LineRenderer))]
    public class Edge : MonoBehaviour
    {
        public int EdgeID { get; set; } = 0;
        public int timeToFinish { get; set; } = 0;
        public List<Node> connectedNodes;
        public bool bOpen = true;

        private Transform _objectTransform;
        
        public float G = 0f;
        public float H = 0f;
        public float F = 0f;

        private LineRenderer _line;

        public Edge()
        {
            connectedNodes = new List<Node>(2);
        }
        // Start is called before the first frame update
        void Start()
        {
            _objectTransform = GetComponent<Transform>();
        }

        public void ConnectEdge(Node a, Node b, float edgeThicknessPercentage)
        {
            connectedNodes.Add(a);
            connectedNodes.Add(b);
        
            _line = GetComponent<LineRenderer>();
        
            _line.material = new Material(Shader.Find("Sprites/Default"));
            _line.widthMultiplier = edgeThicknessPercentage*0.01f;
            _line.positionCount = 2;
        
            var points = new Vector3[2];
            points[0] = a.position;
            points[1] = b.position;
        
            _line.SetPositions(points);
        }

        public void SetColor(Color NewColor)
        {
            _line.startColor = NewColor;
            _line.endColor = NewColor;
        }
        
        public static bool operator==(Edge someEdge, Edge otherEdge)
        {
            return otherEdge != null && someEdge != null && someEdge.EdgeID == otherEdge.EdgeID;
        }
        public static bool operator !=(Edge someEdge, Edge otherEdge)
        {
            return !(someEdge == otherEdge);
        }
    }
}
