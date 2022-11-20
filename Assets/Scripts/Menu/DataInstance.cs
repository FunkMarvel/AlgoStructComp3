using UnityEngine;

public class DataInstance : MonoBehaviour
{
    public enum Algorithm
    {
        Dijkstra,
        AStar,
        TSP
    }

    public Algorithm? ChosenAlgorithm = null;
    public int? NumberOfEdgesPerNode = null;
    public int? NumberOfNodes = null;
    public static DataInstance Instance { get; private set; }

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