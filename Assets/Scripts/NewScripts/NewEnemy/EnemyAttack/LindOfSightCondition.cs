using UnityEngine;

[CreateAssetMenu(fileName = "LOSCondition", menuName = "MyGame/AI/Conditions/LOSCondition")]
public class LOSCondition : BaseConditionAsset
{
    public override bool Evaluate(AttackContext context)
    {
        // We rely on 'context.hasLineOfSight' which might be set from a sensor
        return context.hasLineOfSight;
    }
}