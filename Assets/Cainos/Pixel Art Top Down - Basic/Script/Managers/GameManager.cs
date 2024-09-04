using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Managers
    public static GameManager m_instance;
    [HideInInspector]
    public GameplayManager m_gameplayManager;
    #endregion

    #region UI
    //[Header("UI")]
    //[SerializeField]
    //private Canvas m_canvas;
    #endregion

    //[Header("Gameplay")]


    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_instance = this;
    }

    private void Update()
    {
    }

}
