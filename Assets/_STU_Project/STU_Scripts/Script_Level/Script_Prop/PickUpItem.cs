using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class PickUpItem : MonoBehaviour
{
    public ItemData ItemData;
    public int StackCount = 1;

    private ItemPickUpSaveData itemSaveData;
    private string id;

    protected virtual void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);
    }
    protected virtual void Start()
    {
        SaveGameManager.data.currentLevelSaveData.activeItems.Add(id, itemSaveData);
    }
    private void LoadGame(SaveData data)
    {
        if(data.currentLevelSaveData.collectItems.Contains(id))
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if(SaveGameManager.data.currentLevelSaveData.activeItems.ContainsKey(id))
            SaveGameManager.data.currentLevelSaveData.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            var inventory = other.GetComponent<InventoryHolder>();

            if (inventory == null) return;
                
            if(inventory.InventorySystem.AddToInventory(ItemData, StackCount))
            {
                SaveGameManager.data.currentLevelSaveData.collectItems.Add(id);
                Destroy(gameObject);
            }
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public ItemData itemData;
    public Vector3 position;
    public Quaternion rotation;

    public ItemPickUpSaveData(ItemData _itemData, Vector3 _position, Quaternion _rotation)
    {
        itemData = _itemData;
        position = _position;
        rotation = _rotation;
    }
}
