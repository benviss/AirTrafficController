using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayViewModel : MonoBehaviour
{
    public ScoreBoard scoreBoard;
    public SpawnManager spawnManager;

    public void SetScore(int score)
    {
        scoreBoard.SetScore(score);
    }
}
