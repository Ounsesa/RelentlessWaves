using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public static class Utils
{
    public const int PLAYERS_NUM = 2;

    //Aqui van las escenas que se van a controlar. El nombre del enum tiene que ser identico al que tiene el asset de la escena
    [Serializable]
    public enum Scenes
    {
        MainMenu,
        LevelTest,
        Main
    }

    [Serializable]
    public enum Players
    {
        Player1,
        Player2
    }
}


