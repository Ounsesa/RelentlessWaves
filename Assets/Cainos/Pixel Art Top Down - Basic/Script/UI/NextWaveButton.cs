using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> CanvasToHide;
    [SerializeField]
    private List<GameObject> CanvasToShow;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartNewWave);
    }

    private void StartNewWave()
    {
        SpawnerController.m_instance.StartNewWave();

        foreach (GameObject go in CanvasToHide) 
        {
            go.SetActive(false);
        }
        foreach (GameObject go in CanvasToShow) 
        {
            go.SetActive(true);
        }
    }
}
