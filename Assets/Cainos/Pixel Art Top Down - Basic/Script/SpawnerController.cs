using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public enum EnemyTypes
{
    Zombie1,
    Zombie2,
    Skeleton1, 
    Skeleton2,
    Tomb,
}

[System.Serializable]
public class EnemyTypeEntry
{
    public EnemyTypes enemyType;
    public GameObject enemyPrefab;
    public int enemyAmount;

    public EnemyTypeEntry(EnemyTypes type, GameObject prefab, int enemyAmount)
    {
        enemyType = type;
        enemyPrefab = prefab;
        this.enemyAmount = enemyAmount;
    }
}


public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    public List<EnemyTypeEntry> EnemyEntries;

    public Dictionary<EnemyTypes, List<GameObject>> AliveEnemyPool = new Dictionary<EnemyTypes, List<GameObject>>();
    public Dictionary<EnemyTypes, List<GameObject>> DeadEnemyPool = new Dictionary<EnemyTypes, List<GameObject>>();

    [SerializeField]
    private float SpawnTime = 1;


    public event System.Action OnEnemyDead;

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
        Invoke("StartSpawningCoroutine", SpawnTime);

        CreateEnemies();
    }

    private void CreateEnemies()
    {
        foreach(EnemyTypeEntry entry in EnemyEntries)
        {
            DeadEnemyPool[entry.enemyType] = new List<GameObject>();
            AliveEnemyPool[entry.enemyType] = new List<GameObject>();
            for (int i = 0; i < entry.enemyAmount; i++) 
            {
                GameObject NewEnemy = Instantiate(entry.enemyPrefab);
                DeadEnemyPool[entry.enemyType].Add(NewEnemy);

                NewEnemy.GetComponent<EnemyController>().Init(entry.enemyType);
            }
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
            List<EnemyTypes> EnemyTypesWithAvailableEnemy = new List<EnemyTypes>();

            foreach (KeyValuePair<EnemyTypes, List<GameObject>> kvp in DeadEnemyPool)
            {
                if(kvp.Value.Count > 0) 
                {
                    EnemyTypesWithAvailableEnemy.Add(kvp.Key);
                }
            }
            if(EnemyTypesWithAvailableEnemy.Count == 0)
            {
                yield return new WaitForSeconds(SpawnTime);
                continue;
            }

            int RandomEnemyToSpawn = UnityEngine.Random.Range(0, EnemyTypesWithAvailableEnemy.Count);
            EnemyTypes EnemyTypeToSpawn = EnemyTypesWithAvailableEnemy[RandomEnemyToSpawn];

            GameObject EnemyToSpawn = DeadEnemyPool[EnemyTypeToSpawn][0];
            DeadEnemyPool[EnemyTypeToSpawn].RemoveAt(0);
            AliveEnemyPool[EnemyTypeToSpawn].Add(EnemyToSpawn);

            EnemyToSpawn.GetComponent<EnemyController>().Respawn();


            yield return new WaitForSeconds(SpawnTime);
        }
    }

    public void RegisterDead(EnemyTypes enemyType, GameObject Enemy)
    {
        AliveEnemyPool[enemyType].Remove(Enemy);
        DeadEnemyPool[enemyType].Add(Enemy);
        Enemy.SetActive(false);

        PowerUpController.m_instance.SpawnPowerUp(Enemy.transform.position);
    }
}
