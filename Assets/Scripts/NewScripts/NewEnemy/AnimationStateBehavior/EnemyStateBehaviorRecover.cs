using UnityEngine;

/// <summary>
/// A simple StateMachineBehaviour that calls enemyManager.RecoverFromStagger() on exit.
/// Attach this to the appropriate state in the Animator (e.g. the Stagger/Recover state).
/// </summary>
public class EnemyStateBehaviorRecover : StateMachineBehaviour
{
    private EnemyManager enemyManager;

    // Called once when the state machine first enters this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Cache reference to EnemyManager if not already done
        if (enemyManager == null)
        {
            enemyManager = animator.GetComponentInParent<EnemyManager>();
            if (enemyManager == null)
            {
                Debug.LogWarning($"[EnemyStateBehaviorRecover] Could not find EnemyManager in parent of {animator.gameObject.name}.");
            }
        }
    }

    // Called when the state machine transitions *out* of this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Trigger the “recover” logic on the manager
        if (enemyManager != null)
        {
            enemyManager.RecoverFromStagger();
        }
    }
}
