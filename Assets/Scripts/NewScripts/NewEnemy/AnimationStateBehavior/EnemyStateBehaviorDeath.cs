using UnityEngine;


public class EnemyStateBehaviorDeath : StateMachineBehaviour
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
                Debug.LogWarning($"[EnemyStateBehaviorDeath] Could not find EnemyManager in parent of {animator.gameObject.name}.");
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If the animation is non-looping, normalizedTime will go from 0 to 1 across its duration.
        // Check if it's reached (or passed) the end.
        if (stateInfo.normalizedTime >= 1f)
        {
            // This means the animation clip has effectively finished its playback.
            if (enemyManager != null)
            {
                enemyManager.OnDeathStateEnded();
            }
            // Optionally: Force the state machine to remain in this state or do something else.
            // Or set a boolean so this logic only triggers once.
        }
    }
}
