using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState {

	private Vector2 lookInput;

	public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerControlData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
	}

	public override void DoChecks() {
		base.DoChecks();
	}

	public override void Enter() {
		base.Enter();
		Movement?.SetVelocityX(0f);
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		lookInput = player.InputHandler.RawLookInput;

		if (!isExitingState) {
			if (xInput != 0) {
				stateMachine.ChangeState(player.MoveState);
			} else if(lookInput != Vector2.zero)
            {
				stateMachine.ChangeState(player.LookState);
            }
		}

	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
