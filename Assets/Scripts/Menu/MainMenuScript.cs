using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenuScript : MonoBehaviour
    {
        public enum MenuState
        {
            MainMenuState,
            ShortestPathState,
            TravelingSalesmanState
        }

        public const string GraphLevel = "Graph Scene";

        public GameObject shortestPathScreen;
        public GameObject travlingSalesmanScreen;

        public TMP_InputField numNodesShort;
        public TMP_InputField numNodesTsp;
        public TMP_InputField numEdgesShort;

        public int numberNodes = 20;
        public int numberEdgesPerNode = 5;

        public MenuState currentState = MenuState.MainMenuState;

        private bool _bEnteringState;

        private DataInstance _dataInstance;
        private DataInstance.Algorithm _chosenAlgorithm;

        private void Awake()
        {
            _dataInstance = FindObjectOfType<DataInstance>();
            if (shortestPathScreen) shortestPathScreen.SetActive(false);
            if (travlingSalesmanScreen) travlingSalesmanScreen.SetActive(false);
        }

        public void OnChangeNumberNodes()
        {
            switch (currentState)
            {
                case MenuState.ShortestPathState:
                    if (numNodesShort)
                    {
                        var fieldValue = 0;
                        int.TryParse(numNodesShort.text, out fieldValue);
                        
                        if (fieldValue < 2)
                        {
                            numberNodes = 2;
                            numNodesShort.text = "2";
                        }
                        else if (fieldValue > 100)
                        {
                            numberNodes = 100;
                            numNodesShort.text = "100";
                        }
                        else
                        {
                            numberNodes = fieldValue;
                        }
                    }

                    break;

                case MenuState.TravelingSalesmanState:
                    if (numNodesTsp)
                    {
                        var fieldValue = 0;
                        int.TryParse(numNodesTsp.text, out fieldValue);
                        
                        if (fieldValue < 2)
                        {
                            numberNodes = 2;
                            numberEdgesPerNode = 1;
                            numNodesTsp.text = "2";
                        }
                        else if (fieldValue > 25)
                        {
                            numberNodes = 25;
                            numberEdgesPerNode = 24;
                            numNodesTsp.text = "25";
                        }
                        else
                        {
                            numberNodes = fieldValue;
                            numberEdgesPerNode = numberNodes - 1;
                        }
                    }

                    break;
            }
        }

        public void OnChangeNumEdgesPerNode()
        {
            var fieldValue = 0;
            int.TryParse(numNodesShort.text, out fieldValue);

            switch (currentState)
            {
                case MenuState.ShortestPathState:
                    if (numEdgesShort)
                    {
                        if (fieldValue < 2)
                        {
                            numberEdgesPerNode = 2;
                            numEdgesShort.text = "2";
                        }
                        else if (fieldValue > numberNodes - 1)
                        {
                            numberEdgesPerNode = numberNodes - 1;
                            numEdgesShort.text = "25";
                        }
                        else
                        {
                            numberEdgesPerNode = fieldValue;
                        }
                    }

                    break;

                case MenuState.TravelingSalesmanState:
                    if (numNodesShort)
                    {
                        if (fieldValue < 2)
                        {
                            numberNodes = 2;
                            numNodesTsp.text = "2";
                        }
                        else if (fieldValue > 25)
                        {
                            numberNodes = 25;
                            numNodesTsp.text = "25";
                        }
                        else
                        {
                            numberNodes = fieldValue;
                        }
                    }

                    break;
            }
        }

        public void ChangeState(MenuState newState)
        {
            if (currentState == newState)
            {
                _bEnteringState = false;
                return;
            }

            if (currentState != newState)
            {
                _bEnteringState = true;
                currentState = newState;
            }
        }

        public void LoadGraph()
        {
            if (_dataInstance)
            {
                _dataInstance.NumberOfNodes = numberNodes;
                if (numberEdgesPerNode > numberNodes - 1) numberEdgesPerNode = numberNodes;
                if (numberEdgesPerNode < 2) numberEdgesPerNode = 2;
                _dataInstance.NumberOfEdgesPerNode = numberEdgesPerNode;
                _dataInstance.ChosenAlgorithm = _chosenAlgorithm;
            }

            SceneManager.LoadScene(GraphLevel);
        }

        public void OnShortestPath()
        {
            switch (currentState)
            {
                case MenuState.MainMenuState:
                    ChangeState(MenuState.ShortestPathState);
                    if (_bEnteringState)
                    {
                        shortestPathScreen.SetActive(true);
                        _bEnteringState = false;
                    }

                    break;
                case MenuState.ShortestPathState:
                    break;
            }
        }

        public void OnTravelingSalesman()
        {
            if (currentState == MenuState.MainMenuState)
            {
                ChangeState(MenuState.TravelingSalesmanState);
                if (_bEnteringState) travlingSalesmanScreen.SetActive(true);
            }

            switch (currentState)
            {
                case MenuState.MainMenuState:
                    ChangeState(MenuState.TravelingSalesmanState);
                    if (_bEnteringState)
                    {
                        travlingSalesmanScreen.SetActive(true);
                        _bEnteringState = false;
                    }

                    break;
                case MenuState.TravelingSalesmanState:
                    break;
            }
        }

        public void OnDijkstra()
        {
            _chosenAlgorithm = DataInstance.Algorithm.Dijkstra;
            LoadGraph();
        }

        public void OnAStar()
        {
            _chosenAlgorithm = DataInstance.Algorithm.AStar;
            LoadGraph();
        }

        public void OnStartTsp()
        {
            _chosenAlgorithm = DataInstance.Algorithm.TSP;
            numberEdgesPerNode = numberNodes;
            LoadGraph();
        }

        public void OnBack()
        {
            switch (currentState)
            {
                case MenuState.ShortestPathState:
                    ChangeState(MenuState.MainMenuState);
                    shortestPathScreen.SetActive(false);
                    break;
                case MenuState.TravelingSalesmanState:
                    ChangeState(MenuState.MainMenuState);
                    travlingSalesmanScreen.SetActive(false);
                    break;
            }
        }

        public void OnExitGame()
        {
            if (currentState == MenuState.MainMenuState)
            {
                Application.Quit();
                Debug.Log("Quitting");
            }
        }
    }
}