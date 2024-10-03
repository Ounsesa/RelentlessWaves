using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseBonusAmount : MonoBehaviour, IDataPersistence
{
    public GameObject BonusAmountButton;
    public GameObject CostText;

    private int CurrentCost = 2000;

    private void Awake()
    {
        BonusAmountButton.GetComponent<Button>().onClick.AddListener(OnBonusAmountButtonPressed);
    }

    private void Start()
    {
        CostText.GetComponent<TextMeshProUGUI>().text = CurrentCost.ToString();
        BonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpController.m_instance.NumberOfValues.ToString();
    }
    public void LoadData(GameData data)
    {
        CurrentCost = data.BonusCost;
    }
    public void SaveData(ref GameData data)
    {
        data.BonusCost = CurrentCost;
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
        if (CurrentCost > SpawnerController.m_instance.Score)
        {
            return false;
        }
        SpawnerController.m_instance.SpendScore(CurrentCost);
        CurrentCost *= 10;
        CostText.GetComponent<TextMeshProUGUI>().text = CurrentCost.ToString();

        return true;
    }
}
