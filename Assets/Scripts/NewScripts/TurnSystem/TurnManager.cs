using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    // List of actors participating in the turn-based system.
    private List<TurnActor> turnActors = new List<TurnActor>();

    // Keeps track of whose turn it is.
    private int currentTurnIndex = 0;
    private bool isTurnActive = false;

    private void Awake()
    {
        // Singleton setup.
        if (Instance == null)
        {
            Instance = this;
            // Uncomment if you want this object to persist across scenes:
            // DontDestroyOnLoad(gameObject);
            Debug.Log("[TurnManager] Singleton instance created.");
        }
        else
        {
            Debug.LogWarning("[TurnManager] Duplicate instance detected and destroyed.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Registers a new actor to the turn order.
    /// </summary>
    public void RegisterTurnActor(TurnActor actor)
    {
        if (!turnActors.Contains(actor))
        {
            turnActors.Add(actor);
            Debug.Log($"[TurnManager] Registered TurnActor '{actor.name}'. Total now: {turnActors.Count}");
        }
        else
        {
            Debug.LogWarning($"[TurnManager] Attempted to register TurnActor '{actor.name}' but it's already in the list.");
        }
    }

    /// <summary>
    /// Unregisters an actor from the turn order.
    /// </summary>
    public void UnregisterTurnActor(TurnActor actor)
    {
        if (turnActors.Contains(actor))
        {
            turnActors.Remove(actor);
            Debug.Log($"[TurnManager] Unregistered TurnActor '{actor.name}'. Remaining: {turnActors.Count}");
        }
        else
        {
            Debug.LogWarning($"[TurnManager] Attempted to unregister TurnActor '{actor.name}' but it wasn't found.");
        }
    }

    /// <summary>
    /// Begins the turn cycle.
    /// </summary>
    public void StartTurnCycle()
    {
        if (turnActors.Count == 0)
        {
            Debug.LogWarning("[TurnManager] No turn actors registered! Cannot start turn cycle.");
            return;
        }
        currentTurnIndex = 0;
        Debug.Log($"[TurnManager] Starting turn cycle. Total actors: {turnActors.Count}");
        StartNextTurn();
    }

    /// <summary>
    /// Starts the turn for the next actor in the list.
    /// </summary>
    private void StartNextTurn()
    {
        if (turnActors.Count == 0)
        {
            Debug.LogWarning("[TurnManager] No turn actors available. Cannot start next turn.");
            return;
        }

        isTurnActive = true;
        TurnActor currentActor = turnActors[currentTurnIndex];
        Debug.Log($"[TurnManager] Starting turn for actor '{currentActor.name}' at index {currentTurnIndex}.");
        currentActor.StartTurn();
    }

    /// <summary>
    /// Ends the current turn and moves to the next actor.
    /// This can be called either by the actor itself (when it completes its actions)
    /// or manually to force the turn change.
    /// </summary>
    public void EndCurrentTurn()
    {
        if (!isTurnActive)
        {
            Debug.LogWarning("[TurnManager] EndCurrentTurn called but no turn is active.");
            return;
        }

        TurnActor currentActor = turnActors[currentTurnIndex];
        currentActor.EndTurn();
        Debug.Log($"[TurnManager] Ended turn for actor '{currentActor.name}' at index {currentTurnIndex}.");

        isTurnActive = false;

        // Cycle to the next actor.
        currentTurnIndex = (currentTurnIndex + 1) % turnActors.Count;
        Debug.Log($"[TurnManager] Now advancing to next actor index {currentTurnIndex}.");
        StartNextTurn();
    }

    /// <summary>
    /// Clears all registered actors, resetting the turn manager.
    /// </summary>
    public void ClearAllActors()
    {
        turnActors.Clear();
        currentTurnIndex = 0;
        isTurnActive = false;
        Debug.Log("[TurnManager] Cleared all actors and reset turn manager state.");
    }

    public bool IsActorTurn(TurnActor turnActor)
    {
        if(!CombatManager.Instance.IsCombatStarted)
        return true;
        TurnActor currentActor = Instance.turnActors[currentTurnIndex];
        if (currentActor == turnActor)
        {
            return true;
        }
        return false;
    }
}
