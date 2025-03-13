using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackDefinition", menuName = "MyGame/AI/AttackDefinition")]
public class AttackDefinition : ScriptableObject
{
    public string attackName;

    [Header("Damage")]
    [Tooltip("A multiplier applied to the base damage or other calculations.")]
    public float damageMultiplier = 1f;

    [Header("Animation / Motion")]
    [Tooltip("Reference to a Motion or AnimationClip that can be swapped into an Animator Override Controller.")]
    public Motion animationMotion;
    // Alternatively, you could store an AnimationClip:
    // public AnimationClip animationClip;

    [Header("Conditions to Satisfy")]
    [SerializeField]
    private List<BaseConditionAsset> conditionObjects;


    /// <summary>
    /// Evaluates whether all the attached IAttackCondition objects pass.
    /// </summary>
    public bool CanAttack(AttackContext context)
    {
        if (conditionObjects == null || conditionObjects.Count == 0) return true;

        foreach (var obj in conditionObjects)
        {
            if (obj is IAttackCondition condition)
            {
                if (!condition.Evaluate(context))
                    return false; 
            }
        }
        return true;
    }

    public virtual void PerformAttack(ICharacter source, ICharacter target)
    {
        
        // 1) Safety checks
        if (source == null) {
            Debug.LogWarning($"[AttackDefinition] PerformAttack called with null source.");
            return;
        }
        if (target == null) {
            Debug.LogWarning($"[AttackDefinition] PerformAttack called with null target.");
            return;
        }

        // 2) Obtain or define a base damage. 
        //    Here we show a simple placeholder of 10f, 
        //    but you could read from source's ability system or attribute set:
        //      float baseDamage = source.GetAbilitySystemComponent().AttributeSet.Damage.CurrentValue;
        float baseDamage = 10f; 
        float finalDamage = baseDamage * damageMultiplier;

        // 3) Build the EventContext 
        var ctx = new EventContext {
            Source = source,
            Target = target as IHitReceiver,  // Must cast to IHitReceiver
            AttackData = new AttackData {
                BaseDamage = finalDamage,
                // Optionally fill in other fields (AmmoType, PushType, etc.)
            },
            HitData = new HitData {
                // FinalDamage often computed later in the chain, 
                // but we can set 0f as placeholder or mirror AttackData.
                FinalDamage = finalDamage,
            }
        };

        // 4) Dispatch to the AttackEventChain (if you have a manager or global chain).
        //    Typically you have something like:
        //      EventChainManager.Instance.ExecuteAttackChain(ref ctx);
        //    Adjust as needed for your own chain system.
        //EventChainManager.Instance.ExecuteAttackChain(ref ctx);
        EventChainManager.Instance.ExecutePlayerHitChain(ref ctx);
        //target.TakeDamage(ctx);
        // After this point, the chain of IEventNode will process the context
        // (applying final damage, triggering VFX, etc.)
    }

}
