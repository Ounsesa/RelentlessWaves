using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the Text component displaying the score
    private int score = 0;   // Initial score

    void Start()
    {
        UpdateScoreText();  // Update the score display at the start
    }

    public void AddScore(int amount)
    {
        score += amount;    // Add to the score
        UpdateScoreText();  // Update the displayed score
    }

    void UpdateScoreText()
    {
        // Convert the score to a string, padded with leading zeros, with a total length of 11
        scoreText.text = score.ToString().PadLeft(11, '0');
    }
}
