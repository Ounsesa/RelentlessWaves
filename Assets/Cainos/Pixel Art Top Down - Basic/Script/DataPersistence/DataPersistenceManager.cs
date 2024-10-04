using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string FileName;

    private FileDataHandler FileDataHandler;

    private GameData GameData = null;
    private List<IDataPersistence> dataPersistences = new List<IDataPersistence>();

    public int BonusCost = 2000;

    public static DataPersistenceManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        instance = this;
    }

    private void Start()
    {
        FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName);
        dataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        GameData = new GameData();

        PowerUpController.m_instance.InitializeFromCSV();
    }

    public void LoadGame()
    {
        GameData = FileDataHandler.Load();

        if(GameData == null)
        {
            NewGame();
        }

        foreach(IDataPersistence persistence in dataPersistences)
        {
            persistence.LoadData(GameData);
        }
        BonusCost = GameData.BonusCost;
    }

    public void SaveGame()
    {
        foreach (IDataPersistence persistence in dataPersistences)
        {
            persistence.SaveData(ref GameData);
        }

        GameData.BonusCost = BonusCost;
        FileDataHandler.Save(GameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    { 
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects);
    }
}
