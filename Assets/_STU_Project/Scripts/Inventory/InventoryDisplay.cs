using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

    public InventorySystem InventorySystem
    {
        get { return inventorySystem; }
    }

    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary
    {
        get { return slotDictionary; }
    }
    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updateSlot)
            {
                slot.Key.UpdateUISlot(updateSlot);
            }
        }
    }

    public void SlotDragBegin(InventorySlot_UI clickedUISlot)
    {
        //如果當前格子有東西並且滑鼠上沒東西 (拿起)
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            mouseInventoryItem.oriUISlot = clickedUISlot;
            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            clickedUISlot.ClearSlot();
            return;
        }
    }

    public void SlotDragEnd(InventorySlot_UI clickedUISlot)
    {
        if (mouseInventoryItem.AssignedInventorySlot.ItemData == null) return;

        //如果格子是空的並且滑鼠上有東西 (放入)
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
        }

        //如果格子上有東西並且滑鼠上也有東西
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            //如果東西相同
            if (isSameItem)
            {
                bool isOverFlow = clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack);

                //且加入時堆疊不會溢出
                if (isOverFlow)
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    clickedUISlot.UpdateUISlot();

                    mouseInventoryItem.ClearSlot();
                }
                else //堆疊溢出
                {
                    //如果那個堆疊是滿的
                    if (leftInStack < 1)
                        SwapSlots(clickedUISlot);
                    else //堆疊不是滿的
                    {
                        int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                        clickedUISlot.UpdateUISlot();

                        var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                        mouseInventoryItem.oriUISlot.AssignedInventorySlot.AssignItem(newItem);
                        mouseInventoryItem.oriUISlot.UpdateUISlot();
                        mouseInventoryItem.ClearSlot();
                    }
                }

            }
            //如果東西不同直接交換
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.oriUISlot.AssignedInventorySlot.AssignItem(clickedUISlot.AssignedInventorySlot);
        mouseInventoryItem.oriUISlot.UpdateUISlot();
        mouseInventoryItem.ClearSlot();

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
