[System.Serializable]

public class PlayerData
{
    public PlayerStats stats = new PlayerStats();
    public Resource resource = new Resource();
    public Wagon wagon = new Wagon();
    public float milesTravelled = 0;
}
