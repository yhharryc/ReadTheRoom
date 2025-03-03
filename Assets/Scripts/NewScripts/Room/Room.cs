using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    [Header("Room Objects")]
    [SerializeField]
    private List<Door> doors;  // Keeping doors as is, or you could unify them too
    [SerializeField]
    private List<EnemyManager> enemies;  // Now referencing the new EnemyManager

    [SerializeField]
    private List<ArtifactSO> rewardList;

    private int remainingEnemyNum;
    public event Action<Room> OnCombatStartedInRoom;
    public event Action<Room> OnCombatEndedInRoom;

    private bool hasCombatEncounter = true;
    private bool isCombatActive = false;

    // We remove `roomClearingScore` since we no longer rely on ScoreOnKill in EnemyManager
    // private float roomClearingScore = 0f;

    void Start()
    {
        InitializeRoomObjects();
    }

    /// <summary>
    /// Kicks off the combat scenario in this room.
    /// </summary>
    public void StartCombat()
    {
        if (isCombatActive || !hasCombatEncounter) return;
        isCombatActive = true;

        OnCombatStartedInRoom?.Invoke(this);

        Debug.Log($"[Room] Combat has started in '{name}'!");
    }

    /// <summary>
    /// Subscribes the Doors and EnemyManagers to the appropriate events.
    /// </summary>
    private void InitializeRoomObjects()
    {
        // 1) Doors
        foreach (var door in doors)
        {
            door.Room = this;
            OnCombatStartedInRoom += door.OnCombatStartedInRoom;
            door.OnDoorFullyOpened += OnDoorFullyOpened;
            OnCombatEndedInRoom += (Room r) => door.OnDoorFullyClosed();
        }

        // 2) Enemies (EnemyManager references)
        foreach (var enemyManager in enemies)
        {
            // If it also implements IRoomObject, you could do:
            OnCombatStartedInRoom += enemyManager.OnCombatStartedInRoom;
            OnCombatEndedInRoom   += enemyManager.OnCombatEndedInRoom;

            // If it’s an ICharacter, we can track death:
            enemyManager.OnCharacterDied += OnEnemyManagerDied;
        }

        remainingEnemyNum = enemies.Count;
        Debug.Log($"[Room] Initialized {doors.Count} doors, {remainingEnemyNum} enemies in '{name}'");
    }

    private void OnDoorFullyOpened()
    {
        // Optionally, we wait 1 second to start combat
        StartCoroutine(DelayedStartCombat());
    }

    private IEnumerator DelayedStartCombat()
    {
        yield return new WaitForSeconds(1f);
        StartCombat();
    }

    /// <summary>
    /// Called when an EnemyManager’s OnCharacterDied event fires.
    /// We reduce the count, check if the room is cleared, etc.
    /// </summary>
    private void OnEnemyManagerDied(ICharacter dyingCharacter)
    {
        dyingCharacter.OnCharacterDied -= OnEnemyManagerDied;

        // If we want to confirm it's actually in our 'enemies' list, we can do so.
        // For now we just reduce the counter:
        remainingEnemyNum--;
        Debug.Log($"[Room] An EnemyManager died in '{name}'. Enemies left: {remainingEnemyNum}");

        if (remainingEnemyNum <= 0)
        {
            Debug.Log($"[Room] Room '{name}' is cleared!");
            OnCombatEndedInRoom?.Invoke(this);
            // If you used to do something like AddScore(roomClearingScore), remove or replace that logic
        }
    }

    /// <summary>
    /// Dynamically adds an EnemyManager to the room at runtime.
    /// </summary>
    public void AddEnemyToRoom(EnemyManager newEnemy)
    {
        if (newEnemy == null) return;
        enemies.Add(newEnemy);
        newEnemy.OnCharacterDied += OnEnemyManagerDied;

        remainingEnemyNum++;
        Debug.Log($"[Room] EnemyManager '{newEnemy.name}' added to '{name}'. Total: {remainingEnemyNum}");
    }
}
