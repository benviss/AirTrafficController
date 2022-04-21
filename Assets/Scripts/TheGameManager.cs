using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheGameManager : Singleton<TheGameManager>
{
    public int maxCraftsOnScreen = 4;
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

    private ScoreBoard scoreBoard;
    private IEnumerator coroutine;

    public GameObject BlackoutWalls;

    private void Start()
    {
        Time.timeScale = 0;
        gameOver = false;
        ReCalcMaxCrafts();
        introPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        scoreBoard = FindObjectOfType<ScoreBoard>();
        scoreBoard.SetScore(daysSinceLastIncident);
        BlackoutWalls.SetActive(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (gameOver)
            {
                // ???
            }
            else if (Time.timeScale == 0)
            {
                StartGame();
            }
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        introPanel.SetActive(false);
    }

    public void IncrementDay()
    {
        daysSinceLastIncident++;

        if (!gameOver)
        {
            scoreBoard.SetScore(daysSinceLastIncident);
        }

        if (daysSinceLastIncident == 1) SpawnManager.Instance.AddSpawnable(firstNewCraft);
        if (daysSinceLastIncident == 3) SpawnManager.Instance.AddSpawnable(secondNewCraft);
        if (daysSinceLastIncident == 5) SpawnManager.Instance.AddSpawnable(thirdNewCraft);

        // Every two days do... something
        if (daysSinceLastIncident % 2 == 0)
        {
            if (SpawnManager.Instance.spawnWaitTime > 1)
            {
                SpawnManager.Instance.spawnWaitTime -= 1;
            }
        }
        else
        {
            // Every other day increase speed multipler
            SpawnManager.Instance.speedMultiplier += 1;
        }

        ReCalcMaxCrafts();
    }

    public void ReCalcMaxCrafts()
    {
        //maxCraftsOnScreen = (int)(Mathf.Pow((float)daysSinceLastIncident + 1, 1.3f) + (daysSinceLastIncident + 1) * 2.5);
        maxCraftsOnScreen += 1;
    }

    public void GameOver()
    {
        if (gameOver) return;

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

    public void ResetGame()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
