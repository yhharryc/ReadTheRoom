using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetAttackGraphFromEnemy", story: "Get [AttackBehaviorGraph] from [Agent]", category: "Action", id: "5877d8faead931d8f40978e30c4991a6")]
public partial class GetAttackGraphFromEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<BehaviorGraph> AttackBehaviorGraph;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        EnemyManager enemy = Agent.Value.GetComponent<EnemyManager>();
        if(enemy==null)
        {
            Debug.LogError("No EnemyManager Found on Agent");
        }
        AttackBehaviorGraph.Value = enemy.AttackBehaviorGraph;
        if(AttackBehaviorGraph.Value == null) return Status.Failure;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

