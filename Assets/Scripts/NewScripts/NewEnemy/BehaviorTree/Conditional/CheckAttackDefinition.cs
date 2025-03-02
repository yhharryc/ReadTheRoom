using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Condition")]
public class CheckAttackDefinition : Conditional
{
    // The AttackDefinition to check
    public AttackDefinition attackDef; 
    // We assume you have a custom SharedVariable type or a reference
    // Or you store it in your blackboard

    // A reference to the context (distance, LOS) 
    // Or you compute them in OnUpdate
    public SharedGameObject self;
    public SharedGameObject target;

    // Possibly we store these as shared floats
    public SharedFloat distanceToTarget;
    public SharedBool hasLineOfSight;

    public override TaskStatus OnUpdate()
    {
        if (attackDef == null) return TaskStatus.Failure;

        // Build the context
        var ctx = new AttackContext {
            self = self.Value,
            target = target.Value,
            distanceToTarget = distanceToTarget.Value,
            hasLineOfSight = hasLineOfSight.Value,
        };

        bool canAttack = attackDef.CanAttack(ctx);
        return canAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}
