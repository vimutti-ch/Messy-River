[System.Serializable]
public class DataWrapper
{
    public int[] time;
    public string[] name;
    
    public DataWrapper(int size)
    {
        time = new int[size];
        name = new string[size];
    }
}
