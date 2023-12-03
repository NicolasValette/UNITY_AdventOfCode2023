using AdventOfCode.Solver;
using AdventOfCode.Solver.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdventOfCode.Solver.Display
{
    public class Day1Display : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private SolverDay1 _solverDay1;
        [SerializeField]
        private Day2.SolverDay2 _solverDay2;
        // Start is called before the first frame update
        void Start()
        {
            _text.text = "Solution Day 1 : " + _solverDay1.GetSolution() +
                "\nSolution Day 2 : " + _solverDay2.GetSolution();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}