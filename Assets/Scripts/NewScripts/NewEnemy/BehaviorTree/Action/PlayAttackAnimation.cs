using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Actions")]
public class PlayAttackAnimation : Action
{
    private Animator animator;

    [Header("Attack Definition")]
    public SharedAttackDefinition currentAttack;

    [Header("Animator Settings")]
    public SharedString animationStateName;
    public int layerIndex = 0;
    public float crossFadeDuration = 0.1f;

    private int stateHash;
    private bool hasBegunAttack = false;

    public override void OnStart()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No Animator found.");
            return;
        }

        if (currentAttack.Value == null)
        {
            Debug.LogWarning("No AttackDefinition set.");
            return;
        }

        // Set the animation state name from AttackDefinition if needed
        // animationStateName.Value = currentAttack.Value.animatorStateName;

        // Calculate the hash for the animation state name
        stateHash = Animator.StringToHash(animationStateName.Value);

        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("AttackTrigger");
        
        hasBegunAttack = true;
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null || !hasBegunAttack)
            return TaskStatus.Failure;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);

        if (animator.IsInTransition(layerIndex)) {
            // Still transitioning, treat as Running
            return TaskStatus.Running;
        }

        if (currentState.shortNameHash == stateHash) {
            // If the state hasn’t fully played, Running; else Success
            return (currentState.normalizedTime >= 1f) 
                ? TaskStatus.Success 
                : TaskStatus.Running;
        }
        else {
            // Not in the animation, not transitioning, so maybe we do treat it as success
            return TaskStatus.Success;
        }

    }

    private void ResetAnimatorParameters()
    {
        animator.ResetTrigger("AttackTrigger");
        animator.SetBool("IsAttacking", false);
    }
}