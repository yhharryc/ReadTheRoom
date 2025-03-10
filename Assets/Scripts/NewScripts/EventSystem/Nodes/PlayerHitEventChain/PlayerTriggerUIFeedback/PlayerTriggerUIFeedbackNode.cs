using UnityEngine;

public class PlayerTriggerUIFeedbackNode : IEventNode<EventContext>
{
    public int Priority => 50; // After damage/stagger is applied

    public void Process(EventContext context)
    {
        if (context == null || context.Target == null || context.HitData == null)
            return;

        // 1) Possibly spawn floating text or update the player's health bar
        //    For example:
        // var damageNumberMgr = DamageNumberManager.Instance;
        // if (damageNumberMgr != null)
        // {
        //     damageNumberMgr.ShowDamageNumber(
        //         playerPosition,
        //         context.HitData.FinalDamage,
        //         wasCrit: false,
        //         context
        //     );
        // }

        // 2) Possibly do screen shake or some VFX for player being hit
        // e.g. CameraShakeManager.Instance.ShakeCamera(...);

        //TODO: Implement UI/FX feedback for player damage
    }
}
