using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class TopDownCharacterController : MonoBehaviour
{
    public Utils.Players m_playerID;
    public PlayerInput[] m_playerInput;


    private InputActions _input;
    private Animator animator;

    [SerializeField]
    private StatsGrid statsGrid;

    [SerializeField]
    private string PlayerDataFileName = "PlayerData";

    [HideInInspector]
    public GameObject m_interactObject;

    public GameObject bulletGameObject;
    private Vector2 lookingDirection = new Vector2(1, 0);

    public GameObject WeaponGO;
    public List<GameObject> WeaponList;
    public int WeaponNumber = 2;

    #region PlayerStats
    public float speed = 4;
    public float ShootCadency = 1;

    #endregion


    private void Start()
    {
        InitPlayer();

        animator = GetComponent<Animator>();

        PlayerInput playerInput = GetComponent<PlayerInput>();
        _input = GameManager.m_instance.m_gameplayManager.m_inputManager.RegsiterInputActionsPlayer(playerInput, m_playerID);

        InitWeapons();
        BindDelegates();

        statsGrid.InitGrid();


        Invoke("StartShootCoroutine", ShootCadency);
    }

    private void InitPlayer()
    {
        List<string[]> PlayerData = CSVParser.ParseCSVToStringList(PlayerDataFileName);
        PlayerData.RemoveAt(0);
        int index = 0;
        foreach (string[] PlayerDataLine in PlayerData)
        {
            int outInt = -1;
            switch (index)
            {
                case 0:
                    WeaponNumber = int.Parse(PlayerDataLine[1]);
                    break;
                case 1:
                    speed = float.Parse(PlayerDataLine[1]);
                    break;
                case 2:
                    ShootCadency = float.Parse(PlayerDataLine[1]);
                    break;
                case 3:
                    bulletGameObject.GetComponent<Bullet>().Damage = float.Parse(PlayerDataLine[1]);
                    break;
                case 4:
                    bulletGameObject.GetComponent<Bullet>().DamageMultiplier = float.Parse(PlayerDataLine[1]);
                    break;
                case 5:
                    bulletGameObject.GetComponent<Bullet>().Range = float.Parse(PlayerDataLine[1]);
                    break;
                case 6:
                    bulletGameObject.GetComponent<Bullet>().Speed = float.Parse(PlayerDataLine[1]);
                    break;
                case 7:
                    bulletGameObject.GetComponent<Bullet>().Size = float.Parse(PlayerDataLine[1]);
                    break;
                case 8:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().Follower = outInt == 1 ? true : false;
                    break;
                case 9:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().ExplodesOnHit = outInt == 1 ? true : false;
                    break;
                case 10:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().Piercing = outInt == 1 ? true : false;
                    break;
                case 11:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().AreaDamage = outInt == 1 ? true : false;
                    break;
            }

            index++;
        }
    }

    private void BindDelegates()
    {
        PowerUpController.m_instance.OnSpeedPicked += AddSpeed;
        PowerUpController.m_instance.OnSizePicked += AddBulletSize;
        PowerUpController.m_instance.OnExplosionPicked += SetExplosion;
        PowerUpController.m_instance.OnFollowerPicked += SetFollower;
        PowerUpController.m_instance.OnDamagePicked += AddDamage;
        PowerUpController.m_instance.OnDamageMultiplierPicked += AddDamageMultiplier;
        PowerUpController.m_instance.OnBulletSpeedPicked += AddBulletSpeed;
        PowerUpController.m_instance.OnAreaDamagePicked += SetAreaDamage;
        PowerUpController.m_instance.OnNewWeaponPicked += AddWeapon;
        PowerUpController.m_instance.OnShootCadencyPicked += ReduceShootCadency;
        PowerUpController.m_instance.OnPiercingPicked += SetPiercing;
        PowerUpController.m_instance.OnRangePicked += AddRange;
    }

    private void StartShootCoroutine()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < WeaponNumber; i++)
            {
                GameObject bullet = Instantiate(bulletGameObject);
                bullet.transform.position = WeaponList[i].transform.Find("Cannon").position;
                bullet.GetComponent<Bullet>().Direction = WeaponList[i].transform.right;

            }
            yield return new WaitForSeconds(ShootCadency);
        }
    }



    private void Update()
    {
        Movement();
        Aim();
    }

    private void Aim()
    {
        // Get the mouse position in world space
        Vector3 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the player's position
        Vector3 PlayerPosition = transform.position;

        // Calculate the direction from the player to the mouse
        Vector3 DirectionToMouse = MouseWorldPosition - PlayerPosition;

        // Calculate the angle between the player and the mouse in degrees
        float Angle = Mathf.Atan2(DirectionToMouse.y, DirectionToMouse.x) * Mathf.Rad2Deg;

        // Calculate the angle step for each weapon
        float AngleStep = 360f / WeaponNumber;

        // Define the radius of the circle (same as in the Start function)
        float radius = 0.3f;

        for (int i = 0; i < WeaponNumber; i++)
        {
            // Calculate the angle for this weapon, offset by the player's aim angle
            float WeaponAngle = Angle + AngleStep * i;

            // Convert the angle to radians
            float angleInRadians = WeaponAngle * Mathf.Deg2Rad;

            // Calculate the weapon's position on the circle around the player
            Vector3 weaponPosition = new Vector3(
                Mathf.Cos(angleInRadians) * radius,
                Mathf.Sin(angleInRadians) * radius,
                0);  // Z position remains zero since we're working in 2D

            // Set the weapon's world position relative to the player
            WeaponList[i].transform.position = transform.position + weaponPosition + Vector3.up * radius;
            WeaponList[i].transform.position = transform.position + weaponPosition + Vector3.up * radius;

            // Rotate the weapon itself to face outward
            WeaponList[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, WeaponAngle));

            // Flip the player and weapons based on the mouse's position
            if (Mathf.Abs(Angle) > 90)
            {
                // Flip the player (rotate around the y-axis)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

                // Flip the weapon's sprite vertically
                WeaponList[i].GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                // Reset player's rotation to default
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                // Reset the weapon's sprite flip
                WeaponList[i].GetComponent<SpriteRenderer>().flipY = false;
            }
        }
    }


    private void Movement()
    {
        Vector2 dir = Vector2.zero;
        if (_input.m_buttonLeft.IsPressed())
        {
            dir.x = -1;
            lookingDirection = new Vector2(-1, 0);
        }
        else if (_input.m_buttonRight.IsPressed())
        {
            dir.x = 1;
            lookingDirection = new Vector2(1, 0);
        }

        if (_input.m_buttonUp.IsPressed())
        {
            dir.y = 1;
        }
        else if (_input.m_buttonDown.IsPressed())
        {
            dir.y = -1;
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;



    }

    private void InitWeapons()
    {

        WeaponGO.SetActive(true);
        float radius = 0.3f;  // Radius of the circle around the player
        float AngleStep = 360f / WeaponNumber;  // Angle step for each weapon

        for (int i = 0; i < WeaponNumber; i++)
        {
            // Calculate the angle for this weapon
            float WeaponAngle = AngleStep * i;

            // Convert angle to radians because Unity uses radians for trigonometric functions
            float angleInRadians = WeaponAngle * Mathf.Deg2Rad;

            // Calculate the position on the circle (polar coordinates to Cartesian)
            Vector3 weaponPosition = new Vector3(
                Mathf.Cos(angleInRadians) * radius,  // X position (cosine of the angle)
                Mathf.Sin(angleInRadians) * radius,  // Y position (sine of the angle)
                0);  // Z position remains zero since we're rotating in 2D

            // Instantiate the weapon
            GameObject NewWeapon = Instantiate(WeaponGO, transform);

            // Set the position in world space, relative to the player's position
            NewWeapon.transform.position = transform.position + weaponPosition + Vector3.up * radius;

            // Add the new weapon to the list
            WeaponList.Add(NewWeapon);
        }

        // Deactivate the original weapon object
        WeaponGO.SetActive(false);

    }

    #region PowerUps
    public void AddWeapon(bool Add)
    {

        for (int i = 0; i < WeaponNumber; i++)
        {
            Destroy(WeaponList[i]);
        }
        WeaponList.Clear();
        if(Add)
        {
            WeaponNumber++;
        }
        else 
        {
            WeaponNumber--;
        }

        InitWeapons();
    }

    public void AddSpeed(float amount)
    {
        speed += amount;
    }

    public void ReduceShootCadency(float amount)
    {
        ShootCadency -= amount;
    }

    public void AddDamage(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().Damage += amount;
    }

    public void AddDamageMultiplier(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().DamageMultiplier += amount;
    }

    public void AddRange(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().Range += amount;
    }

    public void AddBulletSpeed(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().Speed += amount;
    }

    public void AddBulletSize(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().Size += amount;
    }

    public void SetFollower(bool Follower)
    {
        bulletGameObject.GetComponent<Bullet>().Follower = Follower;
    }

    public void SetExplosion(bool Explosion)
    {
        bulletGameObject.GetComponent<Bullet>().ExplodesOnHit = Explosion;
    }

    public void SetPiercing(bool Piercing)
    {
        bulletGameObject.GetComponent<Bullet>().Piercing = Piercing;
    }

    public void SetAreaDamage(bool AreaDamage)
    {
        bulletGameObject.GetComponent<Bullet>().AreaDamage = AreaDamage;
    } 


    #endregion

}













//private void AvoidCorners()
//{
//    RaycastHit2D UpLeftRay = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0.25f, 0), Vector2.up, 0.01f, LayerMask.NameToLayer(gameObject.tag));
//    RaycastHit2D UpRightRay = Physics2D.Raycast(transform.position + new Vector3(0.2f, 0.25f, 0), Vector2.up, 0.01f, LayerMask.NameToLayer(gameObject.tag));
//    if (UpLeftRay.collider != null)
//    {
//        if (UpLeftRay.collider.gameObject.GetComponent<BoxCollider2D>() && !UpLeftRay.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger)
//        {
//            print(UpLeftRay.collider.gameObject.name);
//            print("Hit");
//        }

//    }
//    if (UpRightRay.collider != null)
//    {
//        if (UpRightRay.collider.gameObject.GetComponent<BoxCollider2D>() && !UpRightRay.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger)
//        {
//            print(UpRightRay.collider.gameObject.name);
//            print("Hit2");
//        }
//    }


//    Debug.DrawRay(transform.position + new Vector3(-0.2f, 0.25f, 0), Vector2.up * 0.5f, Color.red, 1);
//    Debug.DrawRay(transform.position + new Vector3(0.2f, 0.25f, 0), Vector2.up * 0.5f, Color.red, 1);



//}