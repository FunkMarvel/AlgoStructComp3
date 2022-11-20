using System.Collections.Generic;
using System.Linq;
using Graph;
using Unity.VisualScripting;
using UnityEngine;
using Edge = Graph.Edge;

namespace Pathfinding
{
    public class Dijkstra : MonoBehaviour
    {
        public List<Node> createdNodes;
        public List<Edge> openEdges = new();
        private readonly float _searchInterval = 0.5f;

        private float _searchTimer;
        private bool _bGotPath;
        private bool _done;

        private Node _goalNode;
        private GraphLogic _graph;
        private Node _lastPos;

        private int _numberOfNodes;
        private Node _startNode;

        private void Awake()
        {
            _graph = FindObjectOfType<GraphLogic>();
        }

        private void Start()
        {
            if (_graph.currentAlgorithm == DataInstance.Algorithm.Dijkstra)
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
            if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
            if (Input.GetKeyDown(KeyCode.C) && !_done) Search(_lastPos);
            if (Input.GetKeyDown(KeyCode.M)) GetPath();

            if (!_done && _searchTimer < _searchInterval)
            {
                _searchTimer += Time.deltaTime;
            }
            else if (!_done)
            {
                _searchTimer = 0f;
                Search(_lastPos);
            }

            if (_done && !_bGotPath)
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
            if (thisNode == _goalNode)
            {
                _done = true;
                return;
            }

            foreach (var NextNode in thisNode.connectedNodes)
            {
                var G = thisNode.timeToFinish;
                var H = GraphLogic.Distance(thisNode, NextNode);
                var F = G + H;

                var TempEdge = thisNode.GetEdge(NextNode);
                TempEdge.UpdateEdge(G, H, F);

                if (TempEdge.bOpen)
                {
                    TempEdge.parent = thisNode;
                    openEdges.Add(TempEdge);
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
                var TempEdge = begin.GetEdge(begin.parent);
                Debug.Log("Found Edge: " + TempEdge.edgeID);
                TempEdge.SetColor(Color.green);
                begin = begin.parent;

                if (begin.IsUnityNull()) break;
            }
        }
    }
}