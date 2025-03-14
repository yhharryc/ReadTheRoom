using UnityEngine;

public class ApplyDamageNode : IEventNode<EventContext>
{
    public int Priority => 40;

    public void Process(EventContext context)
    {
        if (context == null || context.HitData == null || context.Target == null)
            return;
        context.HitData.FinalDamage = context.HitData.FinalDamage * (context.AttackData.DamageMultiplier+1f);
        // Get the ICharacter from the Target’s owner
        var character = context.Target.Owner.GetComponent<ICharacter>();
        if (character == null) return;

        // Pass the final damage via context. 
        // The character uses context.HitData.FinalDamage internally.
        character.TakeDamage(context);
        character.TakeStaggerDamage(context);
        //Debug.LogError("????????????");
        // Also call OnHit to let the IHitReceiver do any local logic
        //context.Target.Owner.OnHit(context);
    }
}
