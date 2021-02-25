using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myScoreScript : MonoBehaviour
{
    public Text myScoreText, myHighscoreText;
    // Start is called before the first frame update
    void Start()
    {
        myScoreText.text = "Your Score: " + PlayerPrefs.GetInt("player_score").ToString();
        myHighscoreText.text = "Your Highscore: " + PlayerPrefs.GetInt("player_highscore").ToString();
    }
}
