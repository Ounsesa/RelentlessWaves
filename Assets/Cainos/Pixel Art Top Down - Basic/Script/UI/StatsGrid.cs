using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsGrid : MonoBehaviour
{
    [SerializeField] private GameObject SpeedValueText;
    [SerializeField] private GameObject SpeedValueTextNew;
    [SerializeField] private GameObject WeaponsValueText;
    [SerializeField] private GameObject WeaponsValueTextNew;
    [SerializeField] private GameObject DamageValueText;
    [SerializeField] private GameObject DamageValueTextNew;
    [SerializeField] private GameObject DamageMultiplierValueText;
    [SerializeField] private GameObject DamageMultiplierValueTextNew;
    [SerializeField] private GameObject ShootCadencyValueText;
    [SerializeField] private GameObject ShootCadencyValueTextNew;
    [SerializeField] private GameObject RangeValueText;
    [SerializeField] private GameObject RangeValueTextNew;
    [SerializeField] private GameObject BulletSpeedValueText;
    [SerializeField] private GameObject BulletSpeedValueTextNew;
    [SerializeField] private GameObject SizeValueText;
    [SerializeField] private GameObject SizeValueTextNew;
    [SerializeField] private GameObject FollowerValueText;
    [SerializeField] private GameObject FollowerValueTextNew;
    [SerializeField] private GameObject ExplodesValueText;
    [SerializeField] private GameObject ExplodesValueTextNew;
    [SerializeField] private GameObject PiercingValueText;
    [SerializeField] private GameObject PiercingValueTextNew;

    [SerializeField] private TopDownCharacterController Player;
    [SerializeField] private float DelayTime = 2;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void RemoveNewTexts()
    {
        SpeedValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        WeaponsValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        DamageValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        DamageMultiplierValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        ShootCadencyValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        RangeValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        BulletSpeedValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        SizeValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        FollowerValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        ExplodesValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
        PiercingValueTextNew.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void InitGrid()
    {
        TextMeshProUGUI textMeshPro = SpeedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.speed.ToString("F2");        

        textMeshPro = WeaponsValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.WeaponNumber.ToString();

        textMeshPro = DamageValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Damage.ToString("F2");

        textMeshPro = DamageMultiplierValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().DamageMultiplier.ToString("F2");

        textMeshPro = ShootCadencyValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.ShootCadency.ToString("F2");

        textMeshPro = RangeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Range.ToString("F2");

        textMeshPro = BulletSpeedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Speed.ToString("F2");

        textMeshPro = SizeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Size.ToString("F2");

        textMeshPro = FollowerValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "No";

        textMeshPro = ExplodesValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "No";

        textMeshPro = PiercingValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "No";


        BindDelegates();
    }

    // Bind event listeners to PowerUpController events
    private void BindDelegates()
    {
        PowerUpController.m_instance.OnSpeedPicked += SetSpeedValues;
        PowerUpController.m_instance.OnNewWeaponPicked += SetWeaponsValues;
        PowerUpController.m_instance.OnDamagePicked += SetDamageValues;
        PowerUpController.m_instance.OnDamageMultiplierPicked += SetDamageMultiplierValues;
        PowerUpController.m_instance.OnShootCadencyPicked += SetShootCadencyValues;
        PowerUpController.m_instance.OnRangePicked += SetRangeValues;
        PowerUpController.m_instance.OnBulletSpeedPicked += SetBulletSpeedValues;
        PowerUpController.m_instance.OnSizePicked += SetSizeValues;
        PowerUpController.m_instance.OnFollowerPicked += SetFollowerValues;
        PowerUpController.m_instance.OnExplosionPicked += SetExplodesValues;
        PowerUpController.m_instance.OnPiercingPicked += SetPiercingValues;
    }

    // Update methods for each stat
    private void SetSpeedValues(float value)
    {
        TextMeshProUGUI textMeshPro = SpeedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.speed.ToString("F2");

        textMeshPro = SpeedValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, SpeedValueTextNew));
    }

    private void SetWeaponsValues(bool value, int weapons)
    {

        TextMeshProUGUI textMeshPro = WeaponsValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.WeaponNumber.ToString();

        textMeshPro = WeaponsValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? $"+ {weapons}" : $"{weapons}";
        StartCoroutine(DelayedHideText(DelayTime, WeaponsValueTextNew));
    }

    private void SetDamageValues(float value)
    {
        TextMeshProUGUI textMeshPro = DamageValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Damage.ToString("F2");

        textMeshPro = DamageValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, DamageValueTextNew));
    }

    private void SetDamageMultiplierValues(float value)
    {
        TextMeshProUGUI textMeshPro = DamageMultiplierValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().DamageMultiplier.ToString("F2");

        textMeshPro = DamageMultiplierValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, DamageMultiplierValueTextNew));
    }

    private void SetShootCadencyValues(float value)
    {
        TextMeshProUGUI textMeshPro = ShootCadencyValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.ShootCadency.ToString("F2");

        textMeshPro = ShootCadencyValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, ShootCadencyValueTextNew));
    }

    private void SetRangeValues(float value)
    {
        TextMeshProUGUI textMeshPro = RangeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Range.ToString("F2");

        textMeshPro = RangeValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, RangeValueTextNew));
    }

    private void SetBulletSpeedValues(float value)
    {
        TextMeshProUGUI textMeshPro = BulletSpeedValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Speed.ToString("F2");

        textMeshPro = BulletSpeedValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, BulletSpeedValueTextNew));
    }

    private void SetSizeValues(float value)
    {
        TextMeshProUGUI textMeshPro = SizeValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = Player.bulletGameObject.GetComponent<Bullet>().Size.ToString("F2");

        textMeshPro = SizeValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value > 0 ? "+ " : "";
        textMeshPro.text += value.ToString("F2");
        StartCoroutine(DelayedHideText(DelayTime, SizeValueTextNew));
    }

    private void SetFollowerValues(bool value)
    {
        TextMeshProUGUI textMeshPro = FollowerValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";

        textMeshPro = FollowerValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(DelayTime, FollowerValueTextNew));
    }

    private void SetExplodesValues(bool value)
    {
        TextMeshProUGUI textMeshPro = ExplodesValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";

        textMeshPro = ExplodesValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(DelayTime, ExplodesValueTextNew));
    }

    private void SetPiercingValues(bool value)
    {
        TextMeshProUGUI textMeshPro = PiercingValueText.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        
        textMeshPro = PiercingValueTextNew.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = value ? "Yes" : "No";
        StartCoroutine(DelayedHideText(DelayTime, PiercingValueTextNew));
    }


    public IEnumerator DelayedHideText(float delayTime, GameObject textToHide)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        textToHide.GetComponent<TextMeshProUGUI>().text = "";
    }
}
