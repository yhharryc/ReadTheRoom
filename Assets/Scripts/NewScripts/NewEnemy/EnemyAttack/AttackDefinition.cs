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
    private List<ScriptableObject> conditionObjects;

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
}
