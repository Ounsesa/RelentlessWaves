using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUpgrade : MonoBehaviour
{
    public GameObject PowerUpText;
    public GameObject AmountButton;
    public GameObject DurationButton;
    public GameObject CostText;

    public PowerUpEnum PowerUpType;

    public PowerUpValues PowerUpData;


    private void Awake()
    {
        AmountButton.GetComponent<Button>().onClick.AddListener(OnAmountButtonPressed);
        DurationButton.GetComponent<Button>().onClick.AddListener(OnDurationButtonPressed);
    }

    private void Start()
    {
        SetPowerUpType();
        PowerUpData = PowerUpController.m_instance.GetPowerUpData(PowerUpType);
        if(PowerUpData != null)
        {
            AmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpAmount.ToString("F2");
            DurationButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpDuration.ToString("F2");
            CostText.GetComponent<TextMeshProUGUI>().text = PowerUpData.powerUpCostUpgrade.ToString();
        }
    }

    private void SetPowerUpType()
    {
        switch (PowerUpText.GetComponent<TextMeshProUGUI>().text)
        {
            case "Weapons":
                PowerUpType = PowerUpEnum.NewWeapon;
                break;
            case "Speed":
                PowerUpType = PowerUpEnum.Speed;
                break;
            case "Cadency":
                PowerUpType = PowerUpEnum.ShootCadency;
                break;
            case "Damage":
                PowerUpType = PowerUpEnum.Damage;
                break;
            case "Damage Multiplier":
                PowerUpType = PowerUpEnum.DamageMultiplier;
                break;
            case "Range":
                PowerUpType = PowerUpEnum.Range;
                break;
            case "BulletSpeed":
                PowerUpType = PowerUpEnum.BulletSpeed;
                break;
            case "Size":
                PowerUpType = PowerUpEnum.Size;
                break;
        }
    }

    private void OnAmountButtonPressed()
    {
        if(!ManageCosts())
        {
            return;
        }
        PowerUpData.powerUpAmount += PowerUpData.powerUpUpgradeAmount;
        AmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpAmount.ToString(); 
    }

    private void OnDurationButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        PowerUpData.powerUpDuration += PowerUpData.powerUpUpgradeDuration;
        DurationButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpDuration.ToString();
    }

    private bool ManageCosts()
    {
        if (PowerUpData.powerUpCostUpgrade > SpawnerController.m_instance.Score)
        {
            return false;
        }
        SpawnerController.m_instance.SpendScore(PowerUpData.powerUpCostUpgrade);
        PowerUpData.powerUpCostUpgrade *= 2;
        CostText.GetComponent<TextMeshProUGUI>().text = PowerUpData.powerUpCostUpgrade.ToString();

        return true;
    }
}
