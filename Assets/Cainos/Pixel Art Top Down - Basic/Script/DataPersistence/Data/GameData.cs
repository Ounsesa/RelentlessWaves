using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    #region Variables
    public int playerHealth = 20;
    public int waveNumber = 1;
    public int bonusCost = 2000;
    public int numberOfPowerUpValues = 1;
    public int score = 0;
    public List<PowerUpValues> powerUpValuesList = new List<PowerUpValues>();
    #endregion

    public GameData()
    {
        playerHealth = 20;
        waveNumber = 1;
        bonusCost = 2000;
        numberOfPowerUpValues = 1;
        score = 0;
        powerUpValuesList = new List<PowerUpValues>();
    }
}
