using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    [RequireComponent(typeof(LineRenderer))]
    public class Edge : MonoBehaviour
    {
        public int EdgeID = -1;
        public List<Node> connectedNodes;
        public bool bOpen = true;
        public Node parent;

        public float G;
        public float H;
        public float F;

        private LineRenderer _line;

        private Transform _objectTransform;

        public Edge()
        {
            connectedNodes = new List<Node>(2);
        }

        public float timeToFinish { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            _objectTransform = GetComponent<Transform>();
            _objectTransform.LookAt(Camera.main.transform);
        }

        public void UpdateEdge(float g, float h, float f)
        {
            G = g;
            H = h;
            F = f;

            var values = GetComponentsInChildren<TextMesh>();
            values[0].text = "G: " + G.ToString("0.00");
            values[1].text = "H: " + H.ToString("0.00");
            values[2].text = "F: " + F.ToString("0.00");
        }


        public void ConnectEdge(Node a, Node b, float edgeThicknessPercentage)
        {
            connectedNodes.Add(a);
            connectedNodes.Add(b);

            _line = GetComponent<LineRenderer>();

            _line.material = new Material(Shader.Find("Sprites/Default"));
            _line.widthMultiplier = edgeThicknessPercentage * 0.01f;
            _line.positionCount = 2;

            var points = new Vector3[2];
            points[0] = a.position;
            points[1] = b.position;

            _line.SetPositions(points);
            timeToFinish = (a.position - b.position).magnitude;
        }

        public void SetColor(Color NewColor)
        {
            _line.startColor = NewColor;
            _line.endColor = NewColor;
        }

        public static bool operator ==(Edge someEdge, Edge otherEdge)
        {
            return otherEdge != null && someEdge != null && someEdge.EdgeID == otherEdge.EdgeID;
        }

        public static bool operator !=(Edge someEdge, Edge otherEdge)
        {
            return !(someEdge == otherEdge);
        }
    }
}