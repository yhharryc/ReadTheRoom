using UnityEngine;

public class CalculateBaseDamageNode : IEventNode<EventContext>
{
    public int Priority => 10; // e.g., run first

    private float minRandomFactor = 0.925f;
    private float maxRandomFactor = 1.075f;

    public void Process(EventContext context)
    {
        if (context == null || context.AttackData == null || context.HitData == null)
            return;

        // 1) Base damage
        float baseDamage = context.AttackData.BaseDamage;

        // 2) Random factor
        float randomFactor = Random.Range(minRandomFactor, maxRandomFactor);

        // 3) Apply and store in FinalDamage (this is just the initial baseline)
        float initialDamage = Mathf.Round(baseDamage * randomFactor);
        context.HitData.FinalDamage = initialDamage;

        // Debug log if desired
        Debug.Log($"[CalculateBaseDamageNode] BaseDamage={baseDamage}, final={initialDamage}");
    }
}
