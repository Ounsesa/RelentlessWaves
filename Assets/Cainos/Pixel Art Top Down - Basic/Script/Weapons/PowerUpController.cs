using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController m_instance;
    public string ResourceName = "PowerUpData";

    public GameObject powerUpGO;
    public TopDownCharacterController m_characterController;

    List<PowerUpValues> PowerUpValuesData = new List<PowerUpValues>();


    #region Delegates
    public event System.Action<float> OnSpeedPicked;
    public event System.Action<float> OnDamagePicked;
    public event System.Action<float> OnDamageMultiplierPicked;
    public event System.Action<float> OnRangePicked;
    public event System.Action<float> OnSizePicked;
    public event System.Action<float> OnShootCadencyPicked;
    public event System.Action<float> OnBulletSpeedPicked;
    public event System.Action<bool> OnNewWeaponPicked;
    public event System.Action<bool> OnFollowerPicked;
    public event System.Action<bool> OnPiercingPicked;
    public event System.Action<bool> OnAreaDamagePicked;
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

        CSVParser.ParseStringListToPowerUpValuesList(ResourceName, out PowerUpValuesData);
    }

    public void SpawnPowerUp(Vector3 position)
    {
        GameObject InstantiatedPowerUp = Instantiate(powerUpGO, position, Quaternion.identity);

        int numberOfValues = UnityEngine.Random.Range(1, 3);

        List<PowerUpValues> valuesForThisPowerUp = new List<PowerUpValues>();

        for(int i = 0; i < numberOfValues; i++) 
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
                OnNewWeaponPicked?.Invoke(powerUpValues.powerUpAmount > 0);
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
            case PowerUpEnum.AreaDamage:
                OnAreaDamagePicked?.Invoke(powerUpValues.powerUpAmount > 0);
                break;
            default:
                Debug.LogWarning("Unknown power-up type: " + powerUpValues.powerUpValue);
                break;
        }

        if(powerUpValues.powerUpAmount > 0)
        {
            Debug.Log(powerUpValues.powerUpValue + "+");
            PowerUpValues copiedValue = new PowerUpValues(powerUpValues);
            copiedValue.powerUpAmount = -powerUpValues.powerUpAmount;
            StartCoroutine(DelayedFunction(copiedValue.powerUpDuration, RegisterPowerUpValues, copiedValue));
        }
        else
        {
            Debug.Log(powerUpValues.powerUpValue + "-");
        }
    }



    public IEnumerator DelayedFunction<T>(float delayTime, Action<T> functionToCall, T parameter)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Call the function with the passed parameter
        functionToCall(parameter);
    }

}
