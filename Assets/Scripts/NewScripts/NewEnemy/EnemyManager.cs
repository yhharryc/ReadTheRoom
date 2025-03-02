using System;
using UnityEngine;

/// <summary>
/// EnemyManager serves as the main script on the Enemy root object.
/// It merges "basic stats & health logic" with references to the 
/// ability system (for attribute sets) and the death flow.
/// </summary>
public class EnemyManager : MonoBehaviour, ICharacter
{
    [Header("Ability System / Stats")]
    [SerializeField] private AbilitySystemComponent abilitySystemComponent;
    [SerializeField] private CharacterAttributeSet enemyAttributeSet; 
    // e.g. Health, MaxHealth, Damage fields in that attribute set

    // Optionally track if we've died
    private bool isDead = false;

    // Public event for the outside world to know we died
    public event Action<ICharacter> OnCharacterDied;

    public Faction Faction => Faction.ENEMY; // we define the enemy's factionw



    private void Awake()
    {
        // If not assigned in inspector, try to get them on the same object
        if (abilitySystemComponent == null)
            abilitySystemComponent = GetComponent<AbilitySystemComponent>();
        if (enemyAttributeSet == null && abilitySystemComponent != null)
            enemyAttributeSet = abilitySystemComponent.AttributeSet as CharacterAttributeSet;

        // Initialize from the attribute set if needed
        if (enemyAttributeSet != null)
        {
            // For example, if we want to set current Health from base Health
            float currentHealth = enemyAttributeSet.Health.CurrentValue;
            float maxHealth = enemyAttributeSet.MaxHealth.CurrentValue;
            Debug.Log($"Enemy initial Health = {currentHealth}/{maxHealth}");
        }
    }

    #region IHitReceiver / ICharacter Implementation

    public void OnHit(HitData hitData)
    {
        // If you have logic that uses hitData for partial absorption or something, do it here
        // e.g. float finalDamage = SomeDamageReductionCheck(hitData.FinalDamage);
        // But typically you'd rely on the "TakeDamage(EventContext)" approach as well.
    }

    // This is the "TakeDamage" from ICharacter. 
    // We'll read from abilitySystem or attribute set to adjust health
    public void TakeDamage(EventContext context)
    {
        if (isDead) return; // ignore further damage

        float damageAmount = context.HitData.FinalDamage;

        // We'll get the current HP from the attribute set
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP - damageAmount;

        // We can pass this through the ability system to do clamping or trigger Pre/Post changes
        // or we can do direct changes on the GameplayAttribute:
        enemyAttributeSet.Health.BaseValue = newHP; 
        // The attribute set or ability system might automatically clamp it
        // if you have hooking logic in PreAttributeChange.

        Debug.Log($"[EnemyManager] Took {damageAmount} damage. HP from {oldHP} => {newHP}");

        if (newHP <= 0f && !isDead)
        {
            Die(context);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP - damage;

        enemyAttributeSet.Health.BaseValue = newHP;

        if (newHP <= 0f && !isDead)
        {
            Die(); 
        }
    }

    // Because ICharacter has "AddHealth(float amount)"
    public void AddHealth(float amount)
    {
        if (isDead) return;
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP + amount;
        enemyAttributeSet.Health.BaseValue = newHP;
    }

    public float Health => enemyAttributeSet != null 
        ? enemyAttributeSet.Health.CurrentValue 
        : 0f;

    public float MaxHealth => enemyAttributeSet != null
        ? enemyAttributeSet.MaxHealth.CurrentValue
        : 100f;

    // If we must implement "Die()" from ICharacter:
    public void Die()
    {
        Die(null);
    }

    #endregion

    /// <summary>
    /// Called internally if we want the context for the final blow
    /// (maybe to pass a reference for awarding XP or kill credit).
    /// </summary>
    private void Die(EventContext context = null)
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[EnemyManager] Enemy {name} died.");

        // Fire event for outside listeners
        OnCharacterDied?.Invoke(this);

        // disable or destroy
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public AbilitySystemComponent GetAbilitySystemComponent()
    {
        return abilitySystemComponent;
    }
}
