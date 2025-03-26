using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    
    public static CombatManager Instance { get; private set; }
    

    [Tooltip("Reference to the player actor if you want to include them in the turn system.")]
    [SerializeField] private PlayerCharacter playerActor;
    private bool isCombatStarted = false;
    public bool IsCombatStarted{get{return isCombatStarted;}}
    // You may keep track of which enemies are currently in active combat
    private List<EnemyManager> activeEnemies = new List<EnemyManager>();

    private void Awake()
    {
        // Singleton setup.
        if (Instance == null)
        {
            Instance = this;
            // Uncomment if you want this object to persist across scenes.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        

        if (playerActor == null)
        {
            // Optional: find or store reference to Player
            playerActor = GameObject.FindObjectOfType<PlayerCharacter>();
        }
    }

    private void OnEnable()
    {
        // Option A: If you already have references to all Rooms, you can subscribe here:
        // e.g., foreach (var room in allRooms) { SubscribeToRoom(room); }
        //
        // Or if Rooms are created dynamically, each Room can do:
        //   
    }

    private void OnDisable()
    {
        // Unsubscribe from any events if needed
    }

    /// <summary>
    /// Called by a Room (in Start or OnEnable) so that the CombatManager can subscribe
    /// to its OnCombatStarted/OnCombatEnded events.
    /// </summary>
    public void RegisterRoom(Room room)
    {
        if (room == null) return;
        Debug.Log($"CombatManager: '{room.name}' is Registered.");
        //room.OnCombatStartedInRoom  += Instance.OnCombatStarted;
        //room.OnCombatEndedInRoom    += OnCombatEnded;
    }

    /// <summary>
    /// Unsubscribe if the Room is destroyed or you no longer need it.
    /// </summary>
    public void UnregisterRoom(Room room)
    {
        if (room == null) return;
        //room.OnCombatStartedInRoom  -= OnCombatStarted;
        //room.OnCombatEndedInRoom    -= OnCombatEnded;
    }

    /// <summary>
    /// Called when the Room's combat starts. We'll register the enemies + (optionally) the player with TurnManager.
    /// </summary>
    public void OnCombatStarted(Room room)
    {
        Debug.Log($"CombatManager: Combat started in room '{room.name}'.");
        isCombatStarted = true;
        // 1) Gather all enemies from the room
        var enemies = room.GetEnemies(); 
        // (If your Room code has a public method or property that returns the list of EnemyManagers)
        if (playerActor != null)
        {
            TurnManager.Instance.RegisterTurnActor(playerActor);
        }
        // 2) Register them with TurnManager
        foreach (var enemy in enemies)
        {
            TurnManager.Instance.RegisterTurnActor(enemy);
            activeEnemies.Add(enemy);
        }

        // 3) Also register the Player if you want them to be part of the turn cycle


        // 4) Start the turn cycle if not already running
        TurnManager.Instance.StartTurnCycle();
    }

    /// <summary>
    /// Called when the Room's combat ends. We'll remove those enemies from TurnManager.
    /// Optionally remove the player as well, or keep them if you want them always in the turn list.
    /// </summary>
    private void OnCombatEnded(Room room)
    {
        Debug.Log($"CombatManager: Combat ended in room '{room.name}'.");
        isCombatStarted = false;
        // 1) Gather all enemies from the room
        var enemies = room.GetEnemies();

        // 2) Unregister them from TurnManager
        foreach (var enemy in enemies)
        {
            TurnManager.Instance.UnregisterTurnActor(enemy);
            activeEnemies.Remove(enemy);
        }

        // 3) Optionally remove the player
        if (playerActor != null)
        {
            TurnManager.Instance.UnregisterTurnActor(playerActor);
        }

        // 4) If you want to forcibly end the turn cycle:
        // (But typically the turn manager will keep going if there are other actors in it.)
        // If you want a "StopTurnCycle()" or "ClearAllActors()" you can add that to TurnManager.
        TurnManager.Instance.ClearAllActors();
    }
}
