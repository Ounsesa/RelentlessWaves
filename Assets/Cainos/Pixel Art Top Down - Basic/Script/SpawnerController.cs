using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

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
    public float damage;
    public float speed;
    public float dropChance;
    public int scoreOnDeath;

    public EnemyValues(float health, float damage, float speed, float dropChance, int scoreOnDeath)
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.dropChance = dropChance;
        this.scoreOnDeath = scoreOnDeath;
    }
}

public class SpawnerController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    Dictionary<EnemyTypes, EnemyValues> enemyValues = new Dictionary<EnemyTypes, EnemyValues>();

    [SerializeField]
    private string EnemiesDataFileName = "EnemiesData";
    [SerializeField]
    private GameObject NextWaveCanvas;

    public IObjectPool<EnemyController> EnemyPool;

    [SerializeField]
    private float SpawnTime = 1;

    List<string[]> EnemiesListData = new List<string[]>();

    [SerializeField]
    private TopDownCharacterController player;
    [SerializeField]
    private ScoreScript ScoreText;
    [SerializeField]
    private float spawnRange = 10;

    Vector2 TopLeft = new Vector2(-10f, 17f);
    Vector2 BottomRight = new Vector2(18f,-12f);

    #region WaveController
    private int WaveNumber = 0;
    private int EnemiesPerWave = 5;
    private bool IsWaveActive = false;
    private float InitialDecreaseInSpawnRatePerWave = 0.1f;
    private float DecreaseInSpawnRatePerWave = 0.1f;

    private int EnemiesRemaining = 0;
    private int EnemiesSpawned = 0;
    private float CurrentSpawnTime = 0;

    private int EnemiesKilledWithoutDrop = 0;
    #endregion

    public static SpawnerController m_instance;
    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_instance = this;
    }

    private void Start()
    {

        EnemyPool = new ObjectPool<EnemyController>(SpawnEnemyFromPool, OnGet, OnRelease);

        Invoke("StartSpawningCoroutine", SpawnTime);

        EnemiesListData = CSVParser.ParseCSVToStringList(EnemiesDataFileName);
        EnemiesListData.RemoveAt(0);
        foreach (string[] entry in EnemiesListData)
        {
            int EnemyIndex = int.Parse(entry[0]);
            EnemyTypes EnemyType = (EnemyTypes)EnemyIndex;

            EnemyValues EnemyValue = new EnemyValues(float.Parse(entry[1]), float.Parse(entry[2]), float.Parse(entry[3]), float.Parse(entry[4]), int.Parse(entry[5]));
            enemyValues.Add(EnemyType, EnemyValue);
        }

        StartNewWave();
    }

    public void StartNewWave()
    {
        WaveNumber++;
        EnemiesRemaining = WaveNumber * EnemiesPerWave;
        DecreaseInSpawnRatePerWave = InitialDecreaseInSpawnRatePerWave / WaveNumber;
        CurrentSpawnTime = SpawnTime - WaveNumber * DecreaseInSpawnRatePerWave;
        EnemiesSpawned = 0;

        IsWaveActive = true;
        GamePauseManager.ResumeGame();
    }


    private EnemyController SpawnEnemyFromPool()
    {
        GameObject enemyGO = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
        EnemyController enemy = enemyGO.GetComponent<EnemyController>();
        enemy.Init(enemyValues[enemy.EnemyType]);
        enemy.SetPool(EnemyPool);

        return enemy;
    }

    private void OnGet(EnemyController enemy)
    {
        Debug.Log("OnGet");
        enemy.gameObject.SetActive(true);
        Vector3 spawnPosition = Vector3.zero;
        RandomSpawnPoint(player.transform.position, spawnRange, spawnRange + 2, out spawnPosition);

        Debug.Log("Spawn Position" + spawnPosition);
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

        Debug.LogWarning("Failed to find a valid spawn position after several attempts.");
    }

    private bool IsPositionSafe(Vector3 spawnPosition, float checkRadius)
    {
        // Check if there's anything within the radius that would block the spawn
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPosition, checkRadius);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Unsafe spawn position detected. Retrying...");
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
        ScoreText.AddScore(enemy.scoreOnDeath);
        enemy.gameObject.SetActive(false);

        EnemiesRemaining--;

        if(EnemiesRemaining <= 0)
        {
            NextWaveCanvas.SetActive(true);
            GamePauseManager.PauseGame();
        }
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
