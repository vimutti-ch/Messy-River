[System.Serializable]
public class GameData
{
    public int time;
    public string name;

    // The values defined in this constructor will be the default values
    // The game starts with when there's no data to load
    // public GameData() // Default Value
    // {
    //     this.time = new int[5] {0,0,0,0,0};
    //     this.name = new string[5] {null, null, null, null, null};
    // }

    public GameData()
    {
        time = -1;
        name = null;
    }
}