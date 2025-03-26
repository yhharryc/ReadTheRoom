using UnityEngine;
using System;

public class TimedTurnComponent : MonoBehaviour
{
    [Header("Turn Duration")]
    [SerializeField]
    private float defaultTurnDuration = 5f;

    private float timeRemaining;
    private bool timerActive;
    public bool TimerActive{get{return timerActive;}}
    private AbilitySystemComponent playerASC;
    /// <summary>
    /// Fires when the turn timer finishes or is manually ended.
    /// </summary>
    public event Action OnTurnComplete;
    private void Awake()
    {
        playerASC = GetComponent<AbilitySystemComponent>();
    }
    /// <summary>
    /// Begins (or restarts) the turn timer with a custom duration.
    /// If you don’t provide a duration, it uses defaultTurnDuration.
    /// </summary>
    public void BeginTimedTurn(float duration = -1f)
    {
        if (duration <= 0f)
        {
            duration = defaultTurnDuration;
        }
        
        timeRemaining = duration;
        playerASC.SetAttributeBaseValue("TurnLength", timeRemaining, out bool foundAttribute);
        timerActive   = true;
    }

    /// <summary>
    /// Manually ends the turn timer and invokes the OnTurnComplete event.
    /// </summary>
    public void EndTimedTurn()
    {
        timerActive   = false;
        timeRemaining = 0f;

        OnTurnComplete?.Invoke();
    }

    private void Update()
    {
        if (!timerActive) return;

        timeRemaining -= Time.deltaTime;
        playerASC.SetAttributeBaseValue("TurnLength", timeRemaining, out bool foundAttribute);
        if (timeRemaining <= 0f)
        {
            EndTimedTurn();
        }
    }
}
