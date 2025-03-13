using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Check LOS", story: "Checks [LOS] from [Agent] to [Target]", category: "Action", id: "b84669cf242424c1caeaea1552670e6a")]
public partial class CheckLosAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> LOS;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

