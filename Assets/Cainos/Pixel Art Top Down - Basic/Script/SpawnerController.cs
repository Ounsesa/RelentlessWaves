using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public enum EnemyTypes
{
    Zombie1,
    Zombie2,
    Skeleton1, 
    Skeleton2,
    Tomb,
}

public class EnemyValues
{
    public float health;
    public int damage;
    public float speed;
    public float dropChance;
    public int scoreOnDeath;

    public EnemyValues(float health, int damage, float speed, float dropChance, int scoreOnDeath)
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.dropChance = dropChance;
        this.scoreOnDeath = scoreOnDeath;
    }
}

public class SpawnerController : MonoBehaviour, IDataPersistence
{

    #region Variables
    public static SpawnerController instance;
    public List<GameObject> enemyPrefabs;
    public IObjectPool<EnemyController> enemyPool;
    public int score = 0;

    [SerializeField]
    private string m_enemiesDataFileName = "EnemiesData";
    [SerializeField]
    private List<GameObject> m_canvasToShow;
    [SerializeField]
    private List<GameObject> m_canvasToHide;
    [SerializeField]
    private StatsGrid m_statsGrid;
    [SerializeField]
    private float m_spawnTime = 1;
    [SerializeField]
    private float m_timeToFirstSpawn = 5;
    [SerializeField]
    private TopDownCharacterController m_player;
    [SerializeField]
    private ScoreScript m_scoreText;
    [SerializeField]
    private float m_spawnRange = 10;
    [SerializeField]
    private GameObject m_waveText;
    [SerializeField]
    private GameObject m_enemiesRemainingText;
    private List<string[]> m_enemiesListData = new List<string[]>();
    private Dictionary<EnemyTypes, EnemyValues> m_enemyValues = new Dictionary<EnemyTypes, EnemyValues>();
    //World Dimensions
    private Vector2 m_topLeftCorner = new Vector2(-10f, 17f);
    private Vector2 m_bottomRightCorner = new Vector2(18f,-12f);

    #region WaveController
    public int waveNumber = 1;
    public bool isWaveActive = false;

    private float m_initialDecreaseInSpawnRatePerWave = 0.1f;
    private float m_decreaseInSpawnRatePerWave = 0.1f;
    private int m_enemiesPerWave = 6;
    private int m_enemiesRemaining = 0;
    private int m_enemiesSpawned = 0;
    private float m_currentSpawnTime = 0;
    private int m_enemiesKilledWithoutDrop = 0;
    #endregion


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

    public void Restart()
    {
        IEnumerable<EnemyController> dataPersistencesObjects = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemy in  dataPersistencesObjects)
        {
            enemy.enemyPool?.Release(enemy);
        }
        IEnumerable<Bullet> bullets = FindObjectsOfType<Bullet>();
        foreach(Bullet bullet in bullets)
        {
            try
            {
                bullet.BulletPool?.Release(bullet);
            }
            catch
            {

            }
        }
        IEnumerable<PowerUp> powers = FindObjectsOfType<PowerUp>();
        foreach(PowerUp power in powers)
        {
            Destroy(power.gameObject);
        }

        waveNumber = 1;
        m_enemiesPerWave = 6;
        foreach (GameObject go in m_canvasToShow)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in m_canvasToHide)
        {
            go.SetActive(false);
        }
        GamePauseManager.ResumeGame();

    }

    private void Start()
    {
        GameManager.instance.onRestartPressed.AddListener(Restart);

        enemyPool = new ObjectPool<EnemyController>(SpawnEnemyFromPool, OnGet, OnRelease);

        Invoke("StartSpawningCoroutine", m_timeToFirstSpawn);

        m_enemiesListData = CSVParser.ParseCSVToStringList(m_enemiesDataFileName);
        m_enemiesListData.RemoveAt(0);
        foreach (string[] entry in m_enemiesListData)
        {
            int EnemyIndex = int.Parse(entry[0]);
            EnemyTypes EnemyType = (EnemyTypes)EnemyIndex;

            EnemyValues EnemyValue = new EnemyValues(float.Parse(entry[1]), int.Parse(entry[2]), float.Parse(entry[3]), float.Parse(entry[4]), int.Parse(entry[5]));
            m_enemyValues.Add(EnemyType, EnemyValue);
        }
        
    }

    private void OnDestroy()
    {
        GameManager.instance.onRestartPressed.RemoveListener(Restart);
        StopAllCoroutines();
    }

    public void LoadData(GameData data)
    {
        waveNumber = data.WaveNumber;
        score = data.Score;
        m_scoreText.AddScore(score);
    }
    public void SaveData(ref GameData data)
    {
        data.WaveNumber = waveNumber;
        data.Score = score;
    }

    public void StartNewWave()
    {
        m_waveText.GetComponent<TextMeshProUGUI>().text = waveNumber.ToString();
        m_enemiesRemaining = waveNumber * m_enemiesPerWave;
        m_enemiesRemainingText.GetComponent<TextMeshProUGUI>().text = m_enemiesRemaining.ToString();
        m_decreaseInSpawnRatePerWave = m_initialDecreaseInSpawnRatePerWave * waveNumber;
        m_currentSpawnTime = Mathf.Clamp(m_spawnTime - m_decreaseInSpawnRatePerWave, 0.1f, m_spawnTime);
        m_enemiesSpawned = 0;

        Debug.Log(m_currentSpawnTime);

        isWaveActive = true;
        GamePauseManager.ResumeGame();
    }


    private EnemyController SpawnEnemyFromPool()
    {
        float RandomNumber = Random.value;

        GameObject enemyPrefab = null;

        if(RandomNumber < 0.05)
        {
            enemyPrefab = enemyPrefabs[4];
        }
        else if(RandomNumber < 0.2)
        {
            enemyPrefab = enemyPrefabs[3];
        }
        else if(RandomNumber < 0.45)
        {
            enemyPrefab = enemyPrefabs[2];
        }
        else if(RandomNumber < 0.7)
        {
            enemyPrefab = enemyPrefabs[1];
        }
        else
        {
            enemyPrefab = enemyPrefabs[0];
        }

        GameObject enemyGO = Instantiate(enemyPrefab);
        EnemyController enemy = enemyGO.GetComponent<EnemyController>();
        enemy.Init(m_enemyValues[enemy.enemyType]);
        enemy.SetPool(enemyPool);

        return enemy;
    }

    private void OnGet(EnemyController enemy)
    {
        enemy.gameObject.SetActive(true);
        Vector3 spawnPosition = Vector3.zero;
        RandomSpawnPoint(m_player.transform.position, m_spawnRange, m_spawnRange + 2, out spawnPosition);
        Debug.Log(spawnPosition);
        enemy.transform.position = spawnPosition;
        enemy.Respawn();
        m_enemiesSpawned++;
    }

    private void RandomSpawnPoint(Vector3 playerPosition, float minRange, float maxRange, out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;
        int maxAttempts = 30; // Try up to 30 times to find a valid position

        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random point inside a unit circle and scale it to be between minRange and maxRange
            Vector2 randomCirclePosition = Random.insideUnitCircle.normalized * Random.Range(minRange, maxRange);

            // Translate this random point to be around the player position
            Vector3 randomPoint = new Vector3(playerPosition.x + randomCirclePosition.x, playerPosition.y + randomCirclePosition.y, 0f);


            // Clamp the point within the map bounds (TopLeft and BottomRight)
            randomPoint.x = Mathf.Clamp(randomPoint.x, m_topLeftCorner.x, m_bottomRightCorner.x);
            randomPoint.y = Mathf.Clamp(randomPoint.y, m_bottomRightCorner.y, m_topLeftCorner.y);

            // Check if the position is safe using the IsPositionSafe method
            if (IsPositionSafe(randomPoint, 0.5f))
            {
                spawnPosition = randomPoint;
                spawnPosition.z = 0f; // Ensure z position is zero
                return; // Valid position found
            }
        }

    }

    private bool IsPositionSafe(Vector3 spawnPosition, float checkRadius)
    {
        // Check if there's anything within the radius that would block the spawn
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPosition, checkRadius);
        if (hitColliders.Length > 0)
        {
            return false; // Unsafe position
        }
        return true; // Safe position
    }


    private void OnRelease(EnemyController enemy)
    {
        m_enemiesKilledWithoutDrop++;
        float randomValue = Random.Range(0f, 1f);
        if (randomValue / m_enemiesKilledWithoutDrop < enemy.dropChance)
        {
            m_enemiesKilledWithoutDrop = 0;
            PowerUpController.instance.SpawnPowerUp(enemy.transform.position);
        }
        if(m_player.health > 0)
        {
            score += enemy.scoreOnDeath;
            m_scoreText.AddScore(enemy.scoreOnDeath);
        }
        enemy.gameObject.SetActive(false);

        m_enemiesRemaining--;
        m_enemiesRemainingText.GetComponent<TextMeshProUGUI>().text = m_enemiesRemaining.ToString();

        if (m_enemiesRemaining <= 0)
        {
            isWaveActive = false;
            m_statsGrid.RemoveNewTexts();
            foreach (GameObject go in m_canvasToShow)
            {
                go.SetActive(true);
            }
            foreach(GameObject go in m_canvasToHide)
            {
                go.SetActive(false);
            }
            waveNumber++;
            if(waveNumber >= 10)
            {
                m_enemiesPerWave = 10;
            }
            GamePauseManager.PauseGame();
        }
    }

    public void SpendScore(int ScoreToSpend)
    {
        score -= ScoreToSpend;
        m_scoreText.AddScore(-ScoreToSpend);
    }



    private void StartSpawningCoroutine()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {

            if (m_enemiesSpawned < waveNumber * m_enemiesPerWave)
            {
                enemyPool.Get();
            }
            
            yield return new WaitForSeconds(m_currentSpawnTime);
        }
    }

}
