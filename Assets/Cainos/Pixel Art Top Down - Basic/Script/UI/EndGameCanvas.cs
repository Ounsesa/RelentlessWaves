using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject m_restartButton;
    [SerializeField]
    private GameObject m_waveText;
    #endregion

    void Awake()
    {
        m_restartButton.GetComponent<Button>().onClick.AddListener(OnRestartPressed);
    }

    private void Start()
    {
        m_waveText.GetComponent<TextMeshProUGUI>().text = $"Wave count: {SpawnerController.instance.waveNumber}";
    }

    void OnRestartPressed()
    {
        gameObject.SetActive(false);
        GameManager.instance.RestartPressed();
    }
}
