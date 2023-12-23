using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventOfCode.Solver.Day22
{
  
    public class BrickGraph<T>
    {

        private long _numberOfNode = 0;


        private Dictionary<T, List<T>> _supporting;
        private Dictionary<T, List<T>> _supportedBy;
        public Dictionary<T, List<T>> Supporting => _supporting;
        public Dictionary<T, List<T>> SupportedBy => _supportedBy;
        public long Count { get => _numberOfNode; }

        public BrickGraph()
        {
            _supporting = new Dictionary<T, List<T>>();
            _supportedBy = new Dictionary<T, List<T>>();
            
        }

        public List<T> GetSupporting(T key)
        {
            if (_supporting.ContainsKey(key))
            {
                return _supporting[key];
            }
            else
            {
                return new List<T>();
            }
        }
        public List<T> GetSupported(T key)
        {
            if (_supportedBy.ContainsKey(key))
            {
                return _supportedBy[key];
            }
            else
            {
                return new List<T>();
            }
        }
        public void AddEdge(T from, T to, bool verbose = false)
        {
            if (!_supporting.ContainsKey(from))
            {
                _supporting[from] = new List<T>();
            }
            if (!_supportedBy.ContainsKey(to))
            {
                _supportedBy[to] = new List<T>();
            }
            _supportedBy[to].Add(from);
            _supporting[from].Add(to);
            if (verbose) Debug.Log($"node : {from.ToString()}, add child : {to.ToString()}");


        }
        public void RemoveEdge(T from, T to)
        {
            _supporting[to].Remove(from);
            _supportedBy[from].Remove(to);
        }



        //public void DepthSearchFromStart(T start)
        //{
        //    Dictionary<T, bool> visited = new Dictionary<T, bool>();
        //    List<T> keys = new List<T>(_edges.Keys);
        //    for (int i = 0; i < keys.Count; i++)
        //    {
        //        visited.Add(keys[i], false);
        //    }
        //    DepthSearch(start, visited);

        //}
        //private void DepthSearch(T current, Dictionary<T, bool> visited)
        //{
        //    visited[current] = true;
        //    Debug.Log(" " + current.ToString());
        //    LinkedList<T> values = new LinkedList<T>();
        //    _edges.TryGetValue(current, out values);
        //    foreach (T value in values)
        //    {
        //        if (visited[value] == false)
        //        {
        //            DepthSearch(value, visited);
        //        }
        //    }
        //}
        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("===Graph===");
        //    sb.AppendLine($"Starting Node : {Starting}");
        //    StringBuilder sbfinal = new StringBuilder();
        //    sbfinal.Append($"Final node : {Final[0].ToString()}");
        //    for (int i = 1; i < Final.Count; i++)
        //    {
        //        sbfinal.Append($", {Final[i].ToString()}");
        //    }
        //    sbfinal.Append(".");
        //    sb.AppendLine(sbfinal.ToString());
        //    sb.AppendLine();
        //    foreach (KeyValuePair<T, LinkedList<T>> node in _edges)
        //    {
        //        sb.Append($"{node.Key} = ({node.Value.First.Value}, {node.Value.First.Next.Value})\n");
        //    }
        //    sb.AppendLine("===========");
        //    return sb.ToString();
        //}

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
