using BehaviorDesigner.Runtime;
using System;


[Serializable]
public class SharedAttackDefinition : SharedVariable<AttackDefinition>
{
    // This makes it easier to assign AttackDefinition assets directly in the Inspector
    public static implicit operator SharedAttackDefinition(AttackDefinition value)
    {
        return new SharedAttackDefinition { Value = value };
    }
}

