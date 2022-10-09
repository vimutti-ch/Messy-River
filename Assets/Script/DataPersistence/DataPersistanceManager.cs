using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useTryCatch;

    private GameData _gameData;
    private SimplifyGameData _simplifyGameData;
    private List<ILoad> _dataLoads; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private List<ISave> _dataSaves; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private FileDataHandler _dataHandler;

    public PlayfabManager playfabManager;
    public static DataPersistanceManager Instance { get; private set; }

    public GameData GameData => _gameData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistance Manager in the scene.");
        }

        Instance = this;
        this._simplifyGameData = new SimplifyGameData();
    }

    private void Start()
    {
        Debug.Log("Start Data Manager");
        this._dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this._dataLoads = FindAllLoadDataObjects();
        this._dataSaves = FindAllSaveDataObjects();
        LoadGame();
        Debug.Log("End Data Manager Start Call");
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        Debug.Log("Initialize Load Data");
        // load any saved data from a file using the data handler
        this._gameData = _dataHandler.Load();
        Debug.Log("File Load Done");

        if(useTryCatch)
        try
        {
            playfabManager.GetLeaderboard();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        else
        {
            playfabManager.GetLeaderboard();
        }
        
        Debug.Log("Get Leaderboard done");

        // if no data can be Loaded, initialize to a new game
        if (this._gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (ILoad dataPersistanceObj in _dataLoads)
        {
            dataPersistanceObj.LoadData(_gameData);
            Debug.Log("Load Data");
        }
        
        Debug.Log("Load Data Done");
    }

    public void SaveGame()
    {
        bool uRnotFastest = false;
        
        // pass the data to other scripts so they can update it
        foreach (ISave dataPersistanceObj in _dataSaves)
        {
            dataPersistanceObj.SaveData(ref _gameData, ref _simplifyGameData);
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameData);

        //Check for name
        for (int i = 0; i < _gameData.time.Length; i++)
        {
            if (_gameData.name[i] == _simplifyGameData.name)
            {
                if (_gameData.time[i] < _simplifyGameData.time)
                {
                    uRnotFastest = true;
                }
            }
        }

        if (!uRnotFastest)
        {
            //playfabManager.SendLeaderboard(_simplifyGameData);
            Debug.Log("Uploaded");
        }
    }

    /* private void OnApplicationQuit()
       {
            SaveGame();
       }
    */

    // Setup a List of all object that have script implementation of IDataPersistance
    private List<ILoad> FindAllLoadDataObjects()
    {
        IEnumerable<ILoad> loadDataObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<ILoad>();

        return new List<ILoad>(loadDataObjects);
    }
    
    private List<ISave> FindAllSaveDataObjects()
    {
        IEnumerable<ISave> saveDataObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<ISave>();

        return new List<ISave>(saveDataObjects);
    }
}
