using UnityEngine;

public class HitPartMultiplierNode : IEventNode<EventContext>
{
    public int Priority => 20;

    public float WeakPointMultiplier { get; set; } = 1.5f;
    public float ArmoredMultiplier   { get; set; } = 0.3f;

    public void Process(EventContext context)
    {
        if (context == null || context.HitData == null) return;

        float finalDamage = context.HitData.FinalDamage;

        // Check if target is an EnemyHitCollider
        var hitCollider = context.Target as EnemyHitCollider;
        var partType = (hitCollider != null) ? hitCollider.partType : HitPartType.Normal;

        switch (partType)
        {
            case HitPartType.WeakPoint:
                finalDamage *= WeakPointMultiplier;
                break;
            case HitPartType.Armored:
                finalDamage *= ArmoredMultiplier;
                break;
        }

        context.HitData.FinalDamage = finalDamage;
        Debug.Log($"[HitPartMultiplierNode] part={partType}, newDamage={finalDamage}");
    }
}
