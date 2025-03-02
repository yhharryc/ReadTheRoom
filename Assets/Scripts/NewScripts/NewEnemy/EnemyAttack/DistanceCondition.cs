using UnityEngine;

[CreateAssetMenu(fileName = "DistanceCondition", menuName = "MyGame/AI/Conditions/DistanceCondition")]
public class DistanceCondition : ScriptableObject, IAttackCondition
{
    [SerializeField] private float requiredDistance = 2f;

    public bool Evaluate(AttackContext context)
    {
        return context.distanceToTarget <= requiredDistance;
    }
}