using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkillDuration : MonoBehaviour
{
    public GameObject PowerUpText;
    public GameObject DurationButton;
    public GameObject CostText;

    public PowerUpEnum PowerUpType;

    public PowerUpValues PowerUpData;


    private void Awake()
    {
        DurationButton.GetComponent<Button>().onClick.AddListener(OnDurationButtonPressed);
    }

    private void Start()
    {
        SetPowerUpType();
        PowerUpData = PowerUpController.m_instance.GetPowerUpData(PowerUpType);
        if (PowerUpData != null)
        {
            DurationButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpDuration.ToString("F2");
            CostText.GetComponent<TextMeshProUGUI>().text = PowerUpData.powerUpCostUpgrade.ToString();
        }
    }

    private void SetPowerUpType()
    {
        switch (PowerUpText.GetComponent<TextMeshProUGUI>().text)
        {
            case "Follower":
                PowerUpType = PowerUpEnum.Follower;
                break;
            case "Explosion":
                PowerUpType = PowerUpEnum.Explodes;
                break;
            case "Piercing":
                PowerUpType = PowerUpEnum.Piercing;
                break;            
        }
    }

    private void OnDurationButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        PowerUpData.powerUpDuration += PowerUpData.powerUpUpgradeDuration;
        DurationButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpData.powerUpDuration.ToString("F2");
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
