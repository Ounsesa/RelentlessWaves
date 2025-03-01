using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUpgrade : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject m_powerUpText;
    [SerializeField] private GameObject m_amountButton;
    [SerializeField] private GameObject m_durationButton;
    [SerializeField] private GameObject m_costText;
    private PowerUpEnum m_powerUpType;
    private PowerUpValues m_powerUpData;
    #endregion


    private void Awake()
    {
        m_amountButton.GetComponent<Button>().onClick.AddListener(OnAmountButtonPressed);
        m_durationButton.GetComponent<Button>().onClick.AddListener(OnDurationButtonPressed);
    }

    private void Start()
    {
        SetPowerUpType();
        m_powerUpData = PowerUpController.instance.GetPowerUpData(m_powerUpType);
        if(m_powerUpData != null)
        {
            m_amountButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpAmount.ToString("F2");
            m_durationButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpDuration.ToString("F2");
            m_costText.GetComponent<TextMeshProUGUI>().text = m_powerUpData.powerUpCostUpgrade.ToString();
        }
    }

    private void SetPowerUpType()
    {
        switch (m_powerUpText.GetComponent<TextMeshProUGUI>().text)
        {
            case "Weapons":
                m_powerUpType = PowerUpEnum.NewWeapon;
                break;
            case "Speed":
                m_powerUpType = PowerUpEnum.Speed;
                break;
            case "Cadency":
                m_powerUpType = PowerUpEnum.ShootCadency;
                break;
            case "Damage":
                m_powerUpType = PowerUpEnum.Damage;
                break;
            case "Damage Multiplier":
                m_powerUpType = PowerUpEnum.DamageMultiplier;
                break;
            case "Range":
                m_powerUpType = PowerUpEnum.Range;
                break;
            case "BulletSpeed":
                m_powerUpType = PowerUpEnum.BulletSpeed;
                break;
            case "Size":
                m_powerUpType = PowerUpEnum.Size;
                break;
        }
    }

    private void OnAmountButtonPressed()
    {
        if(!ManageCosts())
        {
            return;
        }
        m_powerUpData.powerUpAmount += m_powerUpData.powerUpUpgradeAmount;
        m_amountButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpAmount.ToString(); 
    }

    private void OnDurationButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        m_powerUpData.powerUpDuration += m_powerUpData.powerUpUpgradeDuration;
        m_durationButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpDuration.ToString();
    }

    private bool ManageCosts()
    {
        if (m_powerUpData.powerUpCostUpgrade > SpawnerController.instance.score)
        {
            return false;
        }
        SpawnerController.instance.SpendScore(m_powerUpData.powerUpCostUpgrade);
        m_powerUpData.powerUpCostUpgrade *= 2;
        m_costText.GetComponent<TextMeshProUGUI>().text = m_powerUpData.powerUpCostUpgrade.ToString();

        return true;
    }
}
