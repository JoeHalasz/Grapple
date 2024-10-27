using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;


    int score = 0;
    int highScore = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!File.Exists("PlayerData/FlappyBirdHighscore.data"))
        {
            highScore = 0;
        }
        else
        {
            StreamReader reader = new StreamReader("PlayerData/FlappyBirdHighscore.data");
            highScore = int.Parse(reader.ReadToEnd());
            reader.Close();
        }
       

        scoreText.text = "SCORE:  " + score.ToString();
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = "SCORE:  " + score.ToString();

        if (highScore < score)
        {
            StreamWriter writer = new StreamWriter("PlayerData/FlappyBirdHighscore.data", false);
            writer.WriteLine(score.ToString());
            writer.Close();
        }
    }

   
}
