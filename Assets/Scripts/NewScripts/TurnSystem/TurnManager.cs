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
            // Uncomment if you want this object to persist across scenes.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
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
        }
    }

    /// <summary>
    /// Begins the turn cycle.
    /// </summary>
    public void StartTurnCycle()
    {
        if (turnActors.Count == 0)
        {
            Debug.LogWarning("No turn actors registered in TurnManager!");
            return;
        }
        currentTurnIndex = 0;
        StartNextTurn();
    }

    /// <summary>
    /// Starts the turn for the next actor in the list.
    /// </summary>
    private void StartNextTurn()
    {
        if (turnActors.Count == 0)
            return;

        isTurnActive = true;
        TurnActor currentActor = turnActors[currentTurnIndex];
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
            return;

        TurnActor currentActor = turnActors[currentTurnIndex];
        currentActor.EndTurn();
        isTurnActive = false;

        // Cycle to the next actor.
        currentTurnIndex = (currentTurnIndex + 1) % turnActors.Count;
        StartNextTurn();
    }
    public void ClearAllActors()
    {
        turnActors.Clear();
        currentTurnIndex = 0;
        isTurnActive = false;
    }
}
