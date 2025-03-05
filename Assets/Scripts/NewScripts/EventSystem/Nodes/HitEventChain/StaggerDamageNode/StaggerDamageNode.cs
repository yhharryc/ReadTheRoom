using UnityEngine;

public class StaggerDamageNode : IEventNode<EventContext>
{
    public int Priority => 30; // run after we've set finalDamage and know part type

    public void Process(EventContext context)
    {
        if (context == null || context.AttackData == null) return;

        // Suppose we track stagger in AttackData as well:
        // e.g., context.AttackData.StaggerAmount or something
        float baseStagger = context.AttackData.PushStagger;
        float finalStagger = baseStagger;

        // If it’s a weakpoint, double it
        var hitCollider = context.Target as EnemyHitCollider;
        if (hitCollider != null && hitCollider.partType == HitPartType.WeakPoint) {
            finalStagger *= 2f;
        }

        // If push type is > 0 and the enemy is “vulnerable,” we might do more
        // (You’d have to define how we check “enemy is vulnerable.” Possibly in the enemy manager.)
        if (context.AttackData.PushType > 0) {
            // For demonstration: if the enemy manager says IsVulnerable => 2x
            var enemyMgr = context.Target?.Owner.GetComponent<EnemyManager>();
            //if (enemyMgr != null && enemyMgr.SomeCheckIfVulnerable()) {
            //    finalStagger *= 2f;
            //}
        }

        // Then store finalStagger in AttackData or in HitData, 
        // e.g. context.HitData.StaggerValue = finalStagger;

        Debug.Log($"[StaggerDamageNode] baseStagger={baseStagger}, finalStagger={finalStagger}");
    }
}
