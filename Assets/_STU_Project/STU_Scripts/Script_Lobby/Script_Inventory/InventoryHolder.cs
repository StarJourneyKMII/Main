using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem
    {
        get { return inventorySystem; }
    }

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;

        inventorySystem = new InventorySystem(inventorySize);
    }

    protected abstract void LoadInventory(SaveData saveData);
}


[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public Vector3 Positon;
    public Quaternion Rotation;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _positon, Quaternion _rotation)
    {
        InvSystem = _invSystem;
        Positon = _positon;
        Rotation = _rotation;
    }
    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
        Positon = Vector3.zero;
        Rotation = Quaternion.identity;
    }
}
