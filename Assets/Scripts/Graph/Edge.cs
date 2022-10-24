using System.Collections;
using System.Collections.Generic;
using Graph;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(LineRenderer))]
public class Edge : MonoBehaviour
{
    public int EdgeID { get; set; } = 0;
    public Vector3 position { get; set; } = Vector3.zero;
    public int timeToFinish { get; set; } = 0;
    public List<Node> connectedNodes;

    private Transform _objectTransform;

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

    public void ConnectEdge(Node a, Node b)
    {
        connectedNodes.Add(a);
        connectedNodes.Add(b);
        
        _line = GetComponent<LineRenderer>();
        
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.widthMultiplier = 0.05f;
        _line.positionCount = 2;
        
        var points = new Vector3[2];
        points[0] = a.position;
        points[1] = b.position;
        
        _line.SetPositions(points);
    }
}
