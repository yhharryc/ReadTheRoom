using UnityEngine;

public class BatEnemyManager : EnemyManager
{
        public override float GetStaggerMultiplier(EventContext context)
    {
        return 0f;
    }
}
