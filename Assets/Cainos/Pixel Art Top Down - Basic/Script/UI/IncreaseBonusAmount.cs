using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseBonusAmount : MonoBehaviour
{
    #region Variables
    [SerializeField] 
    private GameObject m_bonusAmountButton;
    [SerializeField]
    private GameObject m_costText;
    #endregion

    private void Awake()
    {
        m_bonusAmountButton.GetComponent<Button>().onClick.AddListener(OnBonusAmountButtonPressed);
    }

    private void Start()
    {
        m_costText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.bonusCost.ToString();
        m_bonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = PowerUpController.instance.numberOfValues.ToString();
    }

    private void OnBonusAmountButtonPressed()
    {
        if (!ManageCosts())
        {
            return;
        }
        int NumberOfValues = ++PowerUpController.instance.numberOfValues;
        m_bonusAmountButton.GetComponentInChildren<TextMeshProUGUI>().text = NumberOfValues.ToString();
    }

    private bool ManageCosts()
    {
        if (DataPersistenceManager.instance.bonusCost > SpawnerController.instance.score)
        {
            return false;
        }
        SpawnerController.instance.SpendScore(DataPersistenceManager.instance.bonusCost);
        DataPersistenceManager.instance.bonusCost *= 10;
        m_costText.GetComponent<TextMeshProUGUI>().text = DataPersistenceManager.instance.bonusCost.ToString();

        return true;
    }
}
