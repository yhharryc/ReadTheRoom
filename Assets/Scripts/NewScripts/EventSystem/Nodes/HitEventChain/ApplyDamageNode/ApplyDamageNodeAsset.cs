using UnityEngine;

[CreateAssetMenu(
    fileName = "ApplyDamageNode", 
    menuName = "EventNodes/HitEventChain/ApplyDamageNode")]
public class ApplyDamageNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new ApplyDamageNode();
    }
}
