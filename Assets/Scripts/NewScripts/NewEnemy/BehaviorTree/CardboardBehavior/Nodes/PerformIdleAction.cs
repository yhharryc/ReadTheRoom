using UnityEngine;
using Unity.Behavior;
using Unity.Properties;
using System;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PerformIdleAction", story: "[Agent] performs their idle action with index [IndexNum]", category: "Action", id: "e7dbfceaeec38b0a0b5a799f9f93a922")]
public partial class PerformIdleAction : Unity.Behavior.Action
{
    private EnemyManager enemyManager;
    [SerializeReference] public BlackboardVariable<int> IndexNum;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    protected override Status OnStart()
    {
        enemyManager = Agent.Value.GetComponent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager component missing!");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return enemyManager.OnIdleBehavior(IndexNum.Value);
    }

    protected override void OnEnd()
    {
        // Optional cleanup if needed
    }
}
