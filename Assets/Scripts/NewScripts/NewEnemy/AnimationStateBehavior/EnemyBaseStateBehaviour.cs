using UnityEngine;

public class EnemyBaseStateBehaviour : StateMachineBehaviour {
    protected EnemyManager enemyManager;
    protected Animator animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // Cache references if not done yet
        if (enemyManager == null) {
            enemyManager = animator.GetComponentInParent<EnemyManager>();
            // Alternatively: animator.GetComponent<EnemyManager>() if on the same object
        }
        this.animator = animator;
    }

    // Provide any common utility methods
    protected void NotifyBehaviorTree(string variableName, bool value) {
        // If your manager or a stored BehaviorTree is accessible, set that variable.
        // e.g. enemyManager.behaviorTree.SetVariableValue(variableName, value);
    }
}
