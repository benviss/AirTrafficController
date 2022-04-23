using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class App : Singleton<App>
{
    public readonly DynamicRefService DynamicRefs = new DynamicRefService();

    public event Action OnNewGame;

    public int maxCraftsOnScreen;
    [HideInInspector]
    public int daysSinceLastIncident;
    public int planesCollected;

    public GameObject gameOverPanel;
    public Button gameOverButton;
    public Text gameOverText;

    public GameObject introPanel;
    public Transform firstNewCraft;
    public Transform secondNewCraft;
    public Transform thirdNewCraft;
    public bool gameOver = false;
    public SceneController sceneController;
    public bool gameStarted;

    private readonly DynamicRef<GameplayViewModel> _gameplayViewModel = DynamicRef.SearchByType<GameplayViewModel>();

    private void Awake()
    {
        sceneController = GetComponent<SceneController>();
    }

    private void Start()
    {
        sceneController.OnSceneLoaded += HandleSceneLoaded;
        sceneController.LoadScene(GameConstants.GameplayScene, LoadSceneMode.Additive);
        sceneController.LoadScene("Level_1_PC", LoadSceneMode.Additive);
        Time.timeScale = 0;
        ResetLevel();
    }

    private void ResetLevel()
    {
        maxCraftsOnScreen = 4;
        introPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        daysSinceLastIncident = 0;
        planesCollected = 0;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == GameConstants.GameplayScene)
        {
            StartCoroutine(SetScore());
        }
    }

    private IEnumerator SetScore()
    {
        if (!_gameplayViewModel.HasValue)
            yield return _gameplayViewModel.Wait;

        _gameplayViewModel.Value.SetScore(daysSinceLastIncident);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public IEnumerator StartGameCoroutine()
    {
        gameOver = false;
        gameStarted = true;
        OnNewGame?.Invoke();
        introPanel.SetActive(false);

        if (!_gameplayViewModel.HasValue)
            yield return _gameplayViewModel.Wait;

        _gameplayViewModel.Value.SetScore(daysSinceLastIncident);
        _gameplayViewModel.Value.spawnManager.StartSpawner();
        Time.timeScale = 1;
    }

    public void IncrementDay()
    {
        daysSinceLastIncident++;

        if (!gameOver)
        {
            StartCoroutine(SetScore());
        }

        switch (daysSinceLastIncident)
        {
            case 1:
                _gameplayViewModel.Value.spawnManager.AddSpawnable(firstNewCraft);
                break;

            case 3:
                _gameplayViewModel.Value.spawnManager.AddSpawnable(secondNewCraft);
                break;

            case 5:
                _gameplayViewModel.Value.spawnManager.AddSpawnable(thirdNewCraft);
                break;
        }

        // Every two days increase spawn speed multipler
        if (daysSinceLastIncident % 2 == 1)
        {
            _gameplayViewModel.Value.spawnManager.speedMultiplier += 1;
        }

        ReCalcMaxCrafts();
    }

    public void IncrementHalfDay()
    {
        // every other night add a craft
        if (daysSinceLastIncident % 2 == 0)
        {
            ReCalcMaxCrafts();
        }
    }

    public void ReCalcMaxCrafts()
    {
        //maxCraftsOnScreen = (int)(Mathf.Pow((float)daysSinceLastIncident + 1, 1.3f) + (daysSinceLastIncident + 1) * 2.5);
        maxCraftsOnScreen += 1;
    }

    public void GameOver()
    {
        if (gameOver) return;

        // Save off any data we want to
        // Award achievements

        gameOver = true;
        gameOverPanel.SetActive(true);
        string finalText = daysSinceLastIncident + " Days Since Last Incident";
        gameOverText.text = finalText;
        gameOverButton.gameObject.SetActive(true);
        //StartCoroutine(DisplayRestartButton(2));
    }

    IEnumerator DisplayRestartButton(float time)
    {
        gameOverButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameOverButton.gameObject.SetActive(true);
    }

    public void ReplayLevel()
    {
        // Show advertisment


        ResetLevel();
    }
}
