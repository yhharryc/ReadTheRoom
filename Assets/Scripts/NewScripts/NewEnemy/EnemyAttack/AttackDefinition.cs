using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The base abstract class for all attacks. 
/// It provides a skeleton for performing an attack, 
/// but the actual logic is implemented in derived classes (Melee, Projectile, etc.).
/// </summary>
public abstract class AttackDefinition : ScriptableObject
{
    public string attackName;

    [Header("Damage")]
    [Tooltip("A multiplier applied to the base damage or other calculations.")]
    public float damageMultiplier = 1f;

    [Header("Animation / Motion")]
    [Tooltip("Reference to a Motion or AnimationClip that can be swapped into an Animator Override Controller.")]
    public Motion animationMotion;

    [Header("Conditions to Satisfy")]
    [SerializeField]
    private List<BaseConditionAsset> conditionObjects;

    /// <summary>
    /// Checks if all conditions are satisfied for this attack.
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

    /// <summary>
    /// Perform the attack from source to target. Implementation 
    /// will vary in derived classes (e.g. Melee vs Projectile).
    /// </summary>
    /// <param name="source">The character performing the attack.</param>
    /// <param name="target">The character being targeted.</param>
    public abstract void PerformAttack(ICharacter source, ICharacter target);
}
