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

  IEnumerator coroutine;

    private void Start()
    {
        ReCalcMaxCrafts();
    gameOverPanel.SetActive(false);

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
    gameOverPanel.SetActive(true);
    gameOverText.text = daysSinceLastIncident + " Days Since Last Incident";


    coroutine = WaitForReplay(4.0f);
    StartCoroutine(coroutine);

  }

  private IEnumerator WaitForReplay(float waitTime)
  {
    yield return new WaitForSeconds(waitTime);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

}
