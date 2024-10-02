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
    AreaDamage,
}

[System.Serializable]
public class PowerUpValues
{
    public PowerUpEnum powerUpValue = PowerUpEnum.NewWeapon;
    public float powerUpAmount = 10;
    public float powerUpDuration = 5;
    public PowerUpValues() { }
    public PowerUpValues(PowerUpValues original)
    {
        powerUpValue = original.powerUpValue;
        powerUpAmount = original.powerUpAmount;
        powerUpDuration = original.powerUpDuration;
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
            PowerUpController.m_instance.RegisterPowerUpPicked(this);
            Destroy(gameObject);
        }

    }
}
