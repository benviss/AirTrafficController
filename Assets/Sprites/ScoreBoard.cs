using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private TextMesh _scoreText;

    void Awake()
    {
        _scoreText = GetComponentInChildren<TextMesh>();
    }

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}
