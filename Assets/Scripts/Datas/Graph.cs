using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Datas
{
    public class Graph<T>
    {
        private long _numberOfNode;
        private T _actualNode;
        private T _startingNode;
        private List<T> _finalNode;
        public long NumberOfMove = 0;
        public T Actual { get => _actualNode; }
        public T Starting { get => _startingNode; }
        public List<T> Final { get => _finalNode; }
        private Dictionary<T, LinkedList<T>> _edges;
        public long Count { get => _numberOfNode; }

        public Graph()
        {
            _edges = new Dictionary<T, LinkedList<T>>();
            NumberOfMove = 0;
        }

        public void AddEdge(T from, T to, bool verbose = false)
        {
            if (!_edges.ContainsKey(from))
            {
                _edges[from] = new LinkedList<T>();
                _numberOfNode++;
            }
            if (verbose) Debug.Log($"node : {from.ToString()}, add child : {to.ToString()}");
            _edges[from].AddLast(to);

        }
        public void InitStartingNode (T startingNode)
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
                _edges.TryGetValue(_actualNode, out values);
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

        public void DepthSearchFromStart (T start)
        {
            Dictionary<T, bool> visited = new Dictionary<T, bool>();
            List<T> keys = new List<T>(_edges.Keys);
            for (int i=0; i<keys.Count; i++)
            {
                visited.Add(keys[i], false);
            }
            DepthSearch(start, visited);

        }
        private void DepthSearch (T current, Dictionary<T, bool> visited)
        {
            visited[current] = true;
            Debug.Log(" " + current.ToString());
            LinkedList<T> values = new LinkedList<T>();
            _edges.TryGetValue(current, out values);
            foreach (T value in values)
            {
                if (visited[value] == false)
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
            for (int i=1; i< Final.Count; i++)
            {
                sbfinal.Append($", {Final[i].ToString()}");
            }
            sbfinal.Append(".");
            sb.AppendLine(sbfinal.ToString());
            sb.AppendLine();
            foreach (KeyValuePair<T, LinkedList<T>> node in _edges)
            {
                sb.Append($"{node.Key} = ({node.Value.First.Value}, {node.Value.First.Next.Value})\n");
            }
            sb.AppendLine("===========");
            return sb.ToString();
        }

        /*
       public void DepthFirstSearch(int v)

       {

           // Mark all the vertices as not visited

           bool[] visit = new bool[_v];

           for (int i = 0; i < _v; i++)

               visit[i] = false;

           // Call the recursive function to print DFS traversal

           DFStil(v, visit);

       }

       private void DFStil(int v, bool[] visit)

       {

           // Mark the current node as visited and display it

           visit[v] = true;

           Console.Write(v + " ");

           // Recur for all the vertices adjacent to this vertex

           LinkedList<int> list = _adj[v];

           foreach (var value in list)

           {

               if (!visit[value])

                   DFStil(value, visit);

           }

       }
       */
    }
}