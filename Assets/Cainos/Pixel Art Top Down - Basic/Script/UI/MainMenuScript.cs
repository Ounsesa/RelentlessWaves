using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> CanvasToHide;
    [SerializeField]
    private List<GameObject> CanvasToShowOnPlay;
    [SerializeField]
    private List<GameObject> CanvasToShowOnExplanation;

    [SerializeField]
    private Button PlayButton;
    [SerializeField]
    private Button ExplanationButton;
    [SerializeField]
    private Button ExitButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(OnPlayButtonPressed);
        ExplanationButton.onClick.AddListener(OnGuideButtonPressed);
        ExitButton.onClick.AddListener(OnExitButtonPressed);
    }

    private void Start()
    {
        GamePauseManager.PauseGame();
    }

    private void OnPlayButtonPressed()
    {
        GamePauseManager.ResumeGame();
        gameObject.SetActive(false);
        foreach(GameObject go in CanvasToShowOnPlay)
        {
            go.SetActive(true);
        }
        foreach(GameObject go in CanvasToHide)
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
        foreach (GameObject go in CanvasToShowOnExplanation)
        {
            go.SetActive(!go.activeSelf);
        }

    }

    private void OnExitButtonPressed()
    {
        Application.Quit();
    }
}
