using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeachPanelUnLockPlayerAbility : TeachPanel
{
    public PlayerAbilityType unLockType;

    protected override void Start()
    {
        base.Start();
        OnTouchEvent += UnLockAbility;
    }

    private void UnLockAbility()
    {
        PlayerAbility.Instance.UnLockAbility(unLockType);
    }
}
