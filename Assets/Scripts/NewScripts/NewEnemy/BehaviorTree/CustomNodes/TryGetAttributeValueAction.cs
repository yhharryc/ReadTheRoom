using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TryGetAttributeValue", story: "Try getting an [AttributeValue] from the [Target] as a [Float]", category: "Action/AbilitySystem", id: "4c0cb4658a3cc273f2fdcbd9f7567eb4")]
public partial class TryGetAttributeValueAction : Action
{
    [SerializeReference] public BlackboardVariable<string> AttributeValue;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Float;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        AbilitySystemComponent asc = Target.Value.GetComponent<AbilitySystemComponent>();
        if(asc==null) return Status.Failure;
        float temp = asc.GetAttributeValue(AttributeValue, out bool foundAttribute);
        if (foundAttribute)
        {
            Float.Value = temp;
            return Status.Success;
        }
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

