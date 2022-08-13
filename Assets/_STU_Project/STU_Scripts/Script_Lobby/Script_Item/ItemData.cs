using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Item </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Add New Item")]
public class ItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    [TextArea(3, 3)] public string description;
    public Sprite icon;
    public int number;
    public int maxStackSize;

    public StackType stackType;
    public UsedType usedType;
    public CostType costType;
    public PartType partType;
    public int useLiftEffect; // +-
    public float useHpEffect; // +-
    // public float useDamageEffect;
    public float useSpeedEffect; // +-

    public void Reset() // init
    {
        // Load into default map
        ID = -1;
        icon = Resources.Load<Sprite>("NoImage");
        description = "這個物品沒有任何敘述！";
        maxStackSize = -1;
        stackType = StackType.NO;
        usedType = UsedType.NO;
        costType = CostType.NO;
        partType = PartType.NO;
    }
}
public enum StackType
{
    NO,
    YES
}
public enum UsedType
{
    NO,
    YES
}
public enum CostType
{
    NO,
    YES
}
public enum PartType
{
    NO,
    HEAD,
    HAND,
    BODY,
    FOOT
}