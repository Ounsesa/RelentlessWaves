using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum CSVWindow
{
    PlayerStats,
    EnemyStats,
    PowerUpStats,
}

#if UNITY_EDITOR
public class PowerUpDataWindow : EditorWindow
{
    static PowerUpDataWindow window;
    private static string PowerUpDataFileName = "PowerUpData";
    private static string PlayerDataFileName = "PlayerData";
    private static string EnemiesDataFileName = "EnemiesData";
    static List<PowerUpValues> PowerUpValues = new List<PowerUpValues>();
    static List<string[]> PlayerValues = new List<string[]>();
    static List<string[]> EnemyValues = new List<string[]>();
    private CSVWindow CSVWindow = CSVWindow.PlayerStats;
    
    public static void InitWindow()
    {
        window = EditorWindow.GetWindow<PowerUpDataWindow>("DataWindow");
        window.Show();

        CSVParser.ParseStringListToPowerUpValuesList(PowerUpDataFileName, out PowerUpValues);
        PlayerValues = CSVParser.ParseCSVToStringList(PlayerDataFileName);
        EnemyValues = CSVParser.ParseCSVToStringList(EnemiesDataFileName);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Player Stats"))
        {
            CSVWindow = CSVWindow.PlayerStats;
        }
        else if(GUILayout.Button("Enemy Stats"))
        {
            CSVWindow = CSVWindow.EnemyStats;
        }
        else if(GUILayout.Button("Power Up Stats"))
        {
            CSVWindow = CSVWindow.PowerUpStats;
        }
        EditorGUILayout.EndHorizontal();

        switch (CSVWindow)
        {
            case CSVWindow.PlayerStats:
                ShowPlayerStats(); break;
            case CSVWindow.EnemyStats:
                ShowEnemyStats(); break;
            case CSVWindow.PowerUpStats:
                ShowPowerUpStats(); break;
        }

        if (GUILayout.Button("Save Data"))
        {
            switch (CSVWindow)
            {
                case CSVWindow.PlayerStats:
                    CSVParser.ParseStringListToCSV(PlayerDataFileName, PlayerValues); break;
                case CSVWindow.EnemyStats:
                    CSVParser.ParseStringListToCSV(EnemiesDataFileName, EnemyValues); break;
                case CSVWindow.PowerUpStats:
                    CSVParser.ParsePowerUpListToCSV(PowerUpDataFileName, PowerUpValues);
                    break;
            }

        }
    }

    private void ShowPlayerStats()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("Stat");
        EditorGUILayout.TextArea("Value");
        EditorGUILayout.EndHorizontal();
        for (int i = 1; i < PlayerValues.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(PlayerValues[i][0]);
            string newValue = EditorGUILayout.TextField(PlayerValues[i][1]);

            float newValueFloat;
            if (!(float.TryParse(newValue, out newValueFloat)))
            {
                //EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            PlayerValues[i][1] = newValue;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void ShowEnemyStats()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("EnemyId");
        EditorGUILayout.TextArea("Health");
        EditorGUILayout.TextArea("Damage");
        EditorGUILayout.TextArea("Speed");
        EditorGUILayout.TextArea("DropChance");
        EditorGUILayout.TextArea("ScoreOnDeath");
        EditorGUILayout.EndHorizontal();
        for (int i = 1; i < EnemyValues.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(EnemyValues[i][0]);
            string newHealth = EditorGUILayout.TextField(EnemyValues[i][1]);
            string newDamage = EditorGUILayout.TextField(EnemyValues[i][2]);
            string newSpeed = EditorGUILayout.TextField(EnemyValues[i][3]);
            string newDropChance = EditorGUILayout.TextField(EnemyValues[i][4]);
            string newScoreOnDeath = EditorGUILayout.TextField(EnemyValues[i][5]);

            float newHealthFloat;

            if (!(float.TryParse(newHealth, out newHealthFloat)))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            EnemyValues[i][1] = newHealth;
            float newDamageFloat;
            if (!(float.TryParse(newDamage, out newDamageFloat)))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            EnemyValues[i][2] = newDamage;
            float newSpeedFloat;
            if (!(float.TryParse(newSpeed, out newSpeedFloat)))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            EnemyValues[i][3] = newSpeed;  
            float newDropChanceFloat;
            if (!(float.TryParse(newDropChance, out newDropChanceFloat)))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            EnemyValues[i][4] = newDropChance;  
            float newScoreOnDeathFloat;
            if (!(float.TryParse(newScoreOnDeath, out newScoreOnDeathFloat)))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            EnemyValues[i][5] = newScoreOnDeath;            

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void ShowPowerUpStats()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("Power Up");
        EditorGUILayout.TextArea("Amount");
        EditorGUILayout.TextArea("Duration");
        EditorGUILayout.TextArea("Cost");
        EditorGUILayout.TextArea("UphradeAmount");
        EditorGUILayout.TextArea("UpgradeDuration");
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < PowerUpValues.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(PowerUpValues[i].powerUpValue.ToString());
            string newAmount = EditorGUILayout.TextField(PowerUpValues[i].powerUpAmount.ToString());
            string newDuration = EditorGUILayout.TextField(PowerUpValues[i].powerUpDuration.ToString());
            string newCost = EditorGUILayout.TextField(PowerUpValues[i].powerUpCostUpgrade.ToString());
            string newUpgradeAmount = EditorGUILayout.TextField(PowerUpValues[i].powerUpUpgradeAmount.ToString());
            string newUpgradeDuration = EditorGUILayout.TextField(PowerUpValues[i].powerUpUpgradeDuration.ToString());

            float newAmountFloat;
            if (!(float.TryParse(newAmount, out newAmountFloat) || newAmount == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpAmount = newAmountFloat;
            }
            float newDurationFloat;
            if (!(float.TryParse(newDuration, out newDurationFloat) || newDuration == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Duration", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpDuration = newDurationFloat;
            }
            int newCostFloat;
            if (!(int.TryParse(newCost, out newCostFloat) || newDuration == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Duration", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpCostUpgrade = newCostFloat;
            }
            float newUpgradeAmountFloat;
            if (!(float.TryParse(newUpgradeAmount, out newUpgradeAmountFloat) || newDuration == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Duration", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpUpgradeAmount = newUpgradeAmountFloat;
            }
            float newUpgradeDurationFloat;
            if (!(float.TryParse(newUpgradeDuration, out newUpgradeDurationFloat) || newDuration == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Duration", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpUpgradeDuration = newUpgradeDurationFloat;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}

#endif
