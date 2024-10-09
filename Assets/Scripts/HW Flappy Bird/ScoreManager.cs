using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    int score = 0;
    int highScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "SCORE:  " + score.ToString();
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
