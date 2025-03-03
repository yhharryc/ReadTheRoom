using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculationNode : IEventNode<EventContext>
{
    public int Priority => 10;

    private float minRandomFactor = 0.925f;
    private float maxRandomFactor = 1.075f;

    public void Process(EventContext context)
    {
        if (context == null || context.AttackInfo == null || context.HitData == null) return;

        // 1) Base damage
        float baseDamage = context.AttackInfo.BaseDamage;

        // 2) Random factor
        float randomFactor = UnityEngine.Random.Range(minRandomFactor, maxRandomFactor);
        float finalDamage = Mathf.Round(baseDamage * randomFactor);

        // 3) Determine hit part type (weak/armored/normal).
        // We'll cast the IHitReceiver to EnemyHitCollider to read partType:
        var hitCollider = context.Target as EnemyHitCollider;
        HitPartType partType = HitPartType.Normal;
        if (hitCollider != null) {
            partType = hitCollider.partType;
        }

        // 4) Multiply if weak point, reduce if armored, etc.
        switch (partType)
        {
            case HitPartType.WeakPoint:
                finalDamage *= 1.5f;
                break;
            case HitPartType.Armored:
                finalDamage *= 0.3f;
                break;
        }

        // 5) Assign finalDamage
        context.HitData.FinalDamage = finalDamage;

        // 6) Log the event
        // Owner is the root object (like EnemyManager). We'll use that for the target name.
        var targetName = context.Target?.Owner?.name ?? "Unknown";
        Debug.Log($"[DamageCalculationNode] {targetName} was hit at {partType}, finalDamage = {finalDamage}");

        // 7) Apply it to the target’s ICharacter
        ICharacter character = context.Target?.Owner.GetComponent<ICharacter>();
        character?.TakeDamage(context);

        // 8) Also call OnHit on the target
        context.Target?.OnHit(context);

        // 9) If the target is an enemy, show damage numbers
        if (character != null && character.Faction == Faction.ENEMY)
        {
            Vector3 hitPos = context.HitData.HitInfo.HitPoint;
            DamageNumberManager.Instance.ShowDamageNumber(
                hitPos,
                finalDamage,
                context.HitData.WasCrit,
                context
            );
        }
    }
}
