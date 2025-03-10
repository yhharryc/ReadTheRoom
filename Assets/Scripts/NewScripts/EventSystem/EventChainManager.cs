using UnityEngine;
using System;

public class EventChainManager : MonoBehaviour
{
    public static EventChainManager Instance { get; private set; }

    [Header("Initial Node Configuration (Loadable from ScriptableObjects)")]
    public ScriptableEventNode<EventContext>[] initialAttackNodes;
    public ScriptableEventNode<EventContext>[] initialHitNodes;
    public ScriptableEventNode<CharacterDiedEventContext>[] initialCharacterDiedEventNodes;

    // ---------------- NEW: ScriptableObjects for PlayerHit chain ----------------
    public ScriptableEventNode<EventContext>[] initialPlayerHitEventNodes;

    public EventChain<EventContext> AttackEventChain { get; private set; }
    public EventChain<EventContext> HitEventChain { get; private set; }
    public EventChain<CharacterDiedEventContext> CharacterDiedEventChain { get; private set; }

    // ---------------- NEW: The PlayerHitEventChain reference ----------------
    public EventChain<EventContext> PlayerHitEventChain { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        AttackEventChain = new EventChain<EventContext>();
        HitEventChain = new EventChain<EventContext>();
        CharacterDiedEventChain = new EventChain<CharacterDiedEventContext>();

        // ---------------- NEW: Create PlayerHitEventChain ----------------
        PlayerHitEventChain = new EventChain<EventContext>();

        // 1) Load initial nodes for AttackEventChain
        if (initialAttackNodes != null)
        {
            foreach (var nodeAsset in initialAttackNodes)
            {
                var node = nodeAsset?.CreateNodeInstance();
                if (node != null)
                    AttackEventChain.AddNode(node);
            }
        }

        // 2) Add a default node to HitEventChain
        HitEventChain.AddNode(new HandleHitReceiverNode());

        // 3) Load initial nodes for HitEventChain
        if (initialHitNodes != null)
        {
            foreach (var nodeAsset in initialHitNodes)
            {
                var node = nodeAsset?.CreateNodeInstance();
                if (node != null)
                    HitEventChain.AddNode(node);
            }
        }

        // 4) Load initial nodes for CharacterDiedEventChain
        if (initialCharacterDiedEventNodes != null)
        {
            foreach (var nodeAsset in initialCharacterDiedEventNodes)
            {
                var node = nodeAsset?.CreateNodeInstance();
                if (node != null)
                    CharacterDiedEventChain.AddNode(node);
            }
        }

        // ---------------- NEW: Load initial nodes for PlayerHitEventChain ----------------
        if (initialPlayerHitEventNodes != null)
        {
            foreach (var nodeAsset in initialPlayerHitEventNodes)
            {
                var node = nodeAsset?.CreateNodeInstance();
                if (node != null)
                    PlayerHitEventChain.AddNode(node);
            }
        }
    }

    private void OnValidate()
    {
        // Validate that each array only has nodes matching the correct TContext
        ValidateNodesArray<EventContext>(ref initialAttackNodes, "initialAttackNodes");
        ValidateNodesArray<EventContext>(ref initialHitNodes, "initialHitNodes");
        ValidateNodesArray<CharacterDiedEventContext>(ref initialCharacterDiedEventNodes, "initialCharacterDiedEventNodes");

        // ---------------- NEW: Also validate initialPlayerHitEventNodes as EventContext ----------------
        ValidateNodesArray<EventContext>(ref initialPlayerHitEventNodes, "initialPlayerHitEventNodes");
    }

    /// <summary>
    /// Checks each entry in the array. If the scriptable object is not null and 
    /// does not report the correct context type, we log a warning and set it to null.
    /// </summary>
    private void ValidateNodesArray<TExpectedContext>(
        ref ScriptableEventNode<TExpectedContext>[] array, 
        string arrayName)
    {
        if (array == null) return;

        for (int i = 0; i < array.Length; i++)
        {
            var nodeAsset = array[i];
            if (nodeAsset == null) continue;

            var reportedType = nodeAsset.GetContextType();
            if (reportedType != typeof(TExpectedContext))
            {
                Debug.LogWarning($"[EventChainManager] {arrayName}[{i}] is of type {reportedType}, " + 
                                 $"expected {typeof(TExpectedContext)}. Removing this entry.");
                array[i] = null;
            }
        }
    }

    // -----------------------------------------
    // Attack chain methods
    // -----------------------------------------
    public void ExecuteAttackChain(ref EventContext context)
    {
        AttackEventChain.Execute(context);
    }
    public void AddNodeToAttackChain(IEventNode<EventContext> node)
    {
        AttackEventChain.AddNode(node);
    }
    public void RemoveNodeFromAttackChain(IEventNode<EventContext> node)
    {
        AttackEventChain.RemoveNode(node);
    }

    // -----------------------------------------
    // Hit chain methods
    // -----------------------------------------
    public void ExecuteHitChain(ref EventContext context)
    {
        HitEventChain.Execute(context);
    }
    public void AddNodeToHitChain(IEventNode<EventContext> node)
    {
        HitEventChain.AddNode(node);
    }
    public void RemoveNodeFromHitChain(IEventNode<EventContext> node)
    {
        HitEventChain.RemoveNode(node);
    }

    // -----------------------------------------
    // CharacterDied chain methods
    // -----------------------------------------
    public void ExecuteCharacterDiedChain(ref CharacterDiedEventContext context)
    {
        CharacterDiedEventChain.Execute(context);
    }
    public void AddNodeToCharacterDiedChain(IEventNode<CharacterDiedEventContext> node)
    {
        CharacterDiedEventChain.AddNode(node);
    }
    public void RemoveNodeFromCharacterDiedChain(IEventNode<CharacterDiedEventContext> node)
    {
        CharacterDiedEventChain.RemoveNode(node);
    }

    // -----------------------------------------
    // NEW: PlayerHit chain methods
    // -----------------------------------------
    public void ExecutePlayerHitChain(ref EventContext context)
    {
        PlayerHitEventChain.Execute(context);
    }
    public void AddNodeToPlayerHitChain(IEventNode<EventContext> node)
    {
        PlayerHitEventChain.AddNode(node);
    }
    public void RemoveNodeFromPlayerHitChain(IEventNode<EventContext> node)
    {
        PlayerHitEventChain.RemoveNode(node);
    }
}
