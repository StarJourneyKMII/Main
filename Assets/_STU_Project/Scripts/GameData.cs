

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, LevelData> levelsData;
    public PlayerData playerData;

    public LevelData CurrentLevelData
    {
        get 
        {
            levelsData.TryGetValue(NewGameManager.Instance.CurrentLevelName, out LevelData levelData);
            return levelData;
        }
    }

    public PlanetData PlanetData;


    public GameData()
    {
         levelsData = new SerializableDictionary<string, LevelData>();
        for(int i = 0; i < 15; i ++)
        {
            AddNewLevelData("STJ_Old_Level" + i, new LevelData());
        }
    }

    public void AddNewLevelData(string levelsName, LevelData levelData)
    {
        levelsData.Add(levelsName, levelData);
    }
}
