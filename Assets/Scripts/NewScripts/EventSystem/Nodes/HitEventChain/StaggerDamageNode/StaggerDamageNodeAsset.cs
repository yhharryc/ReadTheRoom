using UnityEngine;

[CreateAssetMenu(
    fileName = "StaggerDamageNode", 
    menuName = "EventNodes/HitEventChain/StaggerDamageNode")]
public class StaggerDamageNodeAsset : ScriptableEventNode<EventContext>
{
    // Example: default pushStagger or additional logic
    // If you want separate config, add them here

    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new StaggerDamageNode();
    }
}
