using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    #region Variables
    public int bonusCost = 2000;
    public static DataPersistenceManager instance;

    [Header("File Storage Config")]
    [SerializeField]
    private string m_fileName;
    private FileDataHandler m_fileDataHandler;
    private GameData m_gameData = null;
    private List<IDataPersistence> m_dataPersistences = new List<IDataPersistence>();
    #endregion

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
        m_fileDataHandler = new FileDataHandler(Application.persistentDataPath, m_fileName);
        m_dataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        m_gameData = new GameData();
        PowerUpController.instance.InitializeFromCSV();
    }

    public void LoadGame()
    {
        m_gameData = m_fileDataHandler.Load();

        if(m_gameData == null)
        {
            NewGame();
        }

        foreach(IDataPersistence persistence in m_dataPersistences)
        {
            persistence.LoadData(m_gameData);
        }

        bonusCost = m_gameData.bonusCost;
    }

    public void SaveGame()
    {
        foreach (IDataPersistence persistence in m_dataPersistences)
        {
            persistence.SaveData(ref m_gameData);
        }

        m_gameData.bonusCost = bonusCost;
        m_fileDataHandler.Save(m_gameData);
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
