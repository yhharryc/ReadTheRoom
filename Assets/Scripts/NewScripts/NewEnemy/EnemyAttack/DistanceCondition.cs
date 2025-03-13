using UnityEngine;

[CreateAssetMenu(fileName = "DistanceCondition", menuName = "MyGame/AI/Conditions/DistanceCondition")]
public class DistanceCondition : BaseConditionAsset
{
    [SerializeField] private float requiredDistance = 2f;

    public override bool Evaluate(AttackContext context)
    {
        return context.distanceToTarget <= requiredDistance;
    }
}