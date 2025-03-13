using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttackDefinition", menuName = "MyGame/AI/MeleeAttackDefinition")]
public class MeleeAttackDefinition : AttackDefinition
{
    public override void PerformAttack(ICharacter source, ICharacter target)
    {
        // 1) Basic checks
        if (source == null) {
            Debug.LogWarning("[MeleeAttackDefinition] PerformAttack: source is null.");
            return;
        }
        if (target == null) {
            Debug.LogWarning("[MeleeAttackDefinition] PerformAttack: target is null.");
            return;
        }

        // 2) Suppose we read the baseDamage from the source's attribute set:
        //    or fallback to some default
        float baseDamage = 10f; // e.g., from source.GetAbilitySystemComponent().AttributeSet.Damage
        float finalDamage = baseDamage * damageMultiplier;

        // 3) Build the EventContext
        var ctx = new EventContext {
            Source = source,
            Target = target as IHitReceiver,
            AttackData = new AttackData {
                BaseDamage = finalDamage
                // We could add other fields, e.g. PushType if melee includes push
            },
            HitData = new HitData {
                FinalDamage = finalDamage
            }
        };

        // 4) Dispatch the event chain (or do direct logic).
        //    Adjust the method name as needed for your system.
        //    E.g., AttackChain vs. PlayerHitChain vs. general chain
        EventChainManager.Instance.ExecuteAttackChain(ref ctx);
    }
}
