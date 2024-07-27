using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dan.Main;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useTryCatch;
    [SerializeField] private int maxDataSize = 5; 
    
    private GameData[] _gameDatas;
    private GameData _currentRunData;
    private List<ILoad> _dataLoads; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private List<ISave> _dataSaves; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private FileDataHandler _dataHandler;

    public static DataPersistanceManager Instance { get; private set; }

    public GameData[] GameDatas => _gameDatas;

    private GameData[] _onlineDatas;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistance Manager in the scene.");
        }

        Instance = this;
        _gameDatas = new GameData[maxDataSize];
        _currentRunData = new GameData();
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
        this._gameDatas = new GameData[maxDataSize];
    }

    public void LoadGame()
    {
        Debug.Log("Initialize Load Data");
        // load any saved data from a file using the data handler
        this._gameDatas = _dataHandler.Load();
        Debug.Log("File Load Done");

        // if(useTryCatch)
        // try
        // {
        //     // playfabManager.GetLeaderboard();
        // }
        // catch (Exception e)
        // {
        //     Console.WriteLine(e);
        // }
        // else
        // {
        //     // playfabManager.GetLeaderboard();
        // }
        
        Leaderboards.test.GetEntries(entries =>
        {
            int length = Mathf.Min(_gameDatas.Length, entries.Length);
            this._gameDatas = new GameData[length];
            
            for (int i = 0; i < length; i++)
            {
                _gameDatas[i] = new GameData();
                _gameDatas[i].name = entries[i].Username;
                _gameDatas[i].time = entries[i].Score;
            }
            
            // if no data can be Loaded, initialize to a new game
            if (this._gameDatas == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            // push the loaded data to all other scripts that need it
            foreach (ILoad dataPersistanceObj in _dataLoads)
            {
                dataPersistanceObj.LoadData(_gameDatas);
                Debug.Log("Load Data");
            }
        
            Debug.Log("Load Data Done");
        });
        
        Debug.Log("Exit Data load call");
    }

    public void SaveGame()
    {
        bool uRnotFastest = false;
        
        // pass the data to other scripts so they can update it
        foreach (ISave dataPersistanceObj in _dataSaves)
        {
            dataPersistanceObj.SaveData(ref _gameDatas, ref _currentRunData);
        }

        // save that data to a file using the data handler
        _dataHandler.Save(_gameDatas);

        //Check for name
        for (int i = 0; i < _gameDatas.Length; i++)
        {
            if (_gameDatas[i].name == _currentRunData.name)
            {
                if (_gameDatas[i].time < _currentRunData.time)
                {
                    uRnotFastest = true;
                }
            }
        }

        if (!uRnotFastest)
        {
            //playfabManager.SendLeaderboard(_simplifyGameData);
            
            Leaderboards.test.UploadNewEntry(_currentRunData.name, _currentRunData.time, isSuccessful =>
            {
                if (isSuccessful)
                {
                    Debug.Log("Uploaded");
                    LoadGame();
                }
                else
                {
                    Debug.LogWarning("Failed to upload");
                }
            });
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
