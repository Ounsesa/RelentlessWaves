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
    public List<EnemyTypeEntry> EnemyEntries;
    public List<GameObject> enemyPrefabs;

    [SerializeField]
    private string EnemiesDataFileName = "EnemiesData";

    public Dictionary<EnemyTypes, List<GameObject>> AliveEnemyPool = new Dictionary<EnemyTypes, List<GameObject>>();
    public Dictionary<EnemyTypes, List<GameObject>> DeadEnemyPool = new Dictionary<EnemyTypes, List<GameObject>>();

    [SerializeField]
    private float SpawnTime = 1;

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
        List<string[]> EnemiesListData = CSVParser.ParseCSVToStringList(EnemiesDataFileName);
        EnemiesListData.RemoveAt(0);
        foreach(string[] entry in EnemiesListData)
        {
            int EnemyIndex = int.Parse(entry[0]);

            EnemyTypes EnemyType = (EnemyTypes)EnemyIndex;

            DeadEnemyPool[EnemyType] = new List<GameObject>();
            AliveEnemyPool[EnemyType] = new List<GameObject>();

            int EnemyAmount = int.Parse(entry[4]);
            for (int i = 0; i < EnemyAmount; i++) 
            {
                GameObject NewEnemy = Instantiate(enemyPrefabs[EnemyIndex]);
                DeadEnemyPool[EnemyType].Add(NewEnemy);

                NewEnemy.GetComponent<EnemyController>().Init(EnemyType, float.Parse(entry[1]), float.Parse(entry[2]), float.Parse(entry[3]));
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
