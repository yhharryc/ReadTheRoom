using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetEnemyTarget", story: "[Agent] gets a [Target]", category: "Action", id: "8ba64644a50a46f67474e7805d3fc40a")]
public partial class GetEnemyTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

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
        Target.Value = enemy.GetTarget();
        if(Target.Value  == null) return Status.Failure;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

