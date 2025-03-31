using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToasterEnemy : EnemyManager
{
    [SerializeField]
    private GameObject toastObject;
    [SerializeField]
    private const string BUTTON_NAME = "ToasterButton";
    public override void TakeDamage(EventContext context)
    {
        base.TakeDamage(context);
        MonoBehaviour mb = context.Target as MonoBehaviour;
        GameObject button = mb.gameObject;
        Debug.LogError(button.name);
    }
}
