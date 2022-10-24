using System.Collections;
using System.Collections.Generic;
using Graph;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public int EdgeID { get; set; } = 0;
    public Vector3 position { get; set; } = Vector3.zero;
    public int timeToFinish { get; set; } = 0;
    public List<Node> connectedNodes;

    private Transform _objectTransform;

    private MeshFilter _mesh;

    public Edge()
    {
        connectedNodes = new List<Node>(2);
    }
    // Start is called before the first frame update
    void Start()
    {
        _objectTransform = GetComponent<Transform>();
        _mesh = GetComponent<MeshFilter>();
    }

    public void ConnectEdge(Node a, Node b)
    {
        
    }
}
