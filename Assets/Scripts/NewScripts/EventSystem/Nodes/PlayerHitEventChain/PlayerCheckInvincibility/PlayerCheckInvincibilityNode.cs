using UnityEngine;

public class PlayerCheckInvincibilityNode : IEventNode<EventContext>
{
    public int Priority => 10; // Runs early in the chain

    public void Process(EventContext context)
    {
        // If no context or the Source/Target is missing, bail out
        if (context == null || context.Target == null) return;
        PlayerCharacter player = context.Target.Owner.GetComponent<PlayerCharacter>();
        if (player != null && player.IsGuarding) 
        {
            context.ShouldContinue = false;
            //Debug.LogError("Happened!");
            //TODO: Fill in actual logic for invincibility states
            
        }
    
        // 1) Try to see if the player has some “isInvincible” or “hasIFrames” property
        //    Typically you'd cast context.Target.Owner to a PlayerCharacter
        //    or a player manager script to check that property.

        // Example:
        // var player = context.Target.Owner.GetComponent<PlayerCharacter>();
        // if (player != null && player.IsInvincible)
        // {
        //     // Mark the chain to stop
        //     context.ShouldContinue = false;
        //     Debug.Log("[PlayerCheckInvincibilityNode] Player is invincible, skipping further hit logic.");
        // }

        
    }
}
