using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipHorizontalState : PlayerAbilityState
{
    public PlayerFlipHorizontalState(Player player, PlayerStateMachine stateMachine, PlayerControlData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseFlipInput();
        player.FlipHorizontal();
        isAbilityDone = true;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
