using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour, IDataPersistence
{
    public static PowerUpController m_instance;
    public string ResourceName = "PowerUpData";

    public GameObject powerUpGO;
    public TopDownCharacterController m_characterController;

    public int NumberOfValues = 1;

    List<PowerUpValues> PowerUpValuesData = new List<PowerUpValues>();


    #region Delegates
    public event System.Action<float> OnSpeedPicked;
    public event System.Action<float> OnDamagePicked;
    public event System.Action<float> OnDamageMultiplierPicked;
    public event System.Action<float> OnRangePicked;
    public event System.Action<float> OnSizePicked;
    public event System.Action<float> OnShootCadencyPicked;
    public event System.Action<float> OnBulletSpeedPicked;
    public event System.Action<bool, int> OnNewWeaponPicked;
    public event System.Action<bool> OnFollowerPicked;
    public event System.Action<bool> OnPiercingPicked;
    public event System.Action<bool> OnExplosionPicked;
    #endregion

    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_instance = this;

    }

    public void InitializeFromCSV()
    {
        CSVParser.ParseStringListToPowerUpValuesList(ResourceName, out PowerUpValuesData);
    }

    public void LoadData(GameData data)
    {
        if(data.powerUpValuesList.Count > 0)
        {
            PowerUpValuesData = data.powerUpValuesList;
        }
        NumberOfValues = data.NumberOfPowerUpValues;
    }
    public void SaveData(ref GameData data)
    {
        data.powerUpValuesList = PowerUpValuesData;
        data.NumberOfPowerUpValues = NumberOfValues;
    }

    public void SpawnPowerUp(Vector3 position)
    {
        GameObject InstantiatedPowerUp = Instantiate(powerUpGO, position, Quaternion.identity);
        Destroy(InstantiatedPowerUp, 15f);

        List<PowerUpValues> valuesForThisPowerUp = new List<PowerUpValues>();

        for(int i = 0; i < NumberOfValues; i++) 
        {
            PowerUpValues randomValue = PowerUpValuesData[UnityEngine.Random.Range(0, PowerUpValuesData.Count)];

            // Assuming PowerUpValues is a class, create a deep copy if needed (using a copy constructor or manual copy)
            PowerUpValues copiedValue = new PowerUpValues(randomValue);

            // Add the copied value to the list
            valuesForThisPowerUp.Add(copiedValue);
        }

        InstantiatedPowerUp.GetComponent<PowerUp>().Init(valuesForThisPowerUp);
    }

    public void RegisterPowerUpPicked(PowerUp powerUp)
    {

        foreach (PowerUpValues powerUpValues in powerUp.powerUpValuesList)
        {
            RegisterPowerUpValues(powerUpValues);
        }
    }

    public void RegisterPowerUpValues(PowerUpValues powerUpValues)
    {
        switch (powerUpValues.powerUpValue)
        {
            case PowerUpEnum.NewWeapon:
                OnNewWeaponPicked?.Invoke(powerUpValues.powerUpAmount > 0, (int)powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Speed:
                OnSpeedPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.ShootCadency:
                OnShootCadencyPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Damage:
                OnDamagePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.DamageMultiplier:
                OnDamageMultiplierPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Range:
                OnRangePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.BulletSpeed:
                OnBulletSpeedPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Size:
                OnSizePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Follower:
                OnFollowerPicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            case PowerUpEnum.Explodes:
                OnExplosionPicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            case PowerUpEnum.Piercing:
                OnPiercingPicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            default:
                Debug.LogWarning("Unknown power-up type: " + powerUpValues.powerUpValue);
                break;
        }

        if(powerUpValues.powerUpAmount > 0)
        {
            PowerUpValues copiedValue = new PowerUpValues(powerUpValues);
            copiedValue.powerUpAmount = -powerUpValues.powerUpAmount;
            StartCoroutine(DelayedFunction(copiedValue.powerUpDuration, RegisterPowerUpValues, copiedValue));
        }
    }



    public IEnumerator DelayedFunction<T>(float delayTime, Action<T> functionToCall, T parameter)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Call the function with the passed parameter
        functionToCall(parameter);
    }

    public void AddDuration(PowerUpEnum powerUpType, float DurationToAdd)
    {
        PowerUpValues powerUpData = GetPowerUpData(powerUpType);
        if (powerUpData != null)
        {
            powerUpData.powerUpDuration += DurationToAdd;
        }
    }

    public void AddAmount(PowerUpEnum powerUpType, float AmountToAdd)
    {
        PowerUpValues powerUpData = GetPowerUpData(powerUpType);
        if (powerUpData != null)
        {
            powerUpData.powerUpAmount += AmountToAdd;
        }
    }

    public PowerUpValues GetPowerUpData(PowerUpEnum powerUpType)
    {
        foreach (PowerUpValues powerUpData in PowerUpValuesData)
        {
            if (powerUpData.powerUpValue == powerUpType)
            {
                return powerUpData;
            }
        }

        return null; 
    }

}
