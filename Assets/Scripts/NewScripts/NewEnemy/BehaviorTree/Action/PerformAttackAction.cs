using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// An action that instructs the EnemyManager to perform an attack using the current AttackDefinition.
/// </summary>
[TaskCategory("MyAI/Actions")]
public class PerformAttackAction : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Reference to the current AttackDefinition (e.g. chosen earlier).")]
    public SharedAttackDefinition currentAttack;
    public SharedGameObject playerObject;

    private EnemyManager enemyManager;

    public override void OnStart()
    {
        // Cache the EnemyManager reference on the same GameObject
        enemyManager = GetComponent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("[PerformAttackAction] No EnemyManager found on this GameObject.");
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyManager == null)
        {
            return TaskStatus.Failure;
        }
        if (currentAttack.Value == null)
        {
            Debug.LogWarning("[PerformAttackAction] No AttackDefinition assigned.");
            return TaskStatus.Failure;
        }

       currentAttack.Value.PerformAttack(enemyManager,playerObject.Value.GetComponent<PlayerCharacter>());    

        // If the attack is instantaneous or we don't need to wait, 
        // we can return success immediately. Otherwise, you might 
        // do more logic to see if it completes.
        return TaskStatus.Success;
    }
}
