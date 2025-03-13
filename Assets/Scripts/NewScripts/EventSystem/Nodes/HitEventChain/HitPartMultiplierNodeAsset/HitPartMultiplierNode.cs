using UnityEngine;

public class HitPartMultiplierNode : IEventNode<EventContext>
{
    public int Priority => 20;

    public float WeakPointMultiplier { get; set; } = 0.5f;
    public float ArmoredMultiplier   { get; set; } = 0.0f;

    public void Process(EventContext context)
    {
        if (context == null || context.HitData == null) return;

        float multiplier = context.AttackData.DamageMultiplier;

        // Check if target is an EnemyHitCollider
        var hitCollider = context.Target as EnemyHitCollider;
        var partType = (hitCollider != null) ? hitCollider.partType : HitPartType.Normal;
        EnemyManager enemyManager = context.Target.Owner.GetComponent<EnemyManager>();
        switch (partType)
        {
            case HitPartType.WeakPoint:
                multiplier = WeakPointMultiplier;
                break;
            case HitPartType.Armored:
                multiplier = ArmoredMultiplier;
                break;
            default:
                if (enemyManager!=null && enemyManager.IsKnockedDown)
                multiplier = WeakPointMultiplier;
                break;
        }
        context.AttackData.DamageMultiplier = multiplier;
        //context.HitData.FinalDamage = finalDamage;
        //Debug.Log($"[HitPartMultiplierNode] part={partType}, newDamage={finalDamage}");
    }
}
