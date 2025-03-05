using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CalculateBaseDamageNode", menuName = "EventNodes/HitEventChain/CalculateBaseDamageNode")]
public class CalculateBaseDamageNodeAsset : ScriptableEventNode<EventContext>
{


    public override IEventNode<EventContext> CreateNodeInstance()
    {
        return new CalculateBaseDamageNode();
    }
}