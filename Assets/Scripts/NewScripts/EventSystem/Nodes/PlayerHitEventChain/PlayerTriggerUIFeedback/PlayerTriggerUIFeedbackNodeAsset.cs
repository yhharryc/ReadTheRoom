using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerTriggerUIFeedbackNodeAsset", 
    menuName = "EventNodes/PlayerHitEventChain/PlayerTriggerUIFeedbackNodeAsset")]
public class PlayerTriggerUIFeedbackNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new PlayerTriggerUIFeedbackNode();
    }
}
