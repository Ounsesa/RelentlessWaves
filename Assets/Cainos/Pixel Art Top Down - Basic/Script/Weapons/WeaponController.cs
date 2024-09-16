//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WeaponController : MonoBehaviour
//{
//    public GameObject bulletGameObject;
//    public Transform cannonPosition; 
//    public float fireRate = 0.0f; // Adjust this value to set the firing cadence
//    public int damage = 1;
//    public float range = 1.0f;
//    public int numberOfBulletsShot = 1;    
//    public float reloadTime = 1.0f;
//    private bool canShoot = true;

//    public int maxAmmo;
//    public int chargerAmmo;
//    public int currentAmmo;


//    public Weapon weaponAmmo;
//    SpriteRenderer spriteRenderer;
//    public ReloadController reloadController;


//    //Initial weapon not working cause dictionary has clone names. May not be needed to fix if starting without gun
//    public void Init(Weapon _weaponAmmo, string layer, string sortingLayer)
//    {
//        gameObject.layer = LayerMask.NameToLayer(layer);
//        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//        spriteRenderer.sortingLayerName = sortingLayer;
//        transform.localPosition = new Vector3(0.3f, 0.3f);
//        transform.localRotation = Quaternion.identity;
//        maxAmmo = _weaponAmmo.maxAmmo;
//        chargerAmmo = _weaponAmmo.chargerAmmo;
//        currentAmmo = _weaponAmmo.currentAmmo;

//        weaponAmmo = _weaponAmmo;
//}
        
    
//    public void Shoot(Vector2 direction)
//    {
//        if(canShoot && currentAmmo > 0)
//        {
//            for(int i = 0; i < numberOfBulletsShot; i++)
//            {
//                GameObject bullet = Instantiate(bulletGameObject, cannonPosition);
//                bullet.GetComponent<BulletController>().Init(direction, LayerMask.LayerToName(gameObject.layer), gameObject.GetComponent<SpriteRenderer>().sortingLayerName, damage, range);
//            }

//            StartCoroutine(FireRateCooldown());

//            currentAmmo -= numberOfBulletsShot;

//            if (currentAmmo <= 0)
//            {
//                StartCoroutine(Reload());
//            }
//        }
        
//    }

//    private IEnumerator FireRateCooldown()
//    {
//        canShoot = false;
//        yield return new WaitForSeconds(fireRate);
//        canShoot = true;
//    }
//    private IEnumerator Reload()
//    {
//        reloadController.m_isReloading = true;
//        canShoot = false;
//        yield return new WaitForSeconds(reloadTime);
//        canShoot = true;
        
//        if(maxAmmo > 0)
//        {

//            currentAmmo = chargerAmmo;
//            maxAmmo -= chargerAmmo;

//        }
//    }

//    private IEnumerator Cooldown(bool boolToChange, float waitSeconds)
//    {
//        boolToChange = false;
//        yield return new WaitForSeconds(waitSeconds);
//        boolToChange = true;
//    }

//    private void OnDestroy()
//    {
//        weaponAmmo.maxAmmo = maxAmmo;
//        weaponAmmo.chargerAmmo = chargerAmmo;
//        weaponAmmo.currentAmmo = currentAmmo;
//    }
//}
