using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LavaFlask : MonoBehaviour
{

    [HideInInspector] public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    public float damage = 5f;
    [Tooltip("the size of the lava flask")]
    public float size = 3.0f;
    [Tooltip("How many times the lava will tick and deal dmg")]
    public int tickCount = 3;
    [Tooltip("Delay in second between dmg ticks")]
    public float tickDelay = 1.0f;
    private int tick = 0;


    private MeshRenderer meshRenderer;
    private Color originalColor;
    [Tooltip("the color in witch the lava flashes when dealing dmg")]
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 0.15f;
    
    //[SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>();
    //[SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();

    // Start is called before the first frame update
    void Start(){
        //init ref
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.GetColor("_LavaColor");

        //grow lava flask
        StartCoroutine(GrowScaleOverTime());

        //after growth, tick every 1s
        Invoke("TickDamage", tickDelay);
    }

    // Update is called once per frame
    void TickDamage(){
        //le sphere collider
        Collider[] colliders = Physics.OverlapSphere(transform.position, size);

        //when there are effect to apply it's not possile to ignore this. Depends if we choose to have effect from network autority
        //if(isReal) foreach(Collider other in colliders){
        foreach(Collider other in colliders){
            OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
            if (forwarder){
                GameObject target = forwarder.ForwardedGameObject;

                if (includeDamageTags.Contains(target.tag))
                {
                    //Debug.Log("HIIIIT");
                    float dmg = damage * LevelStateManager.Instance.PlayerDamageMultiplier;
                    
                    //Spawn effect client side : currently no effect on railgun
                    //HitEffects(dmg);

                    if(!isReal) break; //only the machine of the player who fired solve collision

                    HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                    if (healthSystem){         
                        GameNetworkManager.Instance.RequestDamage(healthSystem, dmg);

                    }
                }
            }
        }

        //animation to indicate it ticks
        //Note that this is probably borken if multiples persons has different CD for lava flask. Idk if unity uses material reference or unique so hum ...
        //well this is poor way to indicated dmg dealth anyways so it should be replaced by smth else in the end
        SetColorFlash();
        Invoke("SetColorOriginal", flashDuration);


        //check for final ticks. Destroy after a small delay so we see color.
        if(++tick == tickCount){
            Destroy(gameObject,flashDuration+0.07f);
            return;
        }

        Invoke("TickDamage", tickDelay);
    }

    void SetColorFlash(){
        meshRenderer.material.SetColor("_LavaColor", flashColor);
    }
    void SetColorOriginal(){
        meshRenderer.material.SetColor("_LavaColor", originalColor);
    }


     private IEnumerator GrowScaleOverTime()
    {
        float duration = tickDelay;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x * size, initialScale.y/2, initialScale.z * size);

        while (elapsedTime < duration){
            // Calculate the interpolation factor
            float t = elapsedTime / duration;
            
            // Smoothly interpolate between initial and target scale
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            
            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            
            // Wait for the next frame
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = targetScale;
    }
}
