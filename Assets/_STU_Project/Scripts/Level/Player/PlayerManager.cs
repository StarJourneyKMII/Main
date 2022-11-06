using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> User Data Manager Center </summary>
public class PlayerManager
{
    private static PlayerManager _instance = null;
    public static PlayerManager instance // singleton
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerManager();
            }
            return _instance;
        }
    }

    /// <summary> Item change </summary>
    public System.Action Act_ItemChange = null;

    /// <summary> primary data </summary>
    public PlayerData pData = new PlayerData();
    /// <summary> Load file </summary>
    public void LoadData()
    {
        string json = PlayerPrefs.GetString("PLAYER_DATA", "");
        if (json == "")
        {
            // New player data 
            pData = new PlayerData(1, 1, 1);
            // AddItem(0);
            LogAllStuff();
        }
        else
        {
            // old data
            pData = JsonUtility.FromJson<PlayerData>(json);
        }
    }
    /// <summary> Save </summary>
    public void Save()
    {
        // pData to JSON
        string json = JsonUtility.ToJson(pData);
        // Display result
        Debug.Log(json);
        // Data save to HD or to database
        PlayerPrefs.SetString("PLAYER_DATA", json);
    }

    // 1. Check the star if it in group
    public bool CheckStarInData(int starId, int starNumber)
    {
        for (int i = 0; i < pData.stuff.Count; i++)
        {
            if (pData.stuff[i].id == starId && pData.stuff[i].number == starNumber)
            {
                // Get the item to modify and put back
                Stuff tempStuff = pData.stuff[i];
                if (tempStuff.stuffState == StuffState.USED)
                    return true;
            }
        }
        return false;
    }

    // 2. Get the star to add in group
    public void AddStarInData(int starId, int starNumber)
    {
        Stuff temp = new Stuff(starId, starNumber, StuffState.USED);
        // Get data to modify and put back
        PlayerData tempPlayerData = pData;
        tempPlayerData.stuff.Add(temp);
        pData = tempPlayerData;

        // Send item change event
        if (Act_ItemChange != null)
        {
            Act_ItemChange.Invoke();
        }
    }
    
    /// <summary> Add item for player </summary>
    public void AddItem(int id)
    {
        Debug.Log(id);
        // Find database to get info
        ItemData itemData = ItemManager.instance.GetItemDataByID(id);
        bool isStack = false;
        if (itemData.stackType == StackType.YES)
        {
            for (int i = 0; i < pData.stuff.Count; i++)
            {
                if (pData.stuff[i].id == id)
                {
                    // Get the item to modify and put back
                    Stuff tempStuff = pData.stuff[i];
                    tempStuff.number += 1;
                    pData.stuff[i] = tempStuff;
                    isStack = true;
                }
            }
        }

        // 如果先前因為任何原因(本身就沒有道具、道具不可堆疊)造成沒有堆疊得現象 就直接新增一個道具
        if (isStack == false)
        {
            Stuff temp = new Stuff(id, 1, StuffState.SUCCESS);
            // Get data to modify and put back
            PlayerData tempPlayerData = pData;
            tempPlayerData.stuff.Add(temp);
            pData = tempPlayerData;
        }

        // Send item change event
        if (Act_ItemChange != null)
        {
            Act_ItemChange.Invoke();
        }
    }

    /// <summary> Debug my property </summary>
    public void LogAllStuff()
    {
        for (int i = 0; i < pData.stuff.Count; i++)
        {
            // Call out Item Data
            ItemData itemData = ItemManager.instance.GetItemDataByID(pData.stuff[i].id);
            Debug.Log(itemData.name + " X " + pData.stuff[i].number);
        }
    }
    /// <summary> Remove the part from equipment bar </summary>
    public int RemovePart(PartType partType)
    {
        // Check stuff
        for (int i = 0; i < pData.partStuff.Count; i++)
        {
            // Remove part stuff
            ItemData itemData = ItemManager.instance.GetItemDataByID(pData.partStuff[i].id);
            if (itemData.partType == partType)
            {
                pData.partStuff.RemoveAt(i);
                // Send item change event
                if (Act_ItemChange != null)
                {
                    Act_ItemChange.Invoke();
                }
                // return remove part id
                return itemData.ID;
            }
        }
        // -1 是意外值
        return -1;
    }
    /// <summary> Put on the equipment </summary>
    public void AddPart(int id)
    {
        Stuff newStuff = new Stuff();
        bool isFind = GetStuffByID(id, out newStuff);

        if (isFind)
        {
            pData.partStuff.Add(newStuff);

            if (Act_ItemChange != null)
            {
                Act_ItemChange.Invoke();
            }
        }
    }
    
    // out 送回修改過的數值(這個值原本可以是空值)
    // ref 送回修改過的數值(這個值本來必須要有內容)
    /// <summary> Use id to get the stuff how much from player </summary>
    public bool GetStuffByID(int id, out Stuff myStuff)
    {
        for (int i = 0; i < pData.stuff.Count; i++)
        {
            if (id == pData.stuff[i].id)
            {
                myStuff = pData.stuff[i];
                return true;
            }
        }
        myStuff = new Stuff();
        return false;
    }

}

[System.Serializable]
public struct PlayerData
{
    [SerializeField] public string nickName;
    [SerializeField] public int level;
    [SerializeField] public int exp;
    [SerializeField] public int hp;
    [SerializeField] public int maxHp;
    [SerializeField] public int life;
    [SerializeField] public int maxlife;
    [SerializeField] public int money;
    [SerializeField] public List<Stuff> stuff;
    [SerializeField] public List<Stuff> partStuff;
    public PlayerData(int hp, int maxhp, int money)
    {
        this.nickName = "無名" + Random.Range(100, 999);
        this.level = 1;
        this.exp = 0;
        this.hp = hp;
        this.maxHp = maxhp;
        this.life = 10;
        this.maxlife = 99;
        this.money = money;
        this.stuff = new List<Stuff>();
        this.partStuff = new List<Stuff>();
    }
}

/// <summary> Stuff Package </summary>
[System.Serializable]
public struct Stuff
{
    [SerializeField] public int id;
    [SerializeField] public int number;
    [SerializeField] public StuffState stuffState;
    public Stuff(int id, int number, StuffState stuffState)
    {
        this.id = id;
        this.number = number;
        this.stuffState = stuffState;
    }
}

public enum StuffState
{
    NO,
    USED,
    SUCCESS
}