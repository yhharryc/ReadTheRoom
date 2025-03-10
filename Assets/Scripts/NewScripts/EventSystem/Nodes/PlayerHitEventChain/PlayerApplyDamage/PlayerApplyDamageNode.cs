using UnityEngine;

public class PlayerApplyDamageNode : IEventNode<EventContext>
{
    public int Priority => 30; // After we know final damage

    public void Process(EventContext context)
    {
        if (context == null || context.Target == null || context.HitData == null) 
            return;

        // 1) Cast to your PlayerCharacter (or manager) to apply HP changes
        var playerChar = context.Target.Owner.GetComponent<PlayerCharacter>();
        if (playerChar == null) return;

        float damage = context.HitData.FinalDamage;

        // 2) Apply damage
        // For example: 
        playerChar.TakeDamage(context);

        Debug.Log($"[PlayerApplyDamageNode] Player took {damage} damage.");

        //TODO: If you want partial damage to stamina if blocking, do it here
    }
}
