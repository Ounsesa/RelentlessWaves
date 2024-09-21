using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVParser : MonoBehaviour
{
    public static List<string[]> ParseCSVToStringList(string resourceName)
    {

        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);

        // Check if the file was successfully loaded
        if (csvFile == null)
        {
            Debug.LogError($"Failed to load resource: {resourceName}");
            return new List<string[]>();
        }

        // Read the CSV content and split it into lines
        List<string[]> lines = new List<string[]>();
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(';');
                lines.Add(values);
            }
        }

        return lines;
        
    }

    public static void ParseStringListToPowerUpValuesList(string resourceName, out List<PowerUpValues> PowerUpValuesList)
    {
        // Initialize the out parameter
        PowerUpValuesList = new List<PowerUpValues>();

        List<string[]> lines = CSVParser.ParseCSVToStringList(resourceName);

        // Fill IntMatrix with the values from the CSV file
        foreach (var line in lines)
        {
            if (line == lines[0])
            {
                continue;
            }
            PowerUpValues row = new PowerUpValues();

            switch (line[0])
            {
                case "NewWeapon":
                    row.powerUpValue = PowerUpEnum.NewWeapon;
                    break;
                case "Speed":
                    row.powerUpValue = PowerUpEnum.Speed;
                    break;
                case "ShootCadency":
                    row.powerUpValue = PowerUpEnum.ShootCadency;
                    break;
                case "Damage":
                    row.powerUpValue = PowerUpEnum.Damage;
                    break;
                case "DamageMultiplier":
                    row.powerUpValue = PowerUpEnum.DamageMultiplier;
                    break;
                case "Range":
                    row.powerUpValue = PowerUpEnum.Range;
                    break;
                case "BulletSpeed":
                    row.powerUpValue = PowerUpEnum.BulletSpeed;
                    break;
                case "Size":
                    row.powerUpValue = PowerUpEnum.Size;
                    break;
                case "Follower":
                    row.powerUpValue = PowerUpEnum.Follower;
                    break;
                case "Explodes":
                    row.powerUpValue = PowerUpEnum.Explodes;
                    break;
                case "Piercing":
                    row.powerUpValue = PowerUpEnum.Piercing;
                    break;
                case "AreaDamage":
                    row.powerUpValue = PowerUpEnum.AreaDamage;
                    break;
                default:
                    throw new ArgumentException($"Unknown power-up type: {line[0]}");
            }


            row.powerUpAmount = float.Parse(line[1]);
            row.powerUpDuration = float.Parse(line[2]);
            PowerUpValuesList.Add(row);
        }
    }


    public static void ParsePowerUpListToCSV(string resourceName, List<PowerUpValues> powerUpValuesList)
    {
        // Construct the full file path in a writable directory like Application.persistentDataPath
        string filePath = "Assets/Resources/" + resourceName + ".csv";

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Debug.Log($"Not found directory {filePath}");
            return;
        }

        // Create a stream writer to write to the CSV file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the header
            writer.WriteLine("PowerUpType;Amount;Duration");

            // Write each PowerUpValues entry as a line in the CSV file
            foreach (var powerUp in powerUpValuesList)
            {
                string line = $"{powerUp.powerUpValue};{powerUp.powerUpAmount};{powerUp.powerUpDuration}";
                writer.WriteLine(line);
            }
        }

        Debug.Log($"PowerUp data successfully written to {filePath}");
    }

    public static void ParseStringListToCSV(string resourceName, List<string[]> stringList)
    { 
        //Construct the full file path in a writable directory like Application.persistentDataPath
        string filePath = "Assets/Resources/" + resourceName + ".csv";

        // Ensure the directory exists
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Debug.Log($"Not found directory {filePath}");
            return;
        }

        // Create a stream writer to write to the CSV file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            string line = "";
            // Write each PowerUpValues entry as a line in the CSV file
            foreach (string[] stringRow in stringList)
            {
                foreach(string stringValue in stringRow)
                {
                    line += stringValue + ";";
                }
                line.Remove(line.Length - 1);
                writer.WriteLine(line);
            }
        }

        Debug.Log($"PowerUp data successfully written to {filePath}");

    }

}
