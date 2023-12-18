using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdventOfCode.Datas
{
    public class UndirectedGraph<T>
    {

        private long _numberOfNode;
        private T _actualNode;
        private T _startingNode;
        private List<T> _finalNode;
        public long NumberOfMove = 0;
        public T Actual { get => _actualNode; }
        public T Starting { get => _startingNode; }
        public List<T> Final { get => _finalNode; }
        private Dictionary<string, T> _nodes;
        private Dictionary<string, LinkedList<T>> _edges;
        public List<string> NodesLabel { get => _edges.Keys.ToList(); }
        public Dictionary<string, LinkedList<T>> Edges { get => _edges; }
        public Dictionary<string, T> Nodes { get => _nodes; }
        public long Count { get => _numberOfNode; }

        public UndirectedGraph()
        {
            _edges = new Dictionary<string, LinkedList<T>>();
            _nodes = new Dictionary<string, T>();
            NumberOfMove = 0;
        }
        public UndirectedGraph( UndirectedGraph<T> copy)
        {
            _edges = new Dictionary<string, LinkedList<T>>(copy.Edges);
            _nodes = new Dictionary<string, T>(copy.Nodes);
            _numberOfNode = copy._numberOfNode;
        }

        public void RemoveNode(T nodeToRemove)
        {
            List<T> neighbours = _edges[nodeToRemove.ToString()].ToList();
            for (int i=0;i<neighbours.Count;i++)
            {
                T neighbour = neighbours[i];
                _edges[neighbour.ToString()].Remove(nodeToRemove);
            }
            _nodes.Remove(nodeToRemove.ToString());
            _edges.Remove(nodeToRemove.ToString());
            _numberOfNode--;
        }
      
        public T GetNode(string node)
        {
            return _nodes[node];
        }
        public List<T> GetNeighbours(T node)
        {
            return _edges[node.ToString()].ToList();
        }
        public void AddEdge(T node1, T node2, bool verbose = false)
        {
            if (!_nodes.ContainsKey(node1.ToString()))
            {
                _nodes.Add(node1.ToString(), node1);
            }
            if (!_nodes.ContainsKey(node2.ToString()))
            {
                _nodes.Add(node2.ToString(), node2);
            }

            if (!_edges.ContainsKey(node1.ToString()))
            {
                _edges[node1.ToString()] = new LinkedList<T>();
                _numberOfNode++;
            }
            if (verbose) Debug.Log($"node : {node1.ToString()}, add child : {node2.ToString()}");
            _edges[node1.ToString()].AddLast(node2);

            if (!_edges.ContainsKey(node2.ToString()))
            {
                _edges[node2.ToString()] = new LinkedList<T>();
                _numberOfNode++;
            }
            if (verbose) Debug.Log($"node : {node2.ToString()}, add child : {node1.ToString()}");
            _edges[node2.ToString()].AddLast(node1);

        }
        public void InitStartingNode(T startingNode)
        {
            _startingNode = startingNode;
        }
        public void InitGraph(T startingNode, List<T> finalNode)
        {
            _startingNode = startingNode;
            _actualNode = startingNode;
            _finalNode = finalNode;
            NumberOfMove = 0;
        }
        public bool IsFinish()
        {
            return (_actualNode.Equals(_finalNode));
        }
        public bool Move(string instrution)
        {
            for (int i = 0; i < instrution.Length; i++)
            {
                LinkedList<T> values = new LinkedList<T>();
                _edges.TryGetValue(_actualNode.ToString(), out values);
                if (instrution[i] == 'L')
                {
                    if (values != null)
                    {
                        _actualNode = values.First.Value;
                        NumberOfMove++;
                    }


                }
                else if (instrution[i] == 'R')
                {
                    if (values != null)
                    {
                        _actualNode = values.Last.Value;
                        NumberOfMove++;
                    }
                }
                if (_finalNode.Contains(_actualNode))
                {
                    return true;

                }
            }
            return false;
        }

        public void DepthSearchFromStart(T start)
        {
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            List<string> keys = new List<string>(_edges.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                visited.Add(keys[i].ToString(), false);
            }
            DepthSearch(start, visited);

        }
        private void DepthSearch(T current, Dictionary<string, bool> visited)
        {
            visited[current.ToString()] = true;
            Debug.Log(" " + current.ToString());
            LinkedList<T> values = new LinkedList<T>();
            _edges.TryGetValue(current.ToString(), out values);
            foreach (T value in values)
            {
                if (visited[value.ToString()] == false)
                {
                    DepthSearch(value, visited);
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===Graph===");
            sb.AppendLine($"Starting Node : {Starting}");
            StringBuilder sbfinal = new StringBuilder();
            sbfinal.Append($"Final node : {Final[0].ToString()}");
            for (int i = 1; i < Final.Count; i++)
            {
                sbfinal.Append($", {Final[i].ToString()}");
            }
            sbfinal.Append(".");
            sb.AppendLine(sbfinal.ToString());
            sb.AppendLine();
            foreach (KeyValuePair<string, LinkedList<T>> node in _edges)
            {
                sb.Append($"{node.Key} = ({node.Value.First.Value}, {node.Value.First.Next.Value})\n");
            }
            sb.AppendLine("===========");
            return sb.ToString();
        }


    }
}

