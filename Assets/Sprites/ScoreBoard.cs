using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static TextMesh scoreText;

    public void SetScore(int score)
    {

        scoreText.text = score.ToString();
    }

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
