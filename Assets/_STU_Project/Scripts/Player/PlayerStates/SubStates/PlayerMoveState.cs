using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState {
	public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerControlData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
	}

	public override void DoChecks() {
		base.DoChecks();
	}

	public override void Enter() {
		base.Enter();
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		Movement?.CheckIfShouldFlip(xInput);

		Movement?.SetVelocityX(playerData.movementVelocity * xInput);

		player.SetMoveInputX(xInput * playerData.moveLookMaxDistanceX);

		if (!isExitingState) {
			if (xInput == 0) {
				stateMachine.ChangeState(player.IdleState);
			} 
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
