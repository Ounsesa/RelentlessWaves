using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Managers
    public static GameManager instance;
    public UnityEvent onRestartPressed;
    #endregion



    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this;
    }

    public void RestartPressed()
    {
        onRestartPressed.Invoke();
    }

}
