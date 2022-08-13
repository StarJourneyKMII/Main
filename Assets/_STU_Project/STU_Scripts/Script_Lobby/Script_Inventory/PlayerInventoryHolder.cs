using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;

    public GameObject inventoryPanel;
    private bool isOpen;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(inventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.playerInventory.InvSystem != null)
        {
            this.inventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            SwitchInventoryPanel(isOpen);
        }
    }

    public void SwitchInventoryPanel(bool value)
    {
        inventoryPanel.SetActive(value);
    }
}
