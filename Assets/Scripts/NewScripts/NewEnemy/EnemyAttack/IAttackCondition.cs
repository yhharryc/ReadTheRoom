
using UnityEngine;

public class AttackContext
{
    public GameObject self;      // The enemy gameObject
    public GameObject target;    // The target (e.g., player)
    public float distanceToTarget;
    public bool hasLineOfSight;

}

public interface IAttackCondition
{
    /// <summary>
    /// Evaluates whether this condition is satisfied, given the context.
    /// </summary>
    bool Evaluate(AttackContext context);
}

