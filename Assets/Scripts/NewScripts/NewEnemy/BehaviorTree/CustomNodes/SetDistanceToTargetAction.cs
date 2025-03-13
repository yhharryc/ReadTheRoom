using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDistanceToTarget", story: "Calculate [Distance] from [Agent] to [Target]", category: "Action", id: "317a74eb128194aa6596e6e31a8915d6")]
public partial class SetDistanceToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent == null || Target == null) return Status.Failure;
        float dist = Vector3.Distance(Agent.Value.transform.position,
                                      Target.Value.transform.position);

        //GameObject.GetComponent<BehaviorGraphAgent>().Graph.BlackboardReference.SetVariableValue("DistanceToTarget", dist);
        Distance.Value = dist;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

