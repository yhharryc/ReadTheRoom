using UnityEngine;

public class PlayerStaggerOrKnockbackNode : IEventNode<EventContext>
{
    public int Priority => 40; // After applying damage

    public void Process(EventContext context)
    {
        if (context == null || context.Target == null) return;

        // var playerChar = context.Target.Owner.GetComponent<PlayerCharacter>();
        // if (playerChar == null) return;

        // 1) Check if the AttackData indicates push/stagger
        // e.g. context.AttackData.PushType or context.AttackData.PushStagger
        // if (context.AttackData.PushType > 0) { ... }

        // 2) Possibly call a method on the PlayerCharacter that sets "IsStaggered" 
        //    or triggers a short "knockback" animation

        //TODO: implement logic to apply a push or stagger state to the player
    }
}
