using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int PlayerHealth = 0;
    public int WaveNumber = 1;
    public int BonusCost = 2000;
    public int NumberOfPowerUpValues = 1;
    public List<PowerUpValues> powerUpValuesList = new List<PowerUpValues>();

    public GameData()
    {
        PlayerHealth = 10;
        WaveNumber = 1;
        BonusCost = 2000;
        NumberOfPowerUpValues = 1;
        powerUpValuesList = new List<PowerUpValues>();
    }
}
