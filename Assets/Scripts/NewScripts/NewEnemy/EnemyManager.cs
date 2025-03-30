using UnityEngine;
using Unity.Behavior;
using Unity.Behavior;  

public class EnemyManager : TurnActor, ICharacter, IRoomObject
{


    [Header("Ability System / Stats")]
    [SerializeField] private AbilitySystemComponent abilitySystemComponent;
    [SerializeField] private CharacterAttributeSet enemyAttributeSet;
    [SerializeField] private Transform projectileSpawnTransform;

    private BehaviorGraph behaviorGraph;
    public BehaviorGraph BehaviorGraph{get {return behaviorGraph;}}

    private Animator animator;

    private bool isDead = false;
    public bool IsDead{get {return isDead;}}
    public event System.Action<ICharacter> OnCharacterDied;

    public Faction Faction => Faction.ENEMY;

    public bool IsKnockedDown {get{return animator.GetBool("KnockedDown");}}

    private bool isAttacking;
    public bool IsAttacking {get{return isAttacking;}set{isAttacking = value;}}
    [Header("AI Settings")]
    [SerializeField]
    private BehaviorGraph attackBehaviorGraph;
    public BehaviorGraph AttackBehaviorGraph{get{return attackBehaviorGraph;}}
    [SerializeField]
    private AttackDefinition currentAttack;
    public AttackDefinition CurrentAttack{get{return currentAttack;}}

    private GameObject target;

    private void Awake()
    {
        // If not assigned in inspector, try to get them on the same object
        if (abilitySystemComponent == null)
            abilitySystemComponent = GetComponent<AbilitySystemComponent>();
        if (enemyAttributeSet == null && abilitySystemComponent != null)
            enemyAttributeSet = abilitySystemComponent.AttributeSet as CharacterAttributeSet;

        if (enemyAttributeSet != null)
        {
            float maxHealth = enemyAttributeSet.MaxHealth.CurrentValue;

            float currentHealth = enemyAttributeSet.Health.CurrentValue;
            
            //Debug.Log($"Enemy initial Health = {currentHealth}/{maxHealth}");
        }
        animator = GetComponent<Animator>();

    }
    public virtual GameObject GetTarget()
    {
        target = GameManager.Instance.PlayerCharacter.gameObject;
        return target;
    }
    protected virtual void Start() {
        behaviorGraph = GetComponent<BehaviorGraphAgent>().Graph;
        if (behaviorGraph != null)
        {
            InitializeBehaviorGraph();
        }
        
        
    }
    private void Update()
    {
        animator.SetFloat("WalkSpeed", 1.2f);
        
    }

    public virtual void InitializeBehaviorGraph()
    {
        behaviorGraph.BlackboardReference.SetVariableValue("Animator", animator);
        behaviorGraph.BlackboardReference.SetVariableValue("PlayerObject",GameManager.Instance.PlayerCharacter.gameObject);
        
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
        
        enemyAttributeSet.Stagger.BaseValue = newStagger;
        if (newStagger <= 0f &&!isDead)
        {
            Stagger(context);
            KnockDown();
            return;
        }
        if (context.Target.HitPartType  == HitPartType.WeakPoint||(context.AttackData.PushType>0 && isAttacking))
        {   
            Stagger(context);
        }
    }
    
    public void Stagger(EventContext context)
    {      
        BehaviorGraph.BlackboardReference.SetVariableValue("IsStaggered",true);
    }

    public void OnAnimationCheckHit()
    {
        
        if (behaviorGraph.BlackboardReference.GetVariableValue("CheckHitEvent",out CheckHit eventValue))
        {
            eventValue.SendEventMessage(gameObject);
        }
    }

    public void KnockDown()
    {
        if (animator != null) {
            animator.SetBool("KnockedDown", true);
        }
        
    }

    public void RecoverFromKnockDown()
    {
        if (animator!=null&&animator.GetBool("KnockedDown"))
        {
            animator.SetBool("KnockedDown", false);
            enemyAttributeSet.Stagger.BaseValue = enemyAttributeSet.MaxStagger.CurrentValue;
        }
        BehaviorGraph.BlackboardReference.SetVariableValue("IsStaggered",false);
        
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
        animator.SetBool("IsDead", true);
        animator.SetTrigger("DieTrigger");
        OnCharacterDied?.Invoke(this);
        //gameObject.SetActive(false);
        
    }
    public void OnDeathStateEnded()
    {
        //HACK: Need proper VFX, turning off collider, etc
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
        
        BehaviorGraph.BlackboardReference.SetVariableValue("IsCombatStarted", true);
    }

    public void OnCombatEndedInRoom(Room room)
    {
        BehaviorGraph.BlackboardReference.SetVariableValue("IsCombatStarted", false);
    }

    public virtual float GetStaggerMultiplier(EventContext context)
    {
        if(isAttacking)
        {
            return 1f;
        }
        return 0f;
    }

    public virtual Transform GetProjectileTransform()
    {
        if (projectileSpawnTransform ==null)
        {
            return gameObject.transform;
        }
        return projectileSpawnTransform;
    }

    public override void StartTurn()
    {
        Debug.Log($"[{gameObject.name}] It's my turn!");
        // Enable player input or UI that indicates "Your Turn"
    }

    public override void EndTurn()
    {
        Debug.Log($"[{gameObject.name}] Turn ended.");
        // Possibly disable certain input or UI
    }

    public override bool IsTurnComplete
    {
        get {
            // If you have logic like "the player pressed End Turn" or "did their actions"
            // or if the player's sub-state says they're done.
            return isTurnComplete;
        }
    }
}
