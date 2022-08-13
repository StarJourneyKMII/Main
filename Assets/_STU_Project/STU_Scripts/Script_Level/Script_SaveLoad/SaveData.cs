using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public SerializableDictionary<string, LevelSaveData> levelSaveData;
    public LevelSaveData currentLevelSaveData
    {
        get 
        {
            string currentLevelName = GameManager.instance.levelName;
            levelSaveData.TryGetValue(currentLevelName, out LevelSaveData levelData);
            if (levelData == null)
            {
                levelData = new LevelSaveData();
                levelSaveData.Add(currentLevelName, levelData);
            }
            return levelData;
        }
    }

    public InventorySaveData playerInventory;

    public SaveData()
    {
        levelSaveData = new SerializableDictionary<string, LevelSaveData>();
        playerInventory = new InventorySaveData();
    }
}
