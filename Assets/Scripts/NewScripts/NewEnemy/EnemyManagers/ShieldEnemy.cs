using UnityEngine;
using MoreMountains.Feedbacks;

public class ShieldEnemy : EnemyManager
{
    public MMF_Player RotateMMF;
    public override void TakeDamage(EventContext context)
    {
        base.TakeDamage(context);

        float dmg = context.HitData.FinalDamage;
        if(dmg == 0f)
        {
            //Damage is blocked. 
            RotateMMF?.PlayFeedbacks();
        }
    }
    public override Unity.Behavior.Node.Status OnIdleBehavior(int behaviorID = 0)
    {
        
        RotateMMF?.PlayFeedbacks();
        return Unity.Behavior.Node.Status.Success;
    }
    
}
