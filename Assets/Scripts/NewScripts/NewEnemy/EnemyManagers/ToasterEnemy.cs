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
        
        if (button.name == BUTTON_NAME)
        {
            PopToast();
        }
    }

    public void PopToast()
    {   
        
        if (toastObject == null)
        {
            Debug.LogWarning("Toast object not assigned.");
            return;
        }
        Rigidbody rb = toastObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = toastObject.AddComponent<Rigidbody>();
        }
        if(rb.isKinematic = false)
        {
            //Already popping
            return;
        }
        // Ensure it's using gravity and not kinematic
        rb.isKinematic = false;
        rb.useGravity = true;

        // Clear any existing velocity just in case
        rb.linearVelocity = Vector3.zero;

        // Apply upward force
        float popForce = 6f; // tweak this number to get the right feel
        rb.AddForce(Vector3.up * popForce, ForceMode.Impulse);
    }
    IEnumerator ResetToastPosition()
    {
        yield return new WaitForSeconds(2f); // wait for the jump to finish
        Rigidbody rb = toastObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //Destroy(rb); // remove physics
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        toastObject.transform.localPosition = Vector3.zero;
        toastObject.transform.localRotation = Quaternion.identity;
    }

    public override Unity.Behavior.Node.Status OnIdleBehavior(int behaviorID = 0)
    {
        PopToast();
        return Unity.Behavior.Node.Status.Success;
    }

}
