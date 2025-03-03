using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsGrid : MonoBehaviour
{
    [SerializeField] private GameObject m_speedValueText;
    [SerializeField] private GameObject m_speedValueTextNew;
    [SerializeField] private GameObject m_weaponsValueText;
    [SerializeField] private GameObject m_weaponsValueTextNew;
    [SerializeField] private GameObject m_damageValueText;
    [SerializeField] private GameObject m_damageValueTextNew;
    [SerializeField] private GameObject m_damageMultiplierValueText;
    [SerializeField] private GameObject m_damageMultiplierValueTextNew;
    [SerializeField] private GameObject m_shootCadencyValueText;
    [SerializeField] private GameObject m_shootCadencyValueTextNew;
    [SerializeField] private GameObject m_rangeValueText;
    [SerializeField] private GameObject m_rangeValueTextNew;
    [SerializeField] private GameObject m_bulletSpeedValueText;
    [SerializeField] private GameObject m_bulletSpeedValueTextNew;
    [SerializeField] private GameObject m_sizeValueText;
    [SerializeField] private GameObject m_sizeValueTextNew;
    [SerializeField] private GameObject m_followerValueText;
    [SerializeField] private GameObject m_followerValueTextNew;
    [SerializeField] private GameObject m_explodesValueText;
    [SerializeField] private GameObject m_explodesValueTextNew;
    [SerializeField] private GameObject m_piercingValueText;
    [SerializeField] private GameObject m_piercingValueTextNew;

    [SerializeField] private TopDownCharacterController m_player;
    [Tooltip("Time that upgrades appear in screen")]
    [SerializeField] private float m_delayTime = 3;


    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
        GameManager.instance.onRestartPressed.AddListener(Restart);
    }

    public void RemoveNewTexts()
    {
        m_speedValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_weaponsValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_damageValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_damageMultiplierValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_shootCadencyValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_rangeValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_bulletSpeedValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_sizeValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_followerValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_explodesValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        m_piercingValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void InitGrid()
    {
        TextMeshProUGUI TextMeshPro = m_speedValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.speed.ToString("F2");        

        TextMeshPro = m_weaponsValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.weaponNumber.ToString();

        TextMeshPro = m_damageValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().damage.ToString("F2");

        TextMeshPro = m_damageMultiplierValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().damageMultiplier.ToString("F2");

        TextMeshPro = m_shootCadencyValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.shootCadency.ToString("F2");

        TextMeshPro = m_rangeValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().range.ToString("F2");

        TextMeshPro = m_bulletSpeedValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().speed.ToString("F2");

        TextMeshPro = m_sizeValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().size.ToString("F2");

        TextMeshPro = m_followerValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = "No";

        TextMeshPro = m_explodesValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = "No";

        TextMeshPro = m_piercingValueText.GetComponent<TextMeshProUGUI>();
        TextMeshPro.text = "No";


        BindDelegates();
    }

    // Bind event listeners to PowerUpController events
    private void BindDelegates()
    {
        PowerUpController.instance.onSpeedPicked += SetSpeedValues;
        PowerUpController.instance.onNewWeaponPicked += SetWeaponsValues;
        PowerUpController.instance.onDamagePicked += SetDamageValues;
        PowerUpController.instance.onDamageMultiplierPicked += SetDamageMultiplierValues;
        PowerUpController.instance.onShootCadencyPicked += SetShootCadencyValues;
        PowerUpController.instance.onRangePicked += SetRangeValues;
        PowerUpController.instance.onBulletSpeedPicked += SetBulletSpeedValues;
        PowerUpController.instance.onSizePicked += SetSizeValues;
        PowerUpController.instance.onFollowerPicked += SetFollowerValues;
        PowerUpController.instance.onExplosionPicked += SetExplodesValues;
        PowerUpController.instance.onPiercingPicked += SetPiercingValues;
    }

    private void OnDestroy()
    {
        PowerUpController.instance.onSpeedPicked -= SetSpeedValues;
        PowerUpController.instance.onNewWeaponPicked -= SetWeaponsValues;
        PowerUpController.instance.onDamagePicked -= SetDamageValues;
        PowerUpController.instance.onDamageMultiplierPicked -= SetDamageMultiplierValues;
        PowerUpController.instance.onShootCadencyPicked -= SetShootCadencyValues;
        PowerUpController.instance.onRangePicked -= SetRangeValues;
        PowerUpController.instance.onBulletSpeedPicked -= SetBulletSpeedValues;
        PowerUpController.instance.onSizePicked -= SetSizeValues;
        PowerUpController.instance.onFollowerPicked -= SetFollowerValues;
        PowerUpController.instance.onExplosionPicked -= SetExplodesValues;
        PowerUpController.instance.onPiercingPicked -= SetPiercingValues;
    }

    // Update methods for each stat
    private void SetSpeedValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_speedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.speed.ToString("F2");

        textMeshPro = m_speedValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_speedValueTextNew));
    }

    private void SetWeaponsValues(bool value, int weapons)
    {

        TextMeshProUGUI textMeshPro = m_weaponsValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.weaponNumber.ToString();

        textMeshPro = m_weaponsValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? $"+ {weapons}" : $"{weapons}";
        StartCoroutine(DelayedHideText(m_delayTime, m_weaponsValueTextNew));
    }

    private void SetDamageValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_damageValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().damage.ToString("F2");

        textMeshPro = m_damageValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_damageValueTextNew));
    }

    private void SetDamageMultiplierValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_damageMultiplierValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().damageMultiplier.ToString("F2");

        textMeshPro = m_damageMultiplierValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_damageMultiplierValueTextNew));
    }

    private void SetShootCadencyValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_shootCadencyValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.shootCadency.ToString("F2");

        textMeshPro = m_shootCadencyValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_shootCadencyValueTextNew));
    }

    private void SetRangeValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_rangeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().range.ToString("F2");

        textMeshPro = m_rangeValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_rangeValueTextNew));
    }

    private void SetBulletSpeedValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_bulletSpeedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().speed.ToString("F2");

        textMeshPro = m_bulletSpeedValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_bulletSpeedValueTextNew));
    }

    private void SetSizeValues(float value)
    {
        TextMeshProUGUI textMeshPro = m_sizeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = m_player.bulletGameObject.GetComponent<Bullet>().size.ToString("F2");

        textMeshPro = m_sizeValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(m_delayTime, m_sizeValueTextNew));
    }

    private void SetFollowerValues(bool value)
    {
        TextMeshProUGUI textMeshPro = m_followerValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";

        textMeshPro = m_followerValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(m_delayTime, m_followerValueTextNew));
    }

    private void SetExplodesValues(bool value)
    {
        TextMeshProUGUI textMeshPro = m_explodesValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";

        textMeshPro = m_explodesValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(m_delayTime, m_explodesValueTextNew));
    }

    private void SetPiercingValues(bool value)
    {
        TextMeshProUGUI textMeshPro = m_piercingValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        
        textMeshPro = m_piercingValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(m_delayTime, m_piercingValueTextNew));
    }


    public IEnumerator DelayedHideText(float delayTime, GameObject textToHide)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        textToHide.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void Restart()
    {
        StopAllCoroutines();
    }
}
