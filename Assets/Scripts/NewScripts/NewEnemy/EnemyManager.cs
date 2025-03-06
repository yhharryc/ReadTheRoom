using UnityEngine;
using BehaviorDesigner.Runtime; // Required for BehaviorTree, SharedVariable, etc.

public class EnemyManager : MonoBehaviour, ICharacter, IRoomObject
{
    [Header("Ability System / Stats")]
    [SerializeField] private AbilitySystemComponent abilitySystemComponent;
    [SerializeField] private CharacterAttributeSet enemyAttributeSet;

    // Add a reference to the Behavior Designer tree
    [Header("Behavior Tree")]
    [SerializeField] private BehaviorTree behaviorTree;

    private Animator animator;

    private bool isDead = false;
    public event System.Action<ICharacter> OnCharacterDied;

    public Faction Faction => Faction.ENEMY;

    private void Awake()
    {
        // If not assigned in inspector, try to get them on the same object
        if (abilitySystemComponent == null)
            abilitySystemComponent = GetComponent<AbilitySystemComponent>();
        if (enemyAttributeSet == null && abilitySystemComponent != null)
            enemyAttributeSet = abilitySystemComponent.AttributeSet as CharacterAttributeSet;

        if (enemyAttributeSet != null)
        {
            float currentHealth = enemyAttributeSet.Health.CurrentValue;
            float maxHealth = enemyAttributeSet.MaxHealth.CurrentValue;
            Debug.Log($"Enemy initial Health = {currentHealth}/{maxHealth}");
        }
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetFloat("WalkSpeed", 1.2f);
        
    }
    // ---------------------------------------------
    // ICharacter / IHitReceiver Implementation
    // ---------------------------------------------
    public void OnHit(EventContext eventContext) { 
        if(isDead)return;
        TakeDamage(eventContext);
        
        TakeStaggerDamage(eventContext);

    }

    public void TakeDamage(EventContext context)
    {
        //if (isDead) return;
        float dmg = context.HitData.FinalDamage;
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP - dmg;

        enemyAttributeSet.Health.BaseValue = newHP;

        if (newHP <= 0f && !isDead)
        {
            Die(context);
        }
    }

    public void TakeDamage(float damage)
    {
        //if (isDead) return;
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP - damage;
        enemyAttributeSet.Health.BaseValue = newHP;

        if (newHP <= 0f && !isDead)
        {
            Die();
        }
    }

    public void TakeStaggerDamage(EventContext context)
    {
        
        float oldStagger = enemyAttributeSet.Stagger.CurrentValue;
        float newStagger = oldStagger - context.HitData.FinalStagger;
        //TODO: Stagger multiplier based on enemy state.

        enemyAttributeSet.Stagger.BaseValue = newStagger;
        if (newStagger <= 0f &&!isDead)
        {
            Stagger(context);
        }
    }
    
    public void Stagger(EventContext context)
    {
        //TODO:Stagger for enemy
        if (animator != null) {
            //animator.SetTrigger("StaggerTrigger");
        }
        behaviorTree.SetVariableValue("IsStaggered", true);
    }

    public void AddHealth(float amount)
    {
        if (isDead) return;
        float oldHP = enemyAttributeSet.Health.CurrentValue;
        float newHP = oldHP + amount;
        enemyAttributeSet.Health.BaseValue = newHP;
    }

    public float Health => enemyAttributeSet != null ? enemyAttributeSet.Health.CurrentValue : 0f;
    public float MaxHealth => enemyAttributeSet != null ? enemyAttributeSet.MaxHealth.CurrentValue : 100f;

    public void Die()
    {
        Die(null);
    }

    private void Die(EventContext context = null)
    {
        if (isDead) return;
        isDead = true;
        Debug.Log($"[EnemyManager] {name} died.");

        OnCharacterDied?.Invoke(this);
        gameObject.SetActive(false);
    }

    public AbilitySystemComponent GetAbilitySystemComponent()
    {
        return abilitySystemComponent;
    }

    // ---------------------------------------------
    // IRoomObject Implementation
    // ---------------------------------------------
    public void OnCombatStartedInRoom(Room room)
    {
        
        // 1) If we have a BehaviorTree, set the "IsCombatStarted" variable to true
        if (behaviorTree != null)
        {
            // Approach A: Using SetVariableValue (no cast needed):
            behaviorTree.SetVariableValue("IsCombatStarted", true);

            // Approach B: Or you can do a direct cast to SharedBool:
            // var isCombatStartedVar = behaviorTree.GetVariable("IsCombatStarted") as SharedBool;
            // if (isCombatStartedVar != null) {
            //     isCombatStartedVar.Value = true;
            // }
        }
    }

    public void OnCombatEndedInRoom(Room room)
    {
        // If you want to reset it to false on combat end:
        if (behaviorTree != null)
        {
            behaviorTree.SetVariableValue("IsCombatStarted", false);
        }
    }

    public virtual float GetStaggerMultiplier(EventContext context)
    {
        return 1f;
    }
}
