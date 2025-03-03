using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour, IDataPersistence
{
    #region Variables
    public static PowerUpController instance;
    public string resourceName = "PowerUpData";
    public GameObject powerUpGO;
    public int numberOfValues = 1;

    private List<PowerUpValues> m_powerUpValuesData = new List<PowerUpValues>();
    #endregion


    #region Delegates
    public event System.Action<float> onSpeedPicked;
    public event System.Action<float> onDamagePicked;
    public event System.Action<float> onDamageMultiplierPicked;
    public event System.Action<float> onRangePicked;
    public event System.Action<float> onSizePicked;
    public event System.Action<float> onShootCadencyPicked;
    public event System.Action<float> onBulletSpeedPicked;
    public event System.Action<bool, int> onNewWeaponPicked;
    public event System.Action<bool> onFollowerPicked;
    public event System.Action<bool> onPiercingPicked;
    public event System.Action<bool> onExplosionPicked;
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

    public void InitializeFromCSV()
    {
        CSVParser.ParseStringListToPowerUpValuesList(resourceName, out m_powerUpValuesData);
    }

    public void LoadData(GameData data)
    {
        if(data.powerUpValuesList.Count > 0)
        {
            m_powerUpValuesData = data.powerUpValuesList;
        }
        numberOfValues = data.numberOfPowerUpValues;
    }
    public void SaveData(ref GameData data)
    {
        data.powerUpValuesList = m_powerUpValuesData;
        data.numberOfPowerUpValues = numberOfValues;
    }

    public void SpawnPowerUp(Vector3 position)
    {
        GameObject InstantiatedPowerUp = Instantiate(powerUpGO, position, Quaternion.identity);

        StartCoroutine(DelayedFunction(15, RemovePowerUp, InstantiatedPowerUp));

        List<PowerUpValues> valuesForThisPowerUp = new List<PowerUpValues>();

        for(int i = 0; i < numberOfValues; i++) 
        {
            PowerUpValues randomValue = m_powerUpValuesData[UnityEngine.Random.Range(0, m_powerUpValuesData.Count)];
            PowerUpValues copiedValue = new PowerUpValues(randomValue);
            valuesForThisPowerUp.Add(copiedValue);
        }

        InstantiatedPowerUp.GetComponent<PowerUp>().Init(valuesForThisPowerUp);
        ActiveEntityController.instance.AddActivePowerUp(InstantiatedPowerUp.GetComponent<PowerUp>());
    }

    private void RemovePowerUp(GameObject powerUp)
    {
        if (powerUp == null) return;
        ActiveEntityController.instance.RemoveActivePowerUp(powerUp.GetComponent<PowerUp>());
        Destroy(powerUp);
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
                onNewWeaponPicked?.Invoke(powerUpValues.powerUpAmount > 0, (int)powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Speed:
                onSpeedPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.ShootCadency:
                onShootCadencyPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Damage:
                onDamagePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.DamageMultiplier:
                onDamageMultiplierPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Range:
                onRangePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.BulletSpeed:
                onBulletSpeedPicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Size:
                onSizePicked?.Invoke(powerUpValues.powerUpAmount);
                break;
            case PowerUpEnum.Follower:
                onFollowerPicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            case PowerUpEnum.Explodes:
                onExplosionPicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            case PowerUpEnum.Piercing:
                onPiercingPicked?.Invoke(powerUpValues.powerUpAmount > 0);
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
        yield return new WaitForSeconds(delayTime);
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
        foreach (PowerUpValues powerUpData in m_powerUpValuesData)
        {
            if (powerUpData.powerUpValue == powerUpType)
            {
                return powerUpData;
            }
        }

        return null; 
    }

}
