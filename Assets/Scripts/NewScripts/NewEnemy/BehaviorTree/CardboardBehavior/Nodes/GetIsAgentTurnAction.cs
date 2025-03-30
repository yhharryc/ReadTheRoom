using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetIsAgentTurn", story: "Get [IsAgentTurn] from [Self]", category: "Action/TurnBased", id: "690243ef4497c344a7035e5b6b429f92")]
public partial class GetIsAgentTurnAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> IsAgentTurn;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        EnemyManager enemy = Self.Value.GetComponent<EnemyManager>();
        if (enemy != null)
        {
            IsAgentTurn.Value = TurnManager.Instance.IsActorTurn(enemy);
            return Status.Success;
        }
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

