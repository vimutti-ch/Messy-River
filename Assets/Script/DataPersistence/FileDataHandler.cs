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

    public GameData Load()
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath,dataFileName); // Alternative: dataDirPath + "/" + dataFileName
        GameData loadedData = null;
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
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                
                Debug.Log("Load Complete");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from file: {fullPath} \n {e}");
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath,dataFileName); // Alternative: dataDirPath + "/" + dataFileName

        try
        {
            // Create the directory of file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            // Serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);
            
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
