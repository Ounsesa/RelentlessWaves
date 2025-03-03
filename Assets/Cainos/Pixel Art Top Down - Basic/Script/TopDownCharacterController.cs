using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;

public class TopDownCharacterController : MonoBehaviour, IDataPersistence
{
    #region Variables
    public Utils.Players playerID;
    public GameObject mainMenuCanvas;
    public GameObject bulletGameObject;
    public GameObject weaponGO;

    [HideInInspector]
    public List<GameObject> weaponList;
    [HideInInspector]
    public int weaponNumber = 2;

    [SerializeField]
    private HealthBar m_healthBar;
    [SerializeField]
    private GameObject m_endGameScreen;
    [SerializeField]
    private GameObject m_damageScreen;
    [SerializeField]
    private GameObject m_waveCanvas;
    [SerializeField]
    private StatsGrid m_statsGrid;
    [SerializeField]
    private string m_playerDataFileName = "PlayerData";
    private IObjectPool<Bullet> m_bulletPool;
    private Vector3 m_lastMousePosition;
    private Vector3 m_lastGamePadPosition;
    private InputActions m_input;
    private Animator m_animator;



    #region PlayerStats
    [HideInInspector]
    public float speed = 4;
    [HideInInspector]
    public float shootCadency = 1;
    [HideInInspector]
    public int health = 20;

    private bool m_canTakeDamage = true;
    [SerializeField]
    private float m_invulnerabilityTime = 1f;

    #endregion


    #endregion

    public void Restart()
    {
        transform.position = new Vector3(0,0,0);
        health = 20;
        m_healthBar.SetHealth(health);
    }
    public void LoadData(GameData data)
    {
        health = data.playerHealth;
        if(health <= 0)
        {
            health = 20;
            EndGame();
        }

        m_healthBar.SetHealth(health);
    }
    public void SaveData(ref GameData data)
    {
        data.playerHealth = health;
    }

    private void Start()
    {
        GameManager.instance.onRestartPressed.AddListener(Restart);
        InitPlayer();

        m_animator = GetComponent<Animator>();

        PlayerInput playerInput = GetComponent<PlayerInput>();
        m_input = GameplayManager.instance.inputManager.RegsiterInputActionsPlayer(playerInput, playerID);


        m_bulletPool = new ObjectPool<Bullet>(InstantiateBullet, OnGet, OnRelease);

        InitWeapons();
        BindDelegates();

        Invoke("StartShootCoroutine", shootCadency);
    }

    private void OnDestroy()
    {
        GameManager.instance.onRestartPressed.RemoveListener(Restart);
        StopAllCoroutines();
    }

    private Bullet InstantiateBullet()
    {
        GameObject bulletGO = Instantiate(bulletGameObject);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.SetPool(m_bulletPool);
        return bullet;
    }

    private void OnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.RespawnBullet(bulletGameObject.GetComponent<Bullet>());
        ActiveEntityController.instance.AddActiveBullet(bullet);
    }

    private void OnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        ActiveEntityController.instance.RemoveActiveBullet(bullet);
    }

    private void InitPlayer()
    {
        List<string[]> PlayerData = CSVParser.ParseCSVToStringList(m_playerDataFileName);
        PlayerData.RemoveAt(0);
        int index = 0;
        foreach (string[] PlayerDataLine in PlayerData)
        {
            int outInt = -1;
            switch (index)
            {
                case 0:
                    weaponNumber = int.Parse(PlayerDataLine[1]);
                    break;
                case 1:
                    speed = float.Parse(PlayerDataLine[1]);
                    break;
                case 2:
                    shootCadency = float.Parse(PlayerDataLine[1]);
                    break;
                case 3:
                    bulletGameObject.GetComponent<Bullet>().damage = float.Parse(PlayerDataLine[1]);
                    break;
                case 4:
                    bulletGameObject.GetComponent<Bullet>().damageMultiplier = float.Parse(PlayerDataLine[1]);
                    break;
                case 5:
                    bulletGameObject.GetComponent<Bullet>().range = float.Parse(PlayerDataLine[1]);
                    break;
                case 6:
                    bulletGameObject.GetComponent<Bullet>().speed = float.Parse(PlayerDataLine[1]);
                    break;
                case 7:
                    bulletGameObject.GetComponent<Bullet>().size = float.Parse(PlayerDataLine[1]);
                    break;
                case 8:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().follower = outInt == 1 ? true : false;
                    break;
                case 9:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().explodesOnHit = outInt == 1 ? true : false;
                    break;
                case 10:
                    outInt = -1;
                    int.TryParse(PlayerDataLine[1], out outInt);
                    bulletGameObject.GetComponent<Bullet>().piercing = outInt == 1 ? true : false;
                    break;
                case 11:
                    //health = int.Parse(PlayerDataLine[1]);
                    break;
            }

            index++;
        }
    }

    private void BindDelegates()
    {
        PowerUpController.instance.onSpeedPicked += AddSpeed;
        PowerUpController.instance.onSizePicked += AddBulletSize;
        PowerUpController.instance.onExplosionPicked += SetExplosion;
        PowerUpController.instance.onFollowerPicked += SetFollower;
        PowerUpController.instance.onDamagePicked += AddDamage;
        PowerUpController.instance.onDamageMultiplierPicked += AddDamageMultiplier;
        PowerUpController.instance.onBulletSpeedPicked += AddBulletSpeed;
        PowerUpController.instance.onNewWeaponPicked += AddWeapon;
        PowerUpController.instance.onShootCadencyPicked += ReduceShootCadency;
        PowerUpController.instance.onPiercingPicked += SetPiercing;
        PowerUpController.instance.onRangePicked += AddRange;
    }

    private void StartShootCoroutine()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if(health > 0)
            {
                for (int i = 0; i < weaponNumber; i++)
                {
                    Bullet bullet = m_bulletPool.Get();
                    bullet.transform.position = weaponList[i].transform.Find("Cannon").position;
                    bullet.direction = weaponList[i].transform.right;

                }
            }
            yield return new WaitForSeconds(shootCadency);
        }
    }



    private void Update()
    {
        Movement();
        Aim();
        Pause();
    }

    private void Pause()
    {
        if(m_input.buttonPause.triggered && transform.position != Vector3.zero)
        {
            if (mainMenuCanvas.activeSelf)
            {
                GameManager.instance.ResumeGame();
                mainMenuCanvas.SetActive(false);
            }
            else
            {
                GameManager.instance.PauseGame();
                mainMenuCanvas.SetActive(true);
            }
        }

    }

    private void Aim()
    {
        if(health <= 0)
        { return; }

        Vector3 DirectionToMouse = Vector3.zero;
        Vector2 GamepadMovement = m_input.buttonAimGamepad.ReadValue<Vector2>();
        if(GamepadMovement != Vector2.zero)
        {
            DirectionToMouse = GamepadMovement;
            m_lastMousePosition = Input.mousePosition;
            m_lastGamePadPosition = GamepadMovement;
        }
        else if(Input.mousePosition != m_lastMousePosition)
        { 
            // Get the mouse position in world space
            Vector3 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get the player's position
            Vector3 PlayerPosition = transform.position;

            // Calculate the direction from the player to the mouse
            DirectionToMouse = MouseWorldPosition - PlayerPosition;
        }
        else
        {
            DirectionToMouse = m_lastGamePadPosition;
        }

        

        // Calculate the angle between the player and the mouse in degrees
        float Angle = Mathf.Atan2(DirectionToMouse.y, DirectionToMouse.x) * Mathf.Rad2Deg;

        // Calculate the angle step for each weapon
        float AngleStep = 360f / weaponNumber;

        // Define the radius of the circle (same as in the Start function)
        float radius = 0.3f;

        for (int i = 0; i < weaponNumber; i++)
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
            weaponList[i].transform.position = transform.position + weaponPosition + Vector3.up * radius;
            weaponList[i].transform.position = transform.position + weaponPosition + Vector3.up * radius;

            // Rotate the weapon itself to face outward
            weaponList[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, WeaponAngle));

            // Flip the player and weapons based on the mouse's position
            if (Mathf.Abs(Angle) > 90)
            {
                // Flip the player (rotate around the y-axis)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

                // Flip the weapon's sprite vertically
                weaponList[i].GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                // Reset player's rotation to default
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                // Reset the weapon's sprite flip
                weaponList[i].GetComponent<SpriteRenderer>().flipY = false;
            }
        }
    }


    private void Movement()
    {
        if (health <= 0)
        { return; }

        Vector2 dir = m_input.buttonMovementGamepad.ReadValue<Vector2>();
        if (m_input.buttonLeft.IsPressed())
        {
            dir.x = -1;
        }
        else if (m_input.buttonRight.IsPressed())
        {
            dir.x = 1;
        }

        if (m_input.buttonUp.IsPressed())
        {
            dir.y = 1;
        }
        else if (m_input.buttonDown.IsPressed())
        {
            dir.y = -1;
        }

        dir.Normalize();
        m_animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;



    }

    private void InitWeapons()
    {

        weaponGO.SetActive(true);
        float radius = 0.3f;  // Radius of the circle around the player
        float AngleStep = 360f / weaponNumber;  // Angle step for each weapon

        for (int i = 0; i < weaponNumber; i++)
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
            GameObject NewWeapon = Instantiate(weaponGO, transform);

            // Set the position in world space, relative to the player's position
            NewWeapon.transform.position = transform.position + weaponPosition + Vector3.up * radius;

            // Add the new weapon to the list
            weaponList.Add(NewWeapon);
        }

        // Deactivate the original weapon object
        weaponGO.SetActive(false);

    }

    private void RelocateWeapons()
    {
        float radius = 0.3f;  // Radius of the circle around the player
        float AngleStep = 360f / weaponNumber;  // Angle step for each weapon

        for (int i = 0; i < weaponNumber; i++)
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
            GameObject Weapon = weaponList[i];

            // Set the position in world space, relative to the player's position
            Weapon.transform.position = transform.position + weaponPosition + Vector3.up * radius;

        }
    }

    private void TakeDamage(int damage)
    {
        if(m_canTakeDamage && health > 0)
        {
            health -= damage;
            m_healthBar.SetHealth(health);
            m_canTakeDamage = false;
            m_damageScreen.SetActive(true);
            Invoke("RemoveDamageScreen", 0.25f);
            Invoke("RemuveInvulnerability", m_invulnerabilityTime);

            if(health <= 0)
            {
                EndGame();
            }
        }
    }

    private void RemoveDamageScreen()
    {
        m_damageScreen.SetActive(false);
    }

    private void EndGame()
    {
        m_waveCanvas.SetActive(false);
        m_endGameScreen.SetActive(true);
    }

    private void RemuveInvulnerability()
    {
        m_canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyController>().damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyController>().damage);
        }
    }

    #region PowerUps
    public void AddWeapon(bool Add, int weapons)
    {        
        if(Add)
        {
            weaponNumber = Mathf.Clamp(weaponNumber + weapons, 1, GameplayManager.instance.maxWeapons);

            weaponGO.SetActive(true);

            for(int i = 0; i < weapons; i++)
            {
                GameObject NewWeapon = Instantiate(weaponGO, transform);
                weaponList.Add(NewWeapon);
            }

            weaponGO.SetActive(false);
        }
        else 
        {
            weaponNumber = Mathf.Clamp(weaponNumber + weapons, 1, GameplayManager.instance.maxWeapons);
            for (int i =0; i< -weapons; i++)
            {
                GameObject WeaponToRemove = weaponList[0];
                weaponList.Remove(WeaponToRemove);
                Destroy(WeaponToRemove);
            }
        }

        RelocateWeapons();
    }

    public void AddSpeed(float amount)
    {
        speed += amount;
    }

    public void ReduceShootCadency(float amount)
    {
        //If the shoot cadency is 0.1, it won't lower more, but it will register the up reset of the power up, so picking this power up in 0.1 is bad

        shootCadency = Mathf.Clamp(shootCadency - amount, GameplayManager.instance.minShootCadency, 1);
    }

    public void AddDamage(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().damage += amount;
    }

    public void AddDamageMultiplier(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().damageMultiplier += amount;
    }

    public void AddRange(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().range += amount;
    }

    public void AddBulletSpeed(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().speed += amount;
    }

    public void AddBulletSize(float amount)
    {
        bulletGameObject.GetComponent<Bullet>().size += amount;
    }

    public void SetFollower(bool Follower)
    {
        bulletGameObject.GetComponent<Bullet>().follower = Follower;
    }

    public void SetExplosion(bool Explosion)
    {
        bulletGameObject.GetComponent<Bullet>().explodesOnHit = Explosion;
    }

    public void SetPiercing(bool Piercing)
    {
        bulletGameObject.GetComponent<Bullet>().piercing = Piercing;
    }



    #endregion

}


