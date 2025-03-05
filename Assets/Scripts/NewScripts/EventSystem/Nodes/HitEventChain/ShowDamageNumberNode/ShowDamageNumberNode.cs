using UnityEngine;

public class ShowDamageNumberNode : IEventNode<EventContext>
{
    public int Priority => 50;

    public void Process(EventContext context)
    {
        if (context == null || context.HitData == null) return;

        var character = context.Target?.Owner.GetComponent<ICharacter>();
        if (context.AttackData.PushType !=0)
        {
            return;
        }
        if (character != null && character.Faction == Faction.ENEMY)
        {
            float finalDamage = context.HitData.FinalDamage;
            Vector3 hitPos = context.HitData.HitInfo.HitPoint;

            // Show a damage number
            DamageNumberManager.Instance.ShowDamageNumber(
                hitPos,
                finalDamage,
                context.HitData.WasCrit,
                context
            );
        }
    }
}
