using UnityEngine;

public abstract class TurnActor : MonoBehaviour
{
    /// <summary>
    /// Called by the TurnManager when this actor’s turn begins.
    /// </summary>
    public virtual void StartTurn()
    {
        isTurnComplete = false;
    }

    /// <summary>
    /// Called by the TurnManager when this actor’s turn ends.
    /// </summary>
    public virtual void EndTurn()
    {

    }

    /// <summary>
    /// Returns true when the actor has finished its turn actions.
    /// This can be used by the TurnManager to auto-advance turns.
    /// </summary>
    [SerializeField]
    protected bool isTurnComplete;
    public virtual bool IsTurnComplete { get; set;}

    public virtual bool IsActorTurn()
    {
        return TurnManager.Instance.IsActorTurn(this);
    }
}
