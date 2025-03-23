using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements; //for [TagField] (xD)
public class Railgun : MonoBehaviour
{    
    [HideInInspector] public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    [SerializeField] public float damage = 5f;
    
    //[SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>();
    //[SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();

    [Tooltip("warning : must match the visual lenght of railgun vfx !")] //doing it manually would be a waste at runtime. Unless the lenghts can change at runtime then another logic would be responsible for controlling the lenght of the vfx and this dmg dealing script
    public float length = 18;
    public float delayBeforeDamage = 0.25f;
    private float width = 0.5f;


    // Start is called before the first frame update
    void Start(){
        Invoke("DealDamage", delayBeforeDamage);
    }

    void DealDamage(){
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * length/2, new Vector3(
            //10, 10, 40
            width,
            width+2,
            length / 2 //it's half size of the box
            ),
            transform.rotation);
        foreach(Collider other in colliders){
            OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
            if (forwarder){
                GameObject target = forwarder.ForwardedGameObject;

                if (includeDamageTags.Contains(target.tag))
                {
                    // Debug.Log("railgunHIIIT");
                    float dmg = damage * LevelStateManager.Instance.PlayerDamageMultiplier;
                    
                    //Spawn effect client side : currently no effect on railgun
                    //HitEffects(dmg);

                    if(!isReal) return; //only the machine of the player who fired solve collision

                    HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                    if (healthSystem){         
                        GameNetworkManager.Instance.RequestDamage(healthSystem, dmg);

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
