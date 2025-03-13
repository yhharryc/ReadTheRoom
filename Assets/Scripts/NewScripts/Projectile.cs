using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool destroyOnHit = true;

    private float spawnTime;
    private bool hasHit=false;

    // We'll store a global flight direction
    private Vector3 flightDirection;

    // We'll store an EventContext if we want to pass it into the HitEventChain
    private EventContext eventContext;

    private ICharacter sourceCharacter;
    public ICharacter SourceCharacter{get{return sourceCharacter;}}
    private Vector3 startPosition;
    private Vector3 endPosition;
    


    private void Start()
    {
        /*
        Collider[] hits = Physics.OverlapSphere(transform.position, gameObject.GetComponent<SphereCollider>().radius, 13);
        if (hits.Length > 0) {
            Debug.LogError("its trrrruuueeee");
            OnTriggerEnter(hits[0]);
        }
        */
    }

    /// <summary>
    /// Called by the spawner to initialize data,
    /// including a global flight direction.
    /// </summary>
    public void Setup(EventContext context, Vector3 flightDir)
    {
        this.eventContext = context;
        this.sourceCharacter = context.Source;
        this.flightDirection = flightDir.normalized; // store and normalize it
        //Debug.Log("PROJECTILE: " + flightDirection);
        spawnTime = Time.time;
    }

    private void Update()
    {
        // Move along the global flightDirection
        transform.position += flightDirection * (speed * Time.deltaTime);

        // Destroy if lifetime expired
        if (Time.time - spawnTime > lifeTime)
        {
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Projectile")|| other.GetComponent<Projectile>()!=null)
        {
            return; 
        }
        if(hasHit)return;
        // 2) If it’s an IHitReceiver, we want to call the HitEventChain
        IHitReceiver hitReceiver = other.GetComponent<IHitReceiver>();
        if(hitReceiver==null) return;
        ICharacter hitCharacter = hitReceiver.Owner.GetComponent<ICharacter>();
        if (hitCharacter != null)
        {
            // 1) Avoid hitting the source (if you want that logic)
            if (hitCharacter== sourceCharacter )
            {
                
                return; 
            }
            if( hitCharacter.Faction ==sourceCharacter.Faction)
            {
                return;
            }
        }

            //Debug.Log("HIT" + other.gameObject.name);
            // If we have an eventContext from the weapon, let's reuse it
            if (eventContext != null)
            {
                // Fill out the HitData
                var hitInfo = new HitInfo {
                    HitPoint = transform.position,
                    HitNormal = -transform.forward,
                    AdditionalData = null
                };

                eventContext.HitData = new HitData {
                    HitInfo = hitInfo,
                    WasCrit = false,
                    IsLethalHit = false
                };
                eventContext.EventStarter = gameObject;
                eventContext.Target = hitReceiver;
                
                // Now call the HitEventChain
                EventChainManager.Instance.ExecuteHitChain(ref eventContext);
            }
            else
            {
                Debug.LogError("Projectile Destroyed before applying context");
                // If no eventContext, do a simplified approach:
                EventContext tempContext = new EventContext {
                    Source = sourceCharacter,
                    Target = hitReceiver,
                    HitData = new HitData {
                        HitInfo = new HitInfo {
                            HitPoint = transform.position,
                            HitNormal = -transform.forward
                        }
                    }
                };
                eventContext.EventStarter = gameObject;
                eventContext.Target = hitReceiver;
                
                // Then run the chain
                EventChainManager.Instance.ExecuteHitChain(ref tempContext);
            }

        endPosition = transform.position;

        // Optionally debug distance traveled, etc.
        // float distance = Vector3.Distance(startPosition, endPosition);
        hasHit = true;
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
        }


    
}
