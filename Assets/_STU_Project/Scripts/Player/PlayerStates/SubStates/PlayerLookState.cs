using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookState : PlayerAbilityState
{
    private Vector2 lookInput;
    private float lookInputX;
    private float lookInputY;

    public PlayerLookState(Player player, PlayerStateMachine stateMachine, PlayerControlData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        lookInput = player.InputHandler.RawLookInput;
        lookInputX = player.InputHandler.NormLookInputX;
        lookInputY = player.InputHandler.NormLookInputY;

        player.SetLookInput(GetLookInputDistacne());
        if (lookInput == Vector2.zero)
        {
            isAbilityDone = true;
        }
    }


    public Vector3 GetLookInputDistacne()
    {
        float x = 0;
        if(lookInputX > 0)
            x = playerData.lookDistanceX.y;
        else if(lookInputX < 0)
            x = playerData.lookDistanceX.x;

        float y = 0;
        if (lookInputY > 0)
            y = (player.CurrentSex == 1 ? playerData.lookDistanceY.y : -playerData.lookDistanceY.x);
        else if (lookInputY < 0)
            y = (player.CurrentSex == 1 ? playerData.lookDistanceY.x : -playerData.lookDistanceY.y);

        return new Vector3(x, y, 0);
    }
}
