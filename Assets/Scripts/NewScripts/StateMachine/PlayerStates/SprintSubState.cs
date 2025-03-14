using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintSubState : BaseState
{
    private MovementParentState parentState;
    PlayerCharacter player;
    private GameplayEffectSpecHandle sprintEffectHandle;
    public SprintSubState(IStateMachineEntity owner, StateMachine stateMachine, MovementParentState parent)
        : base(owner, stateMachine)
    {
        parentState = parent;
        player = owner as PlayerCharacter;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter SprintSubState");
        // 播放冲刺开始动画等等
        player.CanActivate = false;
        if (sprintEffectHandle.HandleID == 0)
        {
            sprintEffectHandle = player.AbilitySystemComponent
                .ApplyEffectToSelf(StateConfig.Instance.SprintEffect, 1f);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        

        // 如果松开Shift，则回到Walk
        if (!player.SprintInput||player.MoveInput.y<=0f)
        {
            parentState.SetSubState(parentState.WalkSubState);
            return;
        }
                // The usual movement logic
        float finalSpeed = player.MoveSpeed;
        Vector3 forward = player.GetCameraYawForward() * player.MoveInput.y;
        Vector3 right   = player.GetCameraYawRight()   * player.MoveInput.x;
        Vector3 movementDir = (forward + right).normalized * finalSpeed* Time.fixedDeltaTime;
        Vector3 finalMovement =  player.ApplyGravity(movementDir);
        if (player.characterController && movementDir.magnitude > 0f)
        {
            
            player.characterController.Move(finalMovement);

        }
        // 或检测体力不足 => 退出冲刺
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        // 在此进行冲刺速度移动
        // float sprintSpeed = 6.0f or from player
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exit SprintSubState");

        if (sprintEffectHandle.HandleID != 0)
        {
            player.AbilitySystemComponent.RemoveEffectSpec(sprintEffectHandle);
            sprintEffectHandle = new GameplayEffectSpecHandle();
        }
        player.ResetSprintTimer();
    }

    public override void OnLeftClickStarted()
    {
        base.OnLeftClickStarted();

        IActivatable item = player.GetCurrentActivatable(); 
        if (item != null && player.CanActivate)
        {
            item.BeginUse(player, ActivationTrigger.LeftMouse);
        }
        else
        {
            Debug.Log("No item to fire.");
        }
        parentState.SetSubState(parentState.WalkSubState);
    }
}
