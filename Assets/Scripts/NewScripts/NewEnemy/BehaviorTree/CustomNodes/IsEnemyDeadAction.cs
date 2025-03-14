using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsEnemyDead", story: "Check if [Agent] is [Dead]", category: "Action/EnemyManager", id: "b4961ad6070ddb97411bc2bb49edea61")]
public partial class IsEnemyDeadAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<bool> Dead;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Agent == null) return Status.Failure;
        EnemyManager enemyManager = Agent.Value.GetComponent<EnemyManager>();
        if(enemyManager == null) return Status.Failure;
        Dead.Value = enemyManager.IsDead;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

