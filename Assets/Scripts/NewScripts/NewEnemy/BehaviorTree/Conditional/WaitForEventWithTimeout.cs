using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskDescription("Waits for an event (eventName). Returns Running until event is received. " +
                 "Returns Success when event is received, or Failure after maxWaitTime if not received.")]
[TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
public class WaitForEventWithTimeout : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the event to receive")]
    public SharedString eventName = "";

    [BehaviorDesigner.Runtime.Tasks.Tooltip("How long to wait before returning Failure if event is not received. " +
             "If <= 0, no timeout and will wait indefinitely.")]
    public SharedFloat maxWaitTime = 5f;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("Optionally store the first argument")]
    [SharedRequired] public SharedVariable storedValue1;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Optionally store the second argument")]
    [SharedRequired] public SharedVariable storedValue2;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Optionally store the third argument")]
    [SharedRequired] public SharedVariable storedValue3;

    private bool eventReceived = false;
    private bool registered = false;
    private float timeElapsed = 0f;

    public override void OnStart()
    {
        // Register for the specified event if not already registered
        if (!registered)
        {
            Owner.RegisterEvent(eventName.Value, ReceivedEvent);
            Owner.RegisterEvent<object>(eventName.Value, ReceivedEvent);
            Owner.RegisterEvent<object, object>(eventName.Value, ReceivedEvent);
            Owner.RegisterEvent<object, object, object>(eventName.Value, ReceivedEvent);
            registered = true;
        }

        eventReceived = false;
        timeElapsed = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        // If we already got the event, we succeed
        if (eventReceived)
        {
            return TaskStatus.Success;
        }

        // Otherwise, check if we timed out
        if (maxWaitTime.Value > 0f)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= maxWaitTime.Value)
            {
                return TaskStatus.Failure;
            }
        }

        // Keep waiting
        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        // Unregister only if we had registered
        if (registered)
        {
            Owner.UnregisterEvent(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object>(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object, object>(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object, object, object>(eventName.Value, ReceivedEvent);
            registered = false;
        }
        eventReceived = false;
        timeElapsed = 0f;
    }

    private void ReceivedEvent()
    {
        eventReceived = true;
    }

    private void ReceivedEvent(object arg1)
    {
        ReceivedEvent();
        if (storedValue1 != null && !storedValue1.IsNone)
        {
            storedValue1.SetValue(arg1);
        }
    }

    private void ReceivedEvent(object arg1, object arg2)
    {
        ReceivedEvent();
        if (storedValue1 != null && !storedValue1.IsNone)
        {
            storedValue1.SetValue(arg1);
        }
        if (storedValue2 != null && !storedValue2.IsNone)
        {
            storedValue2.SetValue(arg2);
        }
    }

    private void ReceivedEvent(object arg1, object arg2, object arg3)
    {
        ReceivedEvent();
        if (storedValue1 != null && !storedValue1.IsNone)
        {
            storedValue1.SetValue(arg1);
        }
        if (storedValue2 != null && !storedValue2.IsNone)
        {
            storedValue2.SetValue(arg2);
        }
        if (storedValue3 != null && !storedValue3.IsNone)
        {
            storedValue3.SetValue(arg3);
        }
    }

    public override void OnBehaviorComplete()
    {
        // Stop receiving the event when the behavior tree completes
        if (registered)
        {
            Owner.UnregisterEvent(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object>(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object, object>(eventName.Value, ReceivedEvent);
            Owner.UnregisterEvent<object, object, object>(eventName.Value, ReceivedEvent);
            registered = false;
        }
        eventReceived = false;
        timeElapsed = 0f;
    }

    public override void OnReset()
    {
        eventName = "";
        maxWaitTime = 5f;
        storedValue1 = null;
        storedValue2 = null;
        storedValue3 = null;
    }
}
