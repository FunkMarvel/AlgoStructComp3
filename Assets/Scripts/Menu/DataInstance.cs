using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataInstance : MonoBehaviour
{
    public static DataInstance Instance { get; private set; }
    public int? NumberOfNodes = null;
    public int? NumberOfEdgesPerNode = null;
    
    public enum Algorithm
    {
        Dijkstra,
        AStar,
        TSP
    }

    public Algorithm? ChosenAlgorithm = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
