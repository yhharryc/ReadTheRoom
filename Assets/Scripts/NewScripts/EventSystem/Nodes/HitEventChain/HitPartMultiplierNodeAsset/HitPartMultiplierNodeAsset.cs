using UnityEngine;

[CreateAssetMenu(
    fileName = "HitPartMultiplierNode", 
    menuName = "EventNodes/HitEventChain/HitPartMultiplierNode")]
public class HitPartMultiplierNodeAsset : ScriptableEventNode<EventContext>
{
    // Optionally store multipliers if you want them configurable
    [Header("Hit Part Multipliers")]
    public float weakPointMultiplier = 1.5f;
    public float armoredMultiplier = 0.3f;

    public override IEventNode<EventContext> CreateNodeInstance()
    {
        var node = new HitPartMultiplierNode();
        node.WeakPointMultiplier = weakPointMultiplier;
        node.ArmoredMultiplier   = armoredMultiplier;
        return node;
    }
}
