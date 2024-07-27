using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dan.Main;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useTryCatch;
    [SerializeField] private int maxDataSize = 5; 
    
    private GameData[] _gameDatas;
    private GameData[] _globalGameDatas;
    private GameData _currentRunData;
    private List<ILoad> _dataLoads; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private List<ISave> _dataSaves; //ตัวเก็บ Obj ทุก ๆ ชิ้นที่มีการใช้งาน IDataPersistance
    private FileDataHandler _dataHandler;

    public GameData[] GameDatas => _gameDatas;

    private GameData[] _onlineDatas;

    private void Awake()
    {
        _gameDatas = new GameData[maxDataSize];
        _globalGameDatas = new GameData[maxDataSize];
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
        
        #region Load Local Data
        // load any saved data from a file using the data handler
        this._gameDatas = _dataHandler.Load();
        if (_gameDatas != null)
        {
            Debug.Log("File Load Done");

            foreach (ILoad dataPersistenceObj in _dataLoads)
            {
                dataPersistenceObj.LoadData(_gameDatas);
            }
            Debug.Log("Local data loaded");
        }
        
        // if no data can be Loaded, initialize to a new game
        else //(this._gameDatas == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        #endregion
        
        #region Load Global Data
        Leaderboards.test.GetEntries(entries =>
        {
            int length = Mathf.Min(_globalGameDatas.Length, entries.Length);
            this._globalGameDatas = new GameData[length];
            
            Debug.Log($"<color=#caf179ff> Global </color> Data Length : {length}");
            if (length == 0)
            {
                Debug.LogWarning("No data was found. Skip loading Global Data");
                return;
            }
            
            for (int i = 0; i < length; i++)
            {
                _globalGameDatas[i] = new GameData();
                _globalGameDatas[i].name = entries[i].Username;
                _globalGameDatas[i].time = entries[i].Score;
            }
            
            if(_globalGameDatas == null) return;

            // push the loaded data to all other scripts that need it
            foreach (ILoad dataPersistanceObj in _dataLoads)
            {
                dataPersistanceObj.LoadDataGlobal(_globalGameDatas);
                Debug.Log("Load<color=#caf179ff> Global </color>Data");
            }
        
            Debug.Log("<color=#caf179ff> Global </color> data loaded");
        });
        #endregion
        
        Debug.Log("Exit Data load call");
    }

    public void SaveGame()
    {
        bool uRnotFastest = false;
        
        // pass the data to other scripts so they can update it
        foreach (ISave dataPersistanceObj in _dataSaves)
        {
            dataPersistanceObj.SaveData(_gameDatas, out _gameDatas, out _currentRunData);
        }

        if(_currentRunData.time < 0) return;
        
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
