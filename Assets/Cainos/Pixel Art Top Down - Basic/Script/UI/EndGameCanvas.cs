using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour
{
    [SerializeField]
    TopDownCharacterController player;
    [SerializeField]
    GameObject DiedText;
    [SerializeField]
    GameObject RestartButton;
    [SerializeField]
    GameObject WaveText;


    void Awake()
    {
        RestartButton.GetComponent<Button>().onClick.AddListener(OnRestartPressed);
    }

    private void Start()
    {
        WaveText.GetComponent<TextMeshProUGUI>().text = $"Wave count: {SpawnerController.m_instance.WaveNumber}";
    }

    void OnRestartPressed()
    {
        gameObject.SetActive(false);
        SpawnerController.m_instance.Restart();
        player.Restart();
    }
}
