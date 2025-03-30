using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EndCurrentTurn", story: "[Agent] ends current turn", category: "Action/TurnBased", id: "9263be53d90b60878a50978e38ec6c38")]
public partial class EndCurrentTurnAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        EnemyManager enemy = Agent.Value.GetComponent<EnemyManager>();
        if (enemy != null)
        {
            //IsAgentTurn.Value = enemy.IsActorTurn();
            if(TurnManager.Instance.IsActorTurn(enemy))
            {
                TurnManager.Instance.EndCurrentTurn();
                return Status.Success;
            }
            return Status.Failure;
        }
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

