using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Day1Display : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private SolverDay1 _solverDay1;
    // Start is called before the first frame update
    void Start()
    {
        _text.text = "Solution Day 1 : " + _solverDay1.Solution;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
