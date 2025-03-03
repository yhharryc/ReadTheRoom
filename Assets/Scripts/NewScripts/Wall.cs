using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour,IHitReceiver
{
    public GameObject Owner{get { return gameObject;} }
    public void OnHit(EventContext eventContext)
    {
        Debug.Log("Got Hit on " + eventContext.HitData.HitInfo.HitPoint);
    }
}
