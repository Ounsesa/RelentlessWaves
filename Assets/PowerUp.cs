using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PowerUpValues
{
    Speed,
    ShootCadency,
    Damage,
    DamageMultiplier,
    Range,
    RangeMultiplier,
    BulletSpeed,
    BulletSpeedMultiplier,
    Size,
    Follower,
    FollowerSizeDetection,
    ExplodesOnHit,
    ExplosionArea,
    Piercing,
    AreaDamage,
    AreaSize,
}



public class PowerUp : MonoBehaviour
{
    PowerUpValues powerUpValue = PowerUpValues.Damage;
    float powerUpAmount = 10;
    float powerUpDuration = 5;


    private void Init(PowerUpValues powerUpValue)
    {
        //Leer de un fichero
        int EnumSize = Enum.GetNames(typeof(PowerUpValues)).Length;
        int RandomValue = UnityEngine.Random.Range(0, EnumSize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<TopDownCharacterController>().AddWeapon();
            Destroy(gameObject);
        }

    }
}
