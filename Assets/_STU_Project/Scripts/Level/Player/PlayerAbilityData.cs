using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerAbilityData", menuName = "Data/Player Data/PlayerAbilityData")]
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
