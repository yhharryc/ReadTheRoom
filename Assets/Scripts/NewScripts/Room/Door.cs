using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class Door : MonoBehaviour, IInteractable, IRoomObject
{
    private Room room;
    public Room Room { get { return room; } set { room = value; } }

    // We no longer store a single collider on the root.
    // Instead, we gather all colliders from LeftPart & RightPart.
    private List<Collider> doorColliders = new List<Collider>();

    private bool canInteract = true;
    public bool CanInteract { get { return canInteract; } }

    public GameObject LeftPart, RightPart;
    [HideInInspector]
    public GameObject DoorOpener;

    private bool isClosed = true;
    public bool IsClosed { get { return isClosed; } }

    private bool isFullyOpen = false;

    [Range(0f, 1f)]
    [SerializeField] private float targetOpenness = 0f;

    [SerializeField, Tooltip("Defines the angle at which the door is considered fully opened."), Range(80f, 179f)]
    private float maxDoorAngle = 120f;

    [SerializeField]
    private float doorOpenSpeed = 2f;

    public Vector3 DoorCenterPosition
    {
        get { return (LeftPart.transform.position + RightPart.transform.position) * 0.5f; }
    }

    [SerializeField, Range(0f, 1f)]
    private float doorOpenessPerInput = 0.1f;

    private Vector3 leftStartingLocalEulerAngles;
    private Vector3 rightStartingLocalEulerAngles;

    private bool compareX;
    private float relativePos;

    [Header("Door Events")]
    public Action OnDoorFullyOpened;
    public Action OnDoorFullyClosed;

    public float CurrentDoorAngle
    {
        get
        {
            if (LeftPart != null)
            {
                float tempAngle = LeftPart.transform.localEulerAngles.y;
                return (tempAngle > 180f) ? tempAngle - 360f : tempAngle;
            }
            Debug.LogWarning("LeftPart is null. Door script can't read angle properly.");
            return 0f;
        }
    }

    public float CurrentOpenness
    {
        get
        {
            return Mathf.Abs(CurrentDoorAngle) / maxDoorAngle;
        }
    }

    public float TargetOpenness
    {
        get { return targetOpenness; }
        set
        {
            targetOpenness = Mathf.Clamp01(value);
        }
    }

    private void Awake()
    {
        // No longer doing 'doorCollider = GetComponent<BoxCollider>()'
        // Instead, we will gather colliders in Start or after we have references to left/right parts.
    }

    private void Start()
    {
        // For now we assume there's a single local player
        DoorOpener = GameManager.Instance.PlayerCharacter.gameObject;
        InitializeDoor();

        // Gather all colliders from the left/right parts
        if (LeftPart != null)
        {
            var leftCols = LeftPart.GetComponentsInChildren<Collider>();
            doorColliders.AddRange(leftCols);
        }
        if (RightPart != null)
        {
            var rightCols = RightPart.GetComponentsInChildren<Collider>();
            doorColliders.AddRange(rightCols);
        }

        // Debug: show how many colliders we found
        // Debug.Log($"Door colliders found: {doorColliders.Count}");
    }

    private void FixedUpdate()
    {
        DoorUpdate();
    }

    private void DoorUpdate()
    {
        if (LeftPart != null && RightPart != null)
        {
            float currentAngle = CurrentDoorAngle;
            float targetAngle = CalculateTargetAngle();
            float tempOpenness = CurrentOpenness;

            if (isClosed && tempOpenness > 0f)
            {
                isClosed = false;
            }

            float opennessGap = Mathf.Abs(tempOpenness - targetOpenness);
            float dynamicSpeed = doorOpenSpeed * (1f + opennessGap);

            if (Mathf.Abs(Mathf.Abs(currentAngle) - Mathf.Abs(targetAngle)) > 0.2f)
            {
                float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.fixedDeltaTime * dynamicSpeed);

                Vector3 leftEuler = LeftPart.transform.localEulerAngles;
                leftEuler.y = newAngle;
                LeftPart.transform.localEulerAngles = leftEuler;

                Vector3 rightEuler = RightPart.transform.localEulerAngles;
                rightEuler.y = -newAngle;
                RightPart.transform.localEulerAngles = rightEuler;
            }
            else
            {
                // Reached or nearly reached the target angle
                if (targetOpenness >= 1f - 0.02f && !isFullyOpen)
                {
                    // Door is effectively open
                    SetDoorCollidersEnabled(false);
                    OnDoorFullyOpened?.Invoke();
                    isFullyOpen = true;
                }
                else if (!isClosed && tempOpenness <= 0.02f)
                {
                    // Fully closed
                    isClosed = true;
                    isFullyOpen = false;
                    Debug.Log("Door Fully Closed");

                    LeftPart.transform.localEulerAngles = leftStartingLocalEulerAngles;
                    RightPart.transform.localEulerAngles = rightStartingLocalEulerAngles;

                    SetDoorCollidersEnabled(true);
                    OnDoorFullyClosed?.Invoke();
                }
            }
        }
        else
        {
            Debug.LogWarning("LeftPart or RightPart is not assigned.");
        }
    }

    private float CalculateTargetAngle()
    {
        float signFactor;
        if (compareX)
            signFactor = Mathf.Sign(LeftPart.transform.position.z - RightPart.transform.position.z);
        else
            signFactor = Mathf.Sign(LeftPart.transform.position.x - RightPart.transform.position.x) * -1f;

        float angle = signFactor * relativePos * TargetOpenness * maxDoorAngle;
        return angle;
    }

    public void Interact(InteractInfo info)
    {
        Debug.Log("Door Interact called. The HFSM or sub-state logic will manage open/close.");
        SetRelativePosition();
        // We do not do direct input hooking here
    }

    private void SetRelativePosition()
    {
        if (DoorOpener != null && LeftPart != null)
        {
            relativePos = compareX
                ? (DoorOpener.transform.position.x > LeftPart.transform.position.x ? 1f : -1f)
                : (DoorOpener.transform.position.z > LeftPart.transform.position.z ? 1f : -1f);
        }
        else
        {
            Debug.LogWarning("DoorOpener or LeftPart is not assigned.");
        }
    }

    private void InitializeDoor()
    {
        if (LeftPart != null && RightPart != null)
        {
            leftStartingLocalEulerAngles = LeftPart.transform.localEulerAngles;
            rightStartingLocalEulerAngles = RightPart.transform.localEulerAngles;

            Vector3 leftPos = LeftPart.transform.position;
            Vector3 rightPos = RightPart.transform.position;

            bool isXDifferent = !Mathf.Approximately(leftPos.x, rightPos.x);
            bool isZDifferent = !Mathf.Approximately(leftPos.z, rightPos.z);

            switch ((isXDifferent, isZDifferent))
            {
                case (true, false):
                    compareX = false;
                    break;
                case (false, true):
                    compareX = true;
                    break;
                case (true, true):
                    Debug.LogError("Error: Both x and z positions differ. The code might not handle diagonal doors well.");
                    break;
                case (false, false):
                    Debug.LogError("Error: Both x and z positions are the same, the door parts overlap?");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("LeftPart or RightPart is not assigned in the inspector for Door script.");
        }
    }

    public void SetTargetOpenness(float newOpenness)
    {
        TargetOpenness = newOpenness; // clamp in [0..1]
    }

    public void OnCombatStartedInRoom(Room room)
    {
        TargetOpenness = 0f;
        canInteract = false;
        // Re-enable colliders so the door is physically closed
        SetDoorCollidersEnabled(true);
    }

    public void OnCombatEndedInRoom(Room room)
    {
        canInteract = true;
    }

    public bool IsFullyOpen { get { return isFullyOpen; } }

    public Vector3 GetPlayerStandPosition(float standDistance, bool inside = false)
    {
        Vector3 leftPos  = LeftPart.transform.position;
        Vector3 rightPos = RightPart.transform.position;

        Vector3 doorAxis = (rightPos - leftPos);
        doorAxis.y = 0f;
        doorAxis.Normalize();

        float signFactor;
        if (compareX)
            signFactor = Mathf.Sign(leftPos.z - rightPos.z);
        else
            signFactor = Mathf.Sign(leftPos.x - rightPos.x) * -1f;

        float side = signFactor * -relativePos;
        if (inside) side = -side;

        Vector3 doorPerp = Vector3.Cross(Vector3.up, doorAxis).normalized;
        doorPerp *= side;

        Vector3 standPos = DoorCenterPosition + doorPerp * standDistance;
        return standPos;
    }

    /// <summary>
    /// Enables/disables all colliders on the door parts.
    /// </summary>
    private void SetDoorCollidersEnabled(bool enabled)
    {
        foreach (var col in doorColliders)
        {
            if (col != null)
            {
                col.enabled = enabled;
            }
        }
    }
}
