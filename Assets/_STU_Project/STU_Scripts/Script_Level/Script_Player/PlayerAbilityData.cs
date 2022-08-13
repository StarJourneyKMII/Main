using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerAbilityData", fileName = "new PlayerAbilityData", order = 0)]
public class PlayerAbilityData : ScriptableObject
{
    public bool move;
    public bool jump;
    public bool doubleJump;
    public bool flip;
}

public enum PlayerAbilityType
{
    Move, Jump, DoubleJump, Flip
}
