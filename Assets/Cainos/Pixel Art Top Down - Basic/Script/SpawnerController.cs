using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
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
    public List<GameObject> enemyPrefabs;

    Dictionary<EnemyTypes, EnemyValues> enemyValues = new Dictionary<EnemyTypes, EnemyValues>();

    [SerializeField]
    private string EnemiesDataFileName = "EnemiesData";
    [SerializeField]
    private List<GameObject> CanvasToShow;
    [SerializeField]
    private List<GameObject> CanvasToHide;

    public IObjectPool<EnemyController> EnemyPool;

    [SerializeField]
    private float SpawnTime = 1;
    [SerializeField]
    private float TimeToFirstSpawn = 5;

    List<string[]> EnemiesListData = new List<string[]>();

    [SerializeField]
    private TopDownCharacterController player;
    [SerializeField]
    private ScoreScript ScoreText;
    [SerializeField]
    private float spawnRange = 10;


    [SerializeField]
    private GameObject WaveText;
    [SerializeField]
    private GameObject EnemiesRemainingText;

    Vector2 TopLeft = new Vector2(-10f, 17f);
    Vector2 BottomRight = new Vector2(18f,-12f);

    #region WaveController
    public int WaveNumber = 1;
    private int EnemiesPerWave = 6;
    private bool IsWaveActive = false;
    private float InitialDecreaseInSpawnRatePerWave = 0.1f;
    private float DecreaseInSpawnRatePerWave = 0.1f;

    private int EnemiesRemaining = 0;
    private int EnemiesSpawned = 0;
    private float CurrentSpawnTime = 0;

    private int EnemiesKilledWithoutDrop = 0;
    #endregion

    public static SpawnerController m_instance;

    public int Score = 0;
    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_instance = this;
    }

    public void Restart()
    {
        IEnumerable<EnemyController> dataPersistencesObjects = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemy in  dataPersistencesObjects)
        {
            enemy.EnemyPool?.Release(enemy);
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

        WaveNumber = 1;
        EnemiesPerWave = 6;
        GamePauseManager.ResumeGame();

    }

    private void Start()
    {

        EnemyPool = new ObjectPool<EnemyController>(SpawnEnemyFromPool, OnGet, OnRelease);

        Invoke("StartSpawningCoroutine", TimeToFirstSpawn);

        EnemiesListData = CSVParser.ParseCSVToStringList(EnemiesDataFileName);
        EnemiesListData.RemoveAt(0);
        foreach (string[] entry in EnemiesListData)
        {
            int EnemyIndex = int.Parse(entry[0]);
            EnemyTypes EnemyType = (EnemyTypes)EnemyIndex;

            EnemyValues EnemyValue = new EnemyValues(float.Parse(entry[1]), int.Parse(entry[2]), float.Parse(entry[3]), float.Parse(entry[4]), int.Parse(entry[5]));
            enemyValues.Add(EnemyType, EnemyValue);
        }
        
        StartNewWave();
    }

    public void LoadData(GameData data)
    {
        WaveNumber = data.WaveNumber;
        Score = data.Score;
        ScoreText.AddScore(Score);
    }
    public void SaveData(ref GameData data)
    {
        data.WaveNumber = WaveNumber;
        data.Score = Score;
    }

    public void StartNewWave()
    {
        WaveText.GetComponent<TextMeshProUGUI>().text = WaveNumber.ToString();
        EnemiesRemaining = WaveNumber * EnemiesPerWave;
        EnemiesRemainingText.GetComponent<TextMeshProUGUI>().text = EnemiesRemaining.ToString();
        DecreaseInSpawnRatePerWave = InitialDecreaseInSpawnRatePerWave * WaveNumber;
        CurrentSpawnTime = Mathf.Clamp(SpawnTime - DecreaseInSpawnRatePerWave, 0.1f, SpawnTime);
        EnemiesSpawned = 0;

        Debug.Log(CurrentSpawnTime);

        IsWaveActive = true;
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
        enemy.Init(enemyValues[enemy.EnemyType]);
        enemy.SetPool(EnemyPool);

        return enemy;
    }

    private void OnGet(EnemyController enemy)
    {
        enemy.gameObject.SetActive(true);
        Vector3 spawnPosition = Vector3.zero;
        RandomSpawnPoint(player.transform.position, spawnRange, spawnRange + 2, out spawnPosition);

        enemy.transform.position = spawnPosition;
        enemy.Respawn();
        EnemiesSpawned++;
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
            randomPoint.x = Mathf.Clamp(randomPoint.x, TopLeft.x, BottomRight.x);
            randomPoint.y = Mathf.Clamp(randomPoint.y, BottomRight.y, TopLeft.y);

            // Check if the clamped point is valid on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                // Check if the position is safe using the IsPositionSafe method
                if (IsPositionSafe(hit.position, 0.5f))
                {
                    spawnPosition = hit.position;
                    spawnPosition.z = 0f; // Ensure z position is zero
                    return; // Valid position found
                }
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
        EnemiesKilledWithoutDrop++;
        float randomValue = Random.Range(0f, 1f);
        if (randomValue / EnemiesKilledWithoutDrop < enemy.dropChance)
        {
            EnemiesKilledWithoutDrop = 0;
            PowerUpController.m_instance.SpawnPowerUp(enemy.transform.position);
        }
        if(player.health > 0)
        {
            Score += enemy.scoreOnDeath;
            ScoreText.AddScore(enemy.scoreOnDeath);
        }
        enemy.gameObject.SetActive(false);

        EnemiesRemaining--;
        EnemiesRemainingText.GetComponent<TextMeshProUGUI>().text = EnemiesRemaining.ToString();

        if (EnemiesRemaining <= 0)
        {
            foreach(GameObject go in CanvasToShow)
            {
                go.SetActive(true);
            }
            foreach(GameObject go in CanvasToHide)
            {
                go.SetActive(false);
            }
            WaveNumber++;
            if(WaveNumber >= 10)
            {
                EnemiesPerWave = 10;
            }
            GamePauseManager.PauseGame();
        }
    }

    public void SpendScore(int ScoreToSpend)
    {
        Score -= ScoreToSpend;
        ScoreText.AddScore(-ScoreToSpend);
    }



    private void StartSpawningCoroutine()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {

            if (EnemiesSpawned < WaveNumber * EnemiesPerWave)
            {
                EnemyPool.Get();
            }
            
            yield return new WaitForSeconds(CurrentSpawnTime);
        }
    }

}
