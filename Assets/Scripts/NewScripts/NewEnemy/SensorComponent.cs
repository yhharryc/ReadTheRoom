using UnityEngine;
using Micosmo.SensorToolkit; // ST2's namespace

public class SensorComponent : MonoBehaviour
{
    [Header("Pivot Settings")]
    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Vector3 pivotOffset;

    [Header("Sensors")]
    public RangeSensor rangeSensor;
    public LOSSensor losSensor;

    [Header("Detection")]
    public string playerTag = "Player";

    public bool playerInSight { get; private set; }
    public GameObject currentTarget { get; private set; }

    private void OnValidate()
    {
        if (pivotTransform != null)
        {
            transform.position = pivotTransform.position + pivotOffset;
        }
    }

    private void Start()
    {
        if (pivotTransform != null)
        {
            transform.SetParent(pivotTransform, true);
            transform.localPosition = Vector3.zero;
        }
    }

}
