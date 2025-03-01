using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;
using static Utils;

public class InputManager : Manager
{
    private InputActions[] m_playerActions;

    public InputActions RegsiterInputActionsPlayer(PlayerInput player, Utils.Players playerID )
    {
        if (m_playerActions == null)
        {
            m_playerActions = new InputActions[Utils.PLAYERS_NUM];
        }
        m_playerActions[(int)playerID] = new InputActions();

        ActionsName _actionsName = GameplayManager.instance.actionsName;
        m_playerActions[(int)playerID].buttonUp = player.actions.FindAction(_actionsName.buttonUp);
        m_playerActions[(int)playerID].buttonDown = player.actions.FindAction(_actionsName.buttonDown);
        m_playerActions[(int)playerID].buttonRight = player.actions.FindAction(_actionsName.buttonRight);
        m_playerActions[(int)playerID].buttonLeft = player.actions.FindAction(_actionsName.buttonLeft);
        m_playerActions[(int)playerID].buttonAimGamepad = player.actions.FindAction(_actionsName.buttonAimGamepad);
        m_playerActions[(int)playerID].buttonMovementGamepad = player.actions.FindAction(_actionsName.buttonMovementGamepad);
        m_playerActions[(int)playerID].buttonPause = player.actions.FindAction(_actionsName.buttonPause);
        
        return m_playerActions[(int)playerID];
    }
    public InputActions GetPlayerInputAction(Utils.Players playerID)
    {
        return m_playerActions[(int)playerID];
    }
}
public class InputActions
{
    #region Properties
    public InputAction buttonUp;
    public InputAction buttonDown;
    public InputAction buttonRight;
    public InputAction buttonLeft;
    public InputAction buttonAimGamepad;
    public InputAction buttonMovementGamepad;
    public InputAction buttonPause;
    #endregion
}
[System.Serializable]
public struct ActionsName
{
    public string buttonUp;
    public string buttonDown;
    public string buttonRight;
    public string buttonLeft;
    public string buttonAimGamepad;
    public string buttonMovementGamepad;
    public string buttonPause;
}