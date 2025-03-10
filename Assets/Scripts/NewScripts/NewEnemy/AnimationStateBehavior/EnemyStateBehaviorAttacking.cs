using UnityEngine;

/// <summary>

/// </summary>
public class EnemyStateBehaviorAttacking : StateMachineBehaviour
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
                Debug.LogWarning($"[EnemyStateBehaviorAttacking] Could not find EnemyManager in parent of {animator.gameObject.name}.");
            }

            return;
        }
        enemyManager.IsAttacking = true;
        
    }

    // Called when the state machine transitions *out* of this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyManager.IsAttacking = false;
    }
}
