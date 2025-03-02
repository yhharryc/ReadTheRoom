using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class Room : MonoBehaviour
{
    [SerializeField] public List<Door> doors;

    // Replaces the old `List<Enemy>` with a generic list of map objects
    [SerializeField] public List<MapObject> enemyMapObjects;

    [SerializeField] private List<ArtifactSO> rewardList;

    // Number of destroyable objects (those implementing ICharacter) left
    private int remainingDestroyableNum;

    public event Action<Room> OnCombatStartedInRoom;
    public event Action<Room> OnCombatEndedInRoom;

    private bool hasCombatEncounter = true;
    private bool isCombatActive = false;

    // We’ll accumulate the total “score on destroy” from all enemyMapObjects
    private float roomClearingScore = 0f;

    void Start()
    {
        InitializeDoors();
        InitializeMapObjects();
    }

    public void StartCombat()
    {
        if (isCombatActive || !hasCombatEncounter) return;
        isCombatActive = true;

        OnCombatStartedInRoom?.Invoke(this);

        Debug.Log("Combat has started!");
    }

    private void InitializeDoors()
    {
        foreach (Door door in doors)
        {
            door.Room = this;
            OnCombatStartedInRoom += door.OnCombatStartedInRoom;
            door.OnDoorFullyOpened += OnDoorFullyOpened;
            OnCombatEndedInRoom   += door.OnDoorFullyClosed; 
            // Or OnDoorFullyClosed if you have that event
        }
    }

    private void InitializeMapObjects()
    {
        // For each MapObject:
        //  1) Subscribe to OnCombatStartedInRoom / OnCombatEndedInRoom
        //  2) If it implements ICharacter, subscribe to OnCharacterDied
        //  3) Accumulate ScoreOnDestroy, increment a counter

        foreach (MapObject mo in enemyMapObjects)
        {
            // 1) Hook the IRoomObject events
            OnCombatStartedInRoom += mo.OnCombatStartedInRoom;
            OnCombatEndedInRoom   += mo.OnCombatEndedInRoom;

            // 2) If the object is also an ICharacter => it can die
            if (mo is ICharacter characterObj)
            {
                characterObj.OnCharacterDied += OnMapObjectDied;

                // 3) Accumulate its ScoreOnDestroy
                roomClearingScore += mo.ScoreOnDestroy;
                remainingDestroyableNum++;
            }
        }
    }

    private void OnDoorFullyOpened()
    {
        // Delay the actual StartCombat call
        StartCoroutine(DelayedStartCombat());
    }

    private IEnumerator DelayedStartCombat()
    {
        yield return new WaitForSeconds(1f);
        StartCombat();
    }

    private void OnMapObjectDied(ICharacter dyingCharacter)
    {
        // Unsubscribe so we don’t get repeated calls
        dyingCharacter.OnCharacterDied -= OnMapObjectDied;

        // We know we stored them as `MapObject`, so cast:
        var mo = dyingCharacter as MapObject;
        if (mo != null)
        {
            remainingDestroyableNum--;
            Debug.Log($"{mo.name} died/destroyed. Remaining: {remainingDestroyableNum}");
        }
        else
        {
            Debug.LogWarning("Tried to remove a non-MapObject from the list.");
        }

        // If no more destroyable objects remain, the room is cleared
        if (remainingDestroyableNum == 0)
        {
            Debug.Log("Room is Cleared!");
            OnCombatEndedInRoom?.Invoke(this);
            GameManager.Instance.AddScore(roomClearingScore);
        }
    }

    // If you need a method to add new map objects at runtime:
    public void AddMapObject(MapObject mo)
    {
        // Hook up events
        OnCombatStartedInRoom += mo.OnCombatStartedInRoom;
        OnCombatEndedInRoom   += mo.OnCombatEndedInRoom;

        if (mo is ICharacter characterObj)
        {
            characterObj.OnCharacterDied += OnMapObjectDied;
            roomClearingScore += mo.ScoreOnDestroy;
            remainingDestroyableNum++;
        }

        enemyMapObjects.Add(mo);
    }
}
