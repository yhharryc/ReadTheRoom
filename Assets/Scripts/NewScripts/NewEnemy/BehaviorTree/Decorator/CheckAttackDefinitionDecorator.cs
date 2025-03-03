using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[BehaviorDesigner.Runtime.Tasks.TaskCategory("MyAI/Decorators")]
[BehaviorDesigner.Runtime.Tasks.TaskDescription("A decorator that re-evaluates AttackDefinition.CanAttack. If it fails, the child is aborted.")]
public class CheckAttackDefinitionDecorator : Decorator
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Which AttackDefinition to check.")]
    public SharedAttackDefinition attackDef;

    // Typically, you'd store these in your blackboard as Shared Variables
    public SharedGameObject self;
    public SharedGameObject target;
    public SharedFloat distanceToTarget;
    public SharedBool hasLineOfSight;

    [UnityEngine.Tooltip("Time interval (seconds) for re-checking the condition. 0 => re-check every frame.")]
    public float recheckInterval = 0f;

    private float nextCheckTime = 0f;

    // --------------------------------------------------------
    // Called when the decorator starts. We'll schedule 
    // our first re-check immediately.
    // --------------------------------------------------------
    public override void OnStart()
    {
        base.OnStart();
        nextCheckTime = Time.time; 
    }

    // --------------------------------------------------------
    // If CanReevaluate is true, Behavior Designer will keep 
    // calling OnUpdate while the child runs. 
    // --------------------------------------------------------
    public override bool CanReevaluate()
    {
        return true;
    }

    // --------------------------------------------------------
    // Called once to decide if the child can start. 
    // We do an immediate EvaluateCondition check here.
    // If it fails, we won't start the child at all.
    // --------------------------------------------------------
    public override bool CanExecute()
    {
        return EvaluateCondition();
    }

    // --------------------------------------------------------
    // OnUpdate is called *each frame* if CanReevaluate is true
    // while the child is running. We must return Success to 
    // keep the child running, or Failure to abort it.
    // --------------------------------------------------------
    public override TaskStatus OnUpdate()
    {
        // If we have a recheckInterval, only re-check after that time passes
        if (recheckInterval <= 0f || Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + recheckInterval;

            bool canContinue = EvaluateCondition();
            if (!canContinue)
            {
                // Return Failure => triggers an abort if the 
                // decorator is configured to “Abort Self” or “Abort Lower Priority”.
                return TaskStatus.Failure;
            }
        }

        // Condition still valid => keep child running
        return TaskStatus.Success;
    }

    // Evaluate the AttackDefinition logic
    private bool EvaluateCondition()
    {
        if (attackDef == null) return false;

        // Build an AttackContext to pass to the AttackDefinition
        var ctx = new AttackContext {
            self = self.Value,
            target = target.Value,
            distanceToTarget = distanceToTarget.Value,
            hasLineOfSight = hasLineOfSight.Value
        };

        // Return whether AttackDefinition says we can attack
        return attackDef.Value.CanAttack(ctx);
    }
}
