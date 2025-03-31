using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkSubState : BaseState
{
    
    PlayerCharacter player;
    private MovementParentState parentState;

    private GameplayEffectSpecHandle walkDebuffHandle;
    public WalkSubState(IStateMachineEntity owner, StateMachine stateMachine, MovementParentState parent)
        : base(owner, stateMachine)
    {
        parentState = parent;
        player = owner as PlayerCharacter;
    }

    public override void Enter()
    {
        base.Enter();
        
        player.CanActivate = true;
        Debug.Log("Enter WalkSubState");
        //RemoveEffectSpec(GameplayEffectSpecHandle handle)
        //walkDebuffHandle = new GameplayEffectSpecHandle();
        
        
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // 1) Check if left click input is "Performed" 
        //    (meaning it's actively held down)
        var leftClickAction = player.PlayerInputAction.actions["Confirm"]; 
        // We'll define a helper below to get the action by name

        if (leftClickAction != null && leftClickAction.phase == InputActionPhase.Performed)
        {
            // 2) If we have an activatable item, call HoldUse each frame
            IActivatable item = player.GetCurrentActivatable();
            if (item != null && player.CanActivate)
            {
                item.HoldUse(player, ActivationTrigger.LeftMouse);
            }
        }

        // [Optionally] do other logic for sub‐state...
        // e.g. handle shift -> sprint, etc.
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if(CombatManager.Instance.IsCombatStarted &&!player.IsActorTurn())
        {
            //TODO: Add Readability Feedback to failed movement action
            return;
        }
        // 1) Possibly transition to SprintSubState if sprint is allowed
        if (player.SprintInput && player.MoveInput.y > 0f && player.SprintTimer <= 0f)
        {
            parentState.SetSubState(parentState.SprintSubState);
        }

        // 2) Compute movement direction
        float finalSpeed = player.MoveSpeed;
        if (player.MoveInput.y < 0f) {
            finalSpeed *= 0.5f; // half speed if going backward
        }

        Vector3 forward = player.GetCameraYawForward() * player.MoveInput.y;
        Vector3 right   = player.GetCameraYawRight()   * player.MoveInput.x;
        Vector3 movementDir   = (forward + right).normalized * finalSpeed * Time.fixedDeltaTime;
        Vector3 finalMovement = player.ApplyGravity(movementDir);

        // 3) Check if we are actually trying to move
        bool isTryingToMove = (movementDir.magnitude > 0f);

        if (player.characterController && isTryingToMove)
        {
            // 4) Check resource cost (stamina) only if the player's turn is not complete
            float stamina = player.AbilitySystemComponent.GetAttributeValue("Stamina", out bool foundAttr);
            float cost    = player.AbilitySystemComponent.GetAttributeValue("MovementCost", out bool foundCostAttr);

            bool canMoveThisFrame = false;

            if (!CombatManager.Instance.IsCombatStarted)
            {
                // If the player's turn is "complete," no cost is consumed
                canMoveThisFrame = true;
            }
            else
            {
                // Otherwise, check stamina >= cost
                if (stamina >= cost)
                {
                    canMoveThisFrame = true;
                    // Apply the cost effect if actually moving
                    player.AbilitySystemComponent.ApplyEffectToSelf(StateConfig.Instance.MovementCostEffect, 1f);
                }
            }

            // 5) Actually move if allowed
            if (canMoveThisFrame)
            {
                player.characterController.Move(finalMovement);

                // 6) Ensure the walk debuff is applied if we are moving
                if (walkDebuffHandle.HandleID == 0)
                {
                    walkDebuffHandle = player.AbilitySystemComponent
                        .ApplyEffectToSelf(StateConfig.Instance.WalkDebuffEffect, 1f);
                }
            }
            else
            {
                // If not enough stamina, or we decided not to move for some other reason,
                // you could do partial movement, or do nothing, or log feedback.
            }
        }
        else
        {
            // 7) If not moving, remove the walk debuff effect (if that’s desired)
            if (player.IsTurnComplete && walkDebuffHandle.HandleID != 0)
            {
                player.AbilitySystemComponent.RemoveEffectSpec(walkDebuffHandle);
                walkDebuffHandle = new GameplayEffectSpecHandle();
            }
        }
    }


    public override void Exit()
    {
        base.Exit();
        player.CanActivate = false;
        Debug.Log("Exit WalkSubState");

        // If we want to remove the walkDebuff on exit:
        if (walkDebuffHandle.HandleID != 0)
        {
            player.AbilitySystemComponent.RemoveEffectSpec(walkDebuffHandle);
            walkDebuffHandle = new GameplayEffectSpecHandle();
        }
    }

    // ----------------------------------------------------------------------
    // LEFT-CLICK OVERRIDES
    // ----------------------------------------------------------------------
    public override void OnLeftClickStarted()
    {
        base.OnLeftClickStarted();

        IActivatable item = player.GetCurrentActivatable(); 
        if (item != null && player.CanActivate)
        {
            //if(CombatManager.Instance.IsCombatStarted && !TurnManager.Instance.IsActorTurn(player))

            item.BeginUse(player, ActivationTrigger.LeftMouse);
            if((CombatManager.Instance.IsCombatStarted && TurnManager.Instance.IsActorTurn(player) )&& !player.TimedTurnComponent.TimerActive)
            {
                player.BeginTimedTurn();
            }
        }
        else
        {
            Debug.Log("No item to fire.");
        }
    }

    public override void OnLeftClickCanceled()
    {
        base.OnLeftClickCanceled();

        IActivatable item = player.GetCurrentActivatable();
        if (item != null)
        {
            item.EndUse(player, ActivationTrigger.LeftMouse);
        }
    }
}
