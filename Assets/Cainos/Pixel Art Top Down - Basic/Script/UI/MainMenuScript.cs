using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<GameObject> m_canvasToHide;
    [SerializeField]
    private List<GameObject> m_canvasToShowOnPlay;
    [SerializeField]
    private List<GameObject> m_canvasToShowOnExplanation;

    [SerializeField]
    private Button m_playButton;
    [SerializeField]
    private Button m_explanationButton;
    [SerializeField]
    private Button m_exitButton;
    #endregion

    private void Awake()
    {
        m_playButton.onClick.AddListener(OnPlayButtonPressed);
        m_explanationButton.onClick.AddListener(OnGuideButtonPressed);
        m_exitButton.onClick.AddListener(OnExitButtonPressed);
    }

    private void Start()
    {
        GameManager.instance.PauseGame();
    }

    private void OnPlayButtonPressed()
    {
        GameManager.instance.ResumeGame();
        gameObject.SetActive(false);
        foreach(GameObject go in m_canvasToShowOnPlay)
        {
            go.SetActive(true);
        }
        foreach(GameObject go in m_canvasToHide)
        {
            go.SetActive(false);
        }

        if(!SpawnerController.instance.isWaveActive)
        {
            SpawnerController.instance.StartNewWave();
        }
    }

    private void OnGuideButtonPressed()
    {
        foreach (GameObject go in m_canvasToShowOnExplanation)
        {
            go.SetActive(!go.activeSelf);
        }

    }

    private void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
