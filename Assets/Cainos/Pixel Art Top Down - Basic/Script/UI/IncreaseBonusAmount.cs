using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseBonusAmount : MonoBehaviour
{
    public GameObject BonusAmountButton;
    public GameObject CostText;

    private void Awake()
    {
        BonusAmountButton.GetComponent<Button>().onClick.AddListener(OnBonusAmountButtonPressed);
    }

    private void Start()
    {
        CostText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.BonusCost.ToString();
        BonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpController.m_instance.NumberOfValues.ToString();
    }

    private void OnBonusAmountButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        PowerUpController.m_instance.NumberOfValues++;
        BonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpController.m_instance.NumberOfValues.ToString();
    }

    private bool ManageCosts()
    {
        if (DataPersistenceManager.instance.BonusCost > SpawnerController.m_instance.Score)
        {
            return false;
        }
        SpawnerController.m_instance.SpendScore(DataPersistenceManager.instance.BonusCost);
        DataPersistenceManager.instance.BonusCost *= 10;
        CostText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.BonusCost.ToString();

        return true;
    }
}
