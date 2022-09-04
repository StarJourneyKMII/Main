using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public PlayerAbilityData abilityData;

    private void Start()
    {
        ResetAbility();
    }

    public void UnLockAbility(PlayerAbilityType abilityType)
    {
        if (abilityType == PlayerAbilityType.Move)
            abilityData.move = true;
        else if (abilityType == PlayerAbilityType.Jump)
            abilityData.jump = true;
        else if (abilityType == PlayerAbilityType.DoubleJump)
            abilityData.doubleJump = true;
        else if (abilityType == PlayerAbilityType.Flip)
            abilityData.flip = true;
    }

    public void ResetAbility()
    {
        abilityData.move = true;
        //abilityData.jump = false;
        //abilityData.doubleJump = false;
        //abilityData.flip = false;
        abilityData.jump = true;
        abilityData.doubleJump = true;
        abilityData.flip = true;
    }
}
