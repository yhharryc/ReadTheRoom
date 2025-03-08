using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/CheckHit")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "CheckHit", message: "[Agent] has started Attacking", category: "Action/Animation", id: "79f61a244669f7ee20f8512e3867d469")]
public partial class CheckHit : EventChannelBase
{
    public delegate void CheckHitEventHandler(GameObject Agent);
    public event CheckHitEventHandler Event; 

    public void SendEventMessage(GameObject Agent)
    {
        Event?.Invoke(Agent);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Agent);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        CheckHitEventHandler del = (Agent) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as CheckHitEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as CheckHitEventHandler;
    }
}

