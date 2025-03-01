using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<GameObject> m_canvasToHide;
    [SerializeField]
    private List<GameObject> m_canvasToShow;
    #endregion

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartNewWave);
    }

    private void StartNewWave()
    {
        SpawnerController.instance.StartNewWave();

        foreach (GameObject go in m_canvasToHide) 
        {
            go.SetActive(false);
        }
        foreach (GameObject go in m_canvasToShow) 
        {
            go.SetActive(true);
        }
    }
}
