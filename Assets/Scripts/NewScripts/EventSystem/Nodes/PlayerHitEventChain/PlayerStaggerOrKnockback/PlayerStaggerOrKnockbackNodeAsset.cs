using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStaggerOrKnockbackNodeAsset", 
    menuName = "EventNodes/PlayerHitEventChain/PlayerStaggerOrKnockbackNodeAsset")]
public class PlayerStaggerOrKnockbackNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new PlayerStaggerOrKnockbackNode();
    }
}
