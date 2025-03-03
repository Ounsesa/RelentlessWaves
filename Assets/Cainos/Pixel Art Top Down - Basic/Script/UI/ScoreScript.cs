using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TextMeshProUGUI m_scoreText;  // Reference to the Text component displaying the score
    private int m_score = 0;   // Initial score
    #endregion

    void Start()
    {
        UpdateScoreText();  // Update the score display at the start
    }

    public void AddScore(int amount)
    {
        m_score += amount;    // Add to the score
        UpdateScoreText();  // Update the displayed score
    }

    void UpdateScoreText()
    {
        // Convert the score to a string, padded with leading zeros, with a total length of 11
        m_scoreText.text = m_score.ToString().PadLeft(11, '0');
    }
}
