using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Item info </summary>
public class ItemManager
{
    static ItemManager _instance = null;
    public static ItemManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
                _instance.LoadAllItem();
            }
            return _instance;
        }
    }
    
    /// <summary> All Item </summary>
    ItemData[] allItem = new ItemData[0];
    /// <summary> Download all item info in here from HD </summary>
    void LoadAllItem()
    {
        // Auto download all format for item data 自動下載所有格式為ItemData的資料
        allItem = Resources.LoadAll<ItemData>("");
    }

    //----------------------------------

    /// <summary> Use id to call out item data </summary>
    public ItemData GetItemDataByID(int id)
    {
        // List<> Count
        // int[] Length
        for (int i = 0; i < allItem.Length; i++)
        {
            Debug.Log(allItem.Length);
            // Find the same id's stuff from all data
            if (allItem[i].ID == id)
                return allItem[i];
        }
        return null;
    }
}