using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCurrentAttack", story: "Get Current [Attack] from [Agent]", category: "Action", id: "d583b52897f2daeae482f566793c7246")]
public partial class GetCurrentAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<AttackDefinition> Attack;
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
        Attack.Value = enemy.CurrentAttack;
        if(enemy.CurrentAttack ==null) return Status.Failure;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

