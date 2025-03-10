using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerApplyDamageNodeAsset", 
    menuName = "EventNodes/PlayerHitEventChain/PlayerApplyDamageNodeAsset")]
public class PlayerApplyDamageNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new PlayerApplyDamageNode();
    }
}
