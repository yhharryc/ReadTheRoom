using UnityEngine;


public class EnemyStateBehaviorStumble : StateMachineBehaviour
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
                Debug.LogWarning($"[EnemyStateBehaviorStumble] Could not find EnemyManager in parent of {animator.gameObject.name}.");
            }
        }
    }

    // Called when the state machine transitions *out* of this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Trigger the “Stumble” logic on the manager
        if (enemyManager != null)
        {
            enemyManager.RecoverFromKnockDown();
        }
    }
}
