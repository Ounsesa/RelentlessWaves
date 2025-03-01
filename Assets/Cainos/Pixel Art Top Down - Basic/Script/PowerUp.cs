using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PowerUpEnum
{
    NewWeapon,
    Speed,
    ShootCadency,
    Damage,
    DamageMultiplier,
    Range,
    BulletSpeed,
    Size,
    Follower,
    Explodes,
    Piercing,
}

[System.Serializable]
public class PowerUpValues
{
    public PowerUpEnum powerUpValue = PowerUpEnum.NewWeapon;
    public float powerUpAmount = 10;
    public float powerUpDuration = 5;
    public int powerUpCostUpgrade = 100;
    public float powerUpUpgradeAmount = 1;
    public float powerUpUpgradeDuration = 1;
    public PowerUpValues() { }
    public PowerUpValues(PowerUpValues original)
    {
        powerUpValue = original.powerUpValue;
        powerUpAmount = original.powerUpAmount;
        powerUpDuration = original.powerUpDuration;
        powerUpCostUpgrade = original.powerUpCostUpgrade;
        powerUpUpgradeAmount = original.powerUpUpgradeAmount;
        powerUpUpgradeDuration = original.powerUpUpgradeDuration;
    }
}


public class PowerUp : MonoBehaviour
{
    public List<PowerUpValues> powerUpValuesList = new List<PowerUpValues>();

    public void Init(List<PowerUpValues> powerUpValues)
    {
       powerUpValuesList = powerUpValues;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PowerUpController.instance.RegisterPowerUpPicked(this);
            Destroy(gameObject);
        }

    }
}
