using UnityEngine;


public enum HitPartType
{
    Normal,
    WeakPoint,
    Armored
}
/// <summary>
/// Attach this to a child GameObject that has a Collider or Trigger. 
/// It implements IHitReceiver so projectiles or melee hits can call OnHit.
/// Then it forwards the final damage data to the main EnemyManager.
/// </summary>
public class EnemyHitCollider : MonoBehaviour, IHitReceiver
{
    [Header("Hit Part Settings")]
    public HitPartType partType = HitPartType.Normal;


    [Tooltip("Reference to the main EnemyManager (the root).")]
    public EnemyManager enemyManager;
    public GameObject Owner {get {return enemyManager.gameObject;}}

    private void Awake()
    {
        // If not assigned, try to find the EnemyManager on the root:
        if (enemyManager == null)
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            if (enemyManager == null)
            {
                Debug.LogError($"EnemyHitCollider on {name}: no EnemyManager found in parents!");
            }
        }
    }

    /// <summary>
    /// Called by the projectile or melee system to notify of a hit.
    /// We'll adjust or annotate the HitData based on partType, then pass it to EnemyManager.
    /// </summary>
    public void OnHit(EventContext eventContext)
    {
        
    }
}
