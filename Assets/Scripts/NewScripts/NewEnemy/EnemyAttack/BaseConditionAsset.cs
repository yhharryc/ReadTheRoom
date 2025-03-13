using UnityEngine;


public abstract class BaseConditionAsset : ScriptableObject,IAttackCondition
{
    public abstract bool Evaluate(AttackContext context);

}
