using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSkillDuration : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject m_powerUpText;
    [SerializeField] private GameObject m_durationButton;
    [SerializeField] private GameObject m_costText;
    private PowerUpEnum m_powerUpType;
    private PowerUpValues m_powerUpData;
    #endregion


    private void Awake()
    {
        m_durationButton.GetComponent<Button>().onClick.AddListener(OnDurationButtonPressed);
    }

    private void Start()
    {
        SetPowerUpType();
        m_powerUpData = PowerUpController.instance.GetPowerUpData(m_powerUpType);
        if (m_powerUpData != null)
        {
            m_durationButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpDuration.ToString("F2");
            m_costText.GetComponent<TextMeshProUGUI>().text = m_powerUpData.powerUpCostUpgrade.ToString();
        }
    }

    private void SetPowerUpType()
    {
        switch (m_powerUpText.GetComponent<TextMeshProUGUI>().text)
        {
            case "Follower":
                m_powerUpType = PowerUpEnum.Follower;
                break;
            case "Explosion":
                m_powerUpType = PowerUpEnum.Explodes;
                break;
            case "Piercing":
                m_powerUpType = PowerUpEnum.Piercing;
                break;            
        }
    }

    private void OnDurationButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        m_powerUpData.powerUpDuration += m_powerUpData.powerUpUpgradeDuration;
        m_durationButton.GetComponentInChildren<TextMeshProUGUI>().text = m_powerUpData.powerUpDuration.ToString("F2");
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
