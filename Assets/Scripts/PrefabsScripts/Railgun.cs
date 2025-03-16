using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; //for [TagField] (xD)
public class Railgun : MonoBehaviour
{    
    [HideInInspector] public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    [SerializeField] public float damage = 5f;
    [SerializeField] public float hitBoxEase = 8f;
    //[SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>();
    //[SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();


    // Start is called before the first frame update
    void Start(){
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(
            //10, 10, 40
            transform.localScale.x * hitBoxEase,
            transform.localScale.y * hitBoxEase,
            transform.localScale.z
            ),
            transform.rotation);
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
