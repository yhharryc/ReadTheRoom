using MoreMountains.Feedbacks;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayMMF", story: "[Agent] players [MMF_Player]", category: "MMF/Action", id: "dae21c572edfe152ba41b8f5cfedd682")]
public partial class PlayMmfAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<MMF_Player> MMF_Player;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(MMF_Player!=null)
        {
            MMF_Player.Value.PlayFeedbacks();
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

