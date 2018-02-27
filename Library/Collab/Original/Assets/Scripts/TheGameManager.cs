using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TheGameManager : SingletonPersistant<TheGameManager> {


    public int maxCraftsOnScreen = 4;
    [HideInInspector]
    public int daysSinceLastIncident;
    public int planesCollected;
  public GameObject gameOverPanel;
  public Text gameOverText;
  public bool gameOver = false;

  IEnumerator coroutine;

    private void Start()
    {
    gameOver = false;
        ReCalcMaxCrafts();
    gameOverPanel.SetActive(false);

  }

  private void Update()
  {
    if (Input.anyKeyDown && gameOver) {
      gameOverPanel.SetActive(false);
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  public void IncrementDay()
    {
        daysSinceLastIncident++;
        ReCalcMaxCrafts();
    }

    public void ReCalcMaxCrafts()
    {
        maxCraftsOnScreen = (int)(Mathf.Pow((float)daysSinceLastIncident + 1, 1.3f) + (daysSinceLastIncident + 1) * 2.5);
    }

  public void GameOver()
  {
    gameOver = true;
    gameOverPanel.SetActive(true);
    string finalText = daysSinceLastIncident + " Days Since Last Incident";
    gameOverText.text = finalText;
  }
}
