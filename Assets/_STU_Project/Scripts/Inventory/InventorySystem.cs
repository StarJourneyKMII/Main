using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<InventorySlot> InventorySlots
    {
        get { return inventorySlots; }
    }

    public int InventorySize
    {
        get { return inventorySlots.Count; }
    }

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// 添加物品到背包中
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public bool AddToInventory(ItemData itemData, int amountToAdd)
    {
        int tempAdd = amountToAdd;
        if (ContainsItem(itemData, out List<InventorySlot> slot)) //是否有相同物件的格子 有的話加入堆疊沒滿的格子中
        {
            foreach (InventorySlot s in slot)
            {
                bool enoughRoomLeftInStack = s.EnoughRoomLeftInStack(amountToAdd, out int amountRemaining);
                if(enoughRoomLeftInStack == true)
                {
                    s.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(s);
                    return true;
                }
                else if (enoughRoomLeftInStack == false && amountRemaining > 0)
                {
                    tempAdd -= amountRemaining;
                    s.AddToStack(amountRemaining);
                    OnInventorySlotChanged?.Invoke(s);
                    break;
                }
            }
        }

        if (tempAdd <= 0) return true;

        //如果堆疊滿了
        if (HasFreeSlot(out InventorySlot freeSlot)) //是否還有空格子　有的話加入空格子
        {
            if (freeSlot.EnoughRoomLeftInStack(tempAdd))
            {
                freeSlot.UpdateInventorySlot(itemData, tempAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 找尋有裝此物品的格子並傳出
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool ContainsItem(ItemData itemData, out List<InventorySlot> slot)
    {
        slot = new List<InventorySlot>();
        foreach (InventorySlot s in InventorySlots)
        {
            if (s.ItemData == itemData)
                slot.Add(s);
        }
        return slot.Count > 0 ? true : false;
    }


    /// <summary>
    /// 找尋第一個空格子並傳出
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = null;
        foreach (InventorySlot s in InventorySlots)
        {
            if (s.ItemData == null)
            {
                freeSlot = s;
                break;
            }
        }
        return freeSlot == null ? false : true;
    }
}
