using UnityEngine;

[CreateAssetMenu(
    fileName = "ShowDamageNumberNode", 
    menuName = "EventNodes/HitEventChain/ShowDamageNumberNode")]
public class ShowDamageNumberNodeAsset : ScriptableEventNode<EventContext>
{
    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new ShowDamageNumberNode();
    }
}
