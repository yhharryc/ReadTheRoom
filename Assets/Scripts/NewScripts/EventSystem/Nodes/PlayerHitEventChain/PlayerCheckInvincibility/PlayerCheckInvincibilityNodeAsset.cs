using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerCheckInvincibilityNodeAsset", 
    menuName = "EventNodes/PlayerHitEventChain/PlayerCheckInvincibilityNodeAsset")]
public class PlayerCheckInvincibilityNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new PlayerCheckInvincibilityNode();
    }
}
