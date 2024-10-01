using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager m_instance;
    public static bool isPaused = false;
    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        m_instance = this;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1f;           // Resume time
        isPaused = false;              // Set the pause flag to false
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;           // Freeze time
        isPaused = true;               // Set the pause flag to true
    }
}
