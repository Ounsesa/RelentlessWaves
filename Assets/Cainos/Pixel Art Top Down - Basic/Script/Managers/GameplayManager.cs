using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager: MonoBehaviour
{
    #region Managers
    public static GameplayManager instance;
    public InputManager inputManager;
    #endregion

    #region Properties
    [Header("Inputs")]
    public ActionsName actionsName;
    #endregion

    #region References
    [Header("Players")]
    public TopDownCharacterController player;

    #endregion

    #region PlayerLimits
    [SerializeField]
    public int maxWeapons = 20;
    [SerializeField]
    public float minShootCadency = 0.1f;
    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this;
        inputManager = new InputManager();
    }

}
