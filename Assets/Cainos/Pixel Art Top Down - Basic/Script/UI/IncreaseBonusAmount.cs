using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseBonusAmount : MonoBehaviour
{
    #region Variables
    public GameObject bonusAmountButton;
    public GameObject costText;
    #endregion

    private void Awake()
    {
        bonusAmountButton.GetComponent<Button>().onClick.AddListener(OnBonusAmountButtonPressed);
    }

    private void Start()
    {
        costText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.bonusCost.ToString();
        bonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpController.instance.numberOfValues.ToString();
    }

    private void OnBonusAmountButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        int NumberOfValues = ++PowerUpController.instance.numberOfValues;
        bonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = NumberOfValues.ToString();
    }

    private bool ManageCosts()
    {
        if (DataPersistenceManager.instance.bonusCost > SpawnerController.instance.score)
        {
            return false;
        }
        SpawnerController.instance.SpendScore(DataPersistenceManager.instance.bonusCost);
        DataPersistenceManager.instance.bonusCost *= 10;
        costText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.bonusCost.ToString();

        return true;
    }
}
