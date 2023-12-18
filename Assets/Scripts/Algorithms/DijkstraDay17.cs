using AdventOfCode.Datas;
using AdventOfCode.Solver.Day17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode.Algorithms
{
    public class DijkstraDay17
    {
        private UndirectedGraph<CityBlock> _graph;
        private CityBlock _startingBlock;
        private CityBlock _finishingBlock;
        private Dictionary<string, CityBlock> _previous;

        private Dictionary<string, int> _dist;
        public DijkstraDay17(UndirectedGraph<CityBlock> graph, CityBlock start, CityBlock finish)
        {
            _graph = graph;
            _startingBlock = start;
            _finishingBlock = finish;
        }

        private void initDijkstra()
        {
            _dist = new Dictionary<string, int>();
            _previous = new Dictionary<string, CityBlock> ();
            foreach (string key in _graph.NodesLabel)
            {
                _dist.Add(key, int.MaxValue);
            }
            _dist[_startingBlock.ToString()] = 0;
        }

        private CityBlock FindMinimalDistance(UndirectedGraph<CityBlock> subGraph)
        {
            int minDist = int.MaxValue;
            CityBlock sommet = null;
            foreach (var c in subGraph.NodesLabel)
            {
                if (_dist[c] < minDist)
                {
                    minDist = _dist[c];
                    sommet = subGraph.GetNode(c);
                }
            }
            return sommet;
        }
        
        private void UpdateDistances(CityBlock s1, CityBlock s2)
        {
            if (_dist[s2.ToString()] > (_dist[s1.ToString()] + s2.HeatLoss))
            {
                _dist[s2.ToString()] = _dist[s1.ToString()] + s2.HeatLoss;
                _previous.Add(s2.ToString(), s1);
            }
        }

        public Stack<CityBlock> Run()
        {
            initDijkstra();
            UndirectedGraph<CityBlock> Q = new UndirectedGraph<CityBlock>(_graph);
            while (Q.Count > 0)
            {
                CityBlock s1 = FindMinimalDistance(Q);
                Q.RemoveNode(s1);
                foreach (CityBlock s2 in _graph.GetNeighbours(s1))
                {
                    UpdateDistances(s1, s2);
                }
            }

            Stack<CityBlock> path = new Stack<CityBlock>();
            CityBlock s = _finishingBlock;
            while (!s.Equals(_startingBlock))
            {
                path.Push(s);
                s = _previous[s.ToString()];
            }
            path.Push(_startingBlock);
            return path;
        }
    }
}