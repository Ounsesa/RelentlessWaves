using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;
using static Utils;

public class InputManager : Manager
{
    private InputActions[] _playerActions;

    public InputActions RegsiterInputActionsPlayer(PlayerInput player, Utils.Players playerID )
    {
        if (_playerActions == null)
        {
            _playerActions = new InputActions[Utils.PLAYERS_NUM];
        }
        _playerActions[(int)playerID] = new InputActions();

        ActionsName _actionsName = GameManager.m_instance.m_gameplayManager._actionsName;
        _playerActions[(int)playerID].m_buttonUp = player.actions.FindAction(_actionsName.m_buttonUp);
        _playerActions[(int)playerID].m_buttonDown = player.actions.FindAction(_actionsName.m_buttonDown);
        _playerActions[(int)playerID].m_buttonRight = player.actions.FindAction(_actionsName.m_buttonRight);
        _playerActions[(int)playerID].m_buttonLeft = player.actions.FindAction(_actionsName.m_buttonLeft);
        _playerActions[(int)playerID].m_buttonInteract = player.actions.FindAction(_actionsName.m_buttonInteract);
        _playerActions[(int)playerID].m_buttonRightAttack = player.actions.FindAction(_actionsName.m_buttonRightAttack);
        _playerActions[(int)playerID].m_buttonLeftAttack = player.actions.FindAction(_actionsName.m_buttonLeftAttack);
        _playerActions[(int)playerID].m_buttonChangeMeleeWeapon = player.actions.FindAction(_actionsName.m_buttonChangeMeleeWeapon);
        _playerActions[(int)playerID].m_buttonChangeRangeWeapon = player.actions.FindAction(_actionsName.m_buttonChangeRangeWeapon);
        
        return _playerActions[(int)playerID];
    }
    public InputActions GetPlayerInputAction(Utils.Players playerID)
    {
        return _playerActions[(int)playerID];
    }
}
public class InputActions
{
    #region Properties
    public InputAction m_buttonUp;
    public InputAction m_buttonDown;
    public InputAction m_buttonRight;
    public InputAction m_buttonLeft;
    public InputAction m_buttonInteract;
    public InputAction m_buttonRightAttack;
    public InputAction m_buttonLeftAttack;
    public InputAction m_buttonChangeMeleeWeapon;
    public InputAction m_buttonChangeRangeWeapon;
    #endregion
}
[System.Serializable]
public struct ActionsName
{
    public string m_buttonUp;
    public string m_buttonDown;
    public string m_buttonRight;
    public string m_buttonLeft;
    public string m_buttonInteract;
    public string m_buttonRightAttack;
    public string m_buttonLeftAttack;
    public string m_buttonChangeMeleeWeapon;
    public string m_buttonChangeRangeWeapon;
}