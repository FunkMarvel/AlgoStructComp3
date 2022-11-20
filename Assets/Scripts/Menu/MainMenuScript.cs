using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string graphLevel = "Graph Scene";

    public GameObject shortestPathScreen;
    public GameObject travlingSalesmanScreen;

    public TMP_InputField numNodesShort;
    public TMP_InputField numNodesTSP;
    public TMP_InputField numEdgesShort;

    public int numberNodes = 20;
    public int numberEdgesPerNode = 5;

    public enum MenuState
    {
        MainMenuState,
        ShortestPathState,
        TravelingSalesmanState
    }

    public MenuState currentState = MenuState.MainMenuState;

    private bool _bEnteringState = false;

    public void OnChangeNumberNodes()
    {
        int fieldValue = 0;
        int.TryParse(numNodesShort.text, out fieldValue);
        
        switch (currentState)
        {
            case MenuState.ShortestPathState:
                if (numNodesShort)
                {
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
                if (numNodesShort)
                {
                    if (fieldValue < 2)
                    {
                        numberNodes = 2;
                        numNodesTSP.text = "2";
                    }
                    else if (fieldValue > 25)
                    {
                        numberNodes = 25;
                        numNodesTSP.text = "25";
                    }
                    else
                    {
                        numberNodes = fieldValue;
                    }
                }

                break;
        }
    }

    public void OnChangeNumEdges()
    {
        int fieldValue = 0;
        int.TryParse(numEdgesShort.text, out fieldValue);

        switch (currentState)
        {
            case MenuState.ShortestPathState:
                if (numNodesShort)
                {
                    if (fieldValue < 2)
                    {
                        numberEdgesPerNode = 2;
                        numEdgesShort.text = "2";
                    }
                    else if (fieldValue > numberNodes-1)
                    {
                        numberEdgesPerNode = numberNodes-1;
                        numEdgesShort.text = "100";
                    }
                    else
                    {
                        numberNodes = fieldValue;
                    }
                }

                break;
            
            case MenuState.TravelingSalesmanState:
                if (numNodesShort)
                {
                    if (fieldValue < 2)
                    {
                        numberNodes = 2;
                        numNodesTSP.text = "2";
                    }
                    else if (fieldValue > 25)
                    {
                        numberNodes = 25;
                        numNodesTSP.text = "25";
                    }
                    else
                    {
                        numberNodes = fieldValue;
                    }
                }

                break;
        }
    }

    public void OnChangeNumEdgesPerNode()
    {
        if (currentState == MenuState.TravelingSalesmanState && numEdgesShort)
        {
            int fieldValue = 0;
            int.TryParse(numEdgesShort.text, out fieldValue);
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

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        if (shortestPathScreen) shortestPathScreen.SetActive(false);
        if (travlingSalesmanScreen) travlingSalesmanScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadGraph()
    {
        SceneManager.LoadScene(graphLevel);
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
            if (_bEnteringState)
            {
                travlingSalesmanScreen.SetActive(true);
            }
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