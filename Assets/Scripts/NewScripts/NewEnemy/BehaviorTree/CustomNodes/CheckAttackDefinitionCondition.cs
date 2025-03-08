using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Check Attack Definition", story: "[AttackDefinition] is Valid provided [Agent] [LineOfSight] and [DistanceToTarget] to [Target]", category: "Conditions", id: "51e4273b5248064c2118e140ad26f8cc")]
public partial class CheckAttackDefinitionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<AttackDefinition> AttackDefinition;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<bool> LineOfSight;
    [SerializeReference] public BlackboardVariable<float> DistanceToTarget;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        if (AttackDefinition == null) return false;

        AttackContext context = new AttackContext{
            self = Agent,
            target = Target,
            distanceToTarget = DistanceToTarget,
            hasLineOfSight = LineOfSight,
        };
        AttackDefinition definition = AttackDefinition.Value;
        return definition.CanAttack(context);
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
