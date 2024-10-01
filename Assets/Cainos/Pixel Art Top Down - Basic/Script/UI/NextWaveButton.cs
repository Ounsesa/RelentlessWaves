using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartNewWave);
    }

    private void StartNewWave()
    {
        SpawnerController.m_instance.StartNewWave();
        Canvas.SetActive(false);
    }
}
