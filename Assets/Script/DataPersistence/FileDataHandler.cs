using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = ""; //Where to save - Data Directory Path
    private string dataFileName = ""; //Name of the save file

    public FileDataHandler(string dataDirPath, string dataFileName) //Class Constructor
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData[] Load()
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath,dataFileName); // Alternative: dataDirPath + "/" + dataFileName
        // GameData[] loadedData = new GameData[5];
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                // Deserialize the data from Json back into the C# object
                DataWrapper loadedData = JsonUtility.FromJson<DataWrapper>(dataToLoad);
                
                // Convert the loaded data into the GameData array
                GameData[] gameData = new GameData[loadedData.name.Length];
                
                for (int i = 0; i < loadedData.name.Length; i++)
                {
                    gameData[i] = new GameData();
                    gameData[i].time = loadedData.time[i];
                    gameData[i].name = loadedData.name[i];
                }
                
                Debug.Log("Load Complete");
                return gameData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from file: {fullPath} \n {e}");
            }
        }

        return null;
    }

    public void Save(GameData[] data)
    {
        int size = data.Length;
        
        DataWrapper dataWrapper = new DataWrapper(size);
        
        for (int i = 0; i < size; i++)
        {
            dataWrapper.time[i] = data[i].time;
            dataWrapper.name[i] = data[i].name;
        }
        
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath,dataFileName); // Alternative: dataDirPath + "/" + dataFileName

        try
        {
            // Create the directory of file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            // Serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(dataWrapper, true);
            
            // Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to save data to file: {fullPath} \n {e}");
        }
    }
}
