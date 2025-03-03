using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Actions")]
[TaskDescription("Calculates the distance between 'self' and 'target' and stores it in 'storeDistance'.")]
public class GetDistanceToTarget : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The reference to this AI's own GameObject.")]
    public SharedGameObject self;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The target GameObject we want to measure distance to.")]
    public SharedGameObject target;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("Where to store the computed distance.")]
    public SharedFloat storeDistance;

    public override TaskStatus OnUpdate()
    {
        // If either is null, we fail the action
        if (self.Value == null || target.Value == null)
        {
            return TaskStatus.Failure;
        }

        // Calculate distance
        float dist = Vector3.Distance(self.Value.transform.position,
                                      target.Value.transform.position);

        // Store in the SharedFloat
        storeDistance.Value = dist;
        // Action succeeded
        return TaskStatus.Success;
    }
}
