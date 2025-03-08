using UnityEngine;

/// <summary>

/// </summary>
public class EnemyStateBehaviorStagger : StateMachineBehaviour
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
                Debug.LogWarning($"[EnemyStateBehaviorStagger] Could not find EnemyManager in parent of {animator.gameObject.name}.");
            }
        }
    }

    // Called when the state machine transitions *out* of this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemyManager.IsKnockedDown)
        {

        }
    }
}
