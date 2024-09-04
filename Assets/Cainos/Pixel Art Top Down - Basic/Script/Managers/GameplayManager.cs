using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager: MonoBehaviour
{
    #region Managers
    public InputManager m_inputManager;
    #endregion

    #region Properties
    [Header("Inputs")]
    public ActionsName _actionsName;
    #endregion

    #region References
    [Header("Players")]
    public TopDownCharacterController[] m_players;

    #endregion
    private void Awake()
    {
        GameManager.m_instance.m_gameplayManager = this;
        m_inputManager = new InputManager();
    }
}
