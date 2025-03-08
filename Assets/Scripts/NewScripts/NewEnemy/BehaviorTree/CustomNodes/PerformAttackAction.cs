using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Perform Attack Action", story: "[Agent] attacks [Target] with [CurrentAttack]", category: "Action", id: "4380860911cf19ffb8030664c999a8eb")]
public partial class PerformAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<AttackDefinition> CurrentAttack;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent == null|| Target == null) return Status.Failure;
        if (CurrentAttack == null) return Status.Failure;
        CurrentAttack.Value.PerformAttack(Agent.Value.GetComponent<EnemyManager>(), Target.Value.GetComponent<PlayerCharacter>());
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

