using System.Collections.Generic;
using System.Linq;
using Graph;
using Unity.VisualScripting;
using UnityEngine;
using Edge = Graph.Edge;

namespace Pathfinding
{
    public class AStarAlgo : MonoBehaviour
    {
        public List<Node> createdNodes;
        public List<Edge> openEdges = new();
        private readonly float _searchInterval = 0.5f;
        private bool _bGotPath;
        private bool _done;

        private Node _goalNode;
        private GraphLogic _graph;
        private Node _lastPos;

        private int _numberOfNodes;

        private float _searchTimer;
        private Node _startNode;

        private void Awake()
        {
            _graph = FindObjectOfType<GraphLogic>();
        }

        private void Start()
        {
            if (_graph.currentAlgorithm == DataInstance.Algorithm.AStar)
            {
                BeginSearch();
            }
            else
            {
                _done = true;
                _bGotPath = true;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
            // if (Input.GetKeyDown(KeyCode.C) && !_done) Search(_lastPos);
            // if (Input.GetKeyDown(KeyCode.M)) GetPath();

            if (!_done && _searchTimer < _searchInterval)
            {
                _searchTimer += Time.deltaTime;
            }
            else if (!_done)
            {
                _searchTimer = 0f;
                Search(_lastPos);
            }
            else if (_done && _searchTimer < _searchInterval)
            {
                _searchTimer += Time.deltaTime;
            }

            if (_done && !_bGotPath && _searchTimer >= _searchInterval)
            {
                GetPath();
                _bGotPath = true;
            }
        }

        // Start is called before the first frame update
        private void BeginSearch()
        {
            createdNodes = _graph.nodes;
            _numberOfNodes = _graph.nodes.Count;

            Debug.Log("Antall nodes: " + _numberOfNodes);
            Debug.Log("BEFORE");
            _startNode = _graph.nodes[0];
            _goalNode = _graph.nodes[_numberOfNodes - 1];
            Debug.Log("ree");

            _lastPos = _startNode;
            _startNode.SetColor(Color.magenta);
            _goalNode.SetColor(Color.red);
        }

        private void Search(Node thisNode)
        {
            if (_done) return;

            if (thisNode == _goalNode)
            {
                _done = true;
                return;
            }

            foreach (var nextNode in thisNode.connectedNodes)
            {
                var G = GraphLogic.Distance(thisNode, nextNode) + thisNode.timeToFinish;
                var H = GraphLogic.Distance(nextNode, _goalNode);
                var F = G + H;

                var tempEdge = thisNode.GetEdge(nextNode);
                if (tempEdge)
                    Debug.Log("got edge " + tempEdge.edgeID);
                else
                    Debug.Log("did not get edge: ");
                tempEdge.UpdateEdge(G, H, F);

                if (tempEdge.bOpen)
                {
                    tempEdge.parent = thisNode;
                    openEdges.Add(tempEdge);
                }
            }

            openEdges = openEdges.OrderBy(p => p.F).ThenBy(n => n.H).ToList();
            var pm = openEdges.ElementAt(0);
            pm.bOpen = false;
            thisNode = pm.parent;

            openEdges.RemoveAt(0);

            foreach (var e in openEdges) e.SetColor(Color.blue);

            pm.SetColor(Color.red);

            if (!pm.connectedNodes.Contains(thisNode))
                Debug.LogError("Edge-node mismatch! " + thisNode.NodeID + " : " + pm.edgeID);
            _lastPos = pm.connectedNodes.Find(e => e != thisNode);
            _lastPos.parent = thisNode;
        }

        private void GetPath()
        {
            //RemoveAllMarkers();
            var begin = _lastPos;

            while (_startNode != begin)
            {
                Debug.Log("NodeNr: " + begin.NodeID);
                var tempEdge = begin.GetEdge(begin.parent);
                Debug.Log("Found Edge: " + tempEdge.edgeID);
                tempEdge.SetColor(Color.green);
                begin = begin.parent;

                if (begin.IsUnityNull()) break;
            }
        }
    }
}