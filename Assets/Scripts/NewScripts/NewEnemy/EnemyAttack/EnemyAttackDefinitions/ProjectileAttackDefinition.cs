using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAttackDefinition", menuName = "MyGame/AI/ProjectileAttackDefinition")]
public class ProjectileAttackDefinition : AttackDefinition
{
    [Header("Projectile")]
    [Tooltip("The projectile prefab to spawn.")]
    public GameObject projectilePrefab;

    [Tooltip("Speed for the projectile (if not handled inside the projectile logic).")]
    public float projectileSpeed = 10f;

    public override void PerformAttack(ICharacter source, ICharacter target)
    {
        // 1) Basic checks
        if (source == null) {
            Debug.LogWarning("[ProjectileAttackDefinition] PerformAttack: source is null.");
            return;
        }
        if (target == null) {
            Debug.LogWarning("[ProjectileAttackDefinition] PerformAttack: target is null.");
            return;
        }
        if (projectilePrefab == null) {
            Debug.LogWarning("[ProjectileAttackDefinition] No projectilePrefab assigned.");
            return;
        }

        // 2) Suppose we read baseDamage from the source's attribute set:
        float baseDamage = 10f; // e.g., from source.GetAbilitySystemComponent().AttributeSet.Damage
        float finalDamage = baseDamage * damageMultiplier;

        // 3) Build the event context
        var ctx = new EventContext {
            Source = source,
            Target = target as IHitReceiver,
            AttackData = new AttackData {
                BaseDamage = finalDamage,
                ProjectilePrefab = projectilePrefab
            },
            HitData = new HitData {
                FinalDamage = finalDamage
            }
        };

        // 4) Retrieve the source's transform or fallback if not a MonoBehaviour
        var sourceMB = source as MonoBehaviour;
        if (sourceMB == null) {
            Debug.LogWarning("[ProjectileAttackDefinition] Source is not a MonoBehaviour, cannot spawn projectile.");
            return;
        }
        EnemyManager enemyManager= sourceMB.GetComponent<EnemyManager>();
        if (enemyManager == null) {
            Debug.LogWarning("[ProjectileAttackDefinition] Source is not a EnemyManager, cannot spawn projectile.");
        }
        Transform sourceTransform = enemyManager.GetProjectileTransform();

        // 5) Decide where to spawn the projectile
        Vector3 spawnPos = sourceTransform.position;
        Vector3 direction = Vector3.forward; // default
        // If we have a valid target's transform, aim at them:
        var targetMB = target as MonoBehaviour;
        if (targetMB != null) {
            direction = (targetMB.transform.position - spawnPos).normalized;
        } else {
            // If the target isn't a MonoBehaviour, just use forward
            direction = sourceTransform.forward;
        }

        // 6) Instantiate the projectile
        Quaternion spawnRot = Quaternion.LookRotation(direction, Vector3.up);
        GameObject projObj = GameObject.Instantiate(projectilePrefab, spawnPos, spawnRot);

        // 7) If the projectile has a script that uses Setup(EventContext context, Vector3 flightDir), call it:
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Setup(ctx, direction);
        }

        // 8) Optionally, if you want to do something immediately with the AttackChain, 
        //    you could call it here, but usually the projectile handles collisions.
        // EventChainManager.Instance.ExecuteAttackChain(ref ctx);
    }
}
