using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

/*
Note : very little abstraction in this class because we only needed one projectiles

Should we ever need more than 3 other projectile this should be refactored. (dmg dealing , effect based on layer ...)
*/

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Vitesse du projectile
    
    [HideInInspector] public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    [SerializeField] public float damage = 5f;
    [SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>();
    

    [SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private LerpLightIntensity lightLerp;

    private bool fadingAway = false;
    private bool isDestroyed = false; // Flag to stop movement when destroying


    void FixedUpdate()
    {
        if (isDestroyed) return; // Prevent movement when destroying

        //float fac = isReal ? .5f : 0.25f; //tmp debug to see if isreal is correctly set up todo remove
        transform.Translate(/*fac */ Vector3.forward * speed * Time.deltaTime); // D�placer vers l'avant
    }

    void HitEffects(float dmg){
        foreach( var effect in EffectsPrefab)
        {
            GameObject go = Instantiate(effect, transform.position, transform.rotation, LevelStateManager.Instance.OtherParent);
            DamageValueForward damageValueForward = go.GetComponent<DamageValueForward>();
            if (damageValueForward != null){
                damageValueForward.damageValue = dmg;
            }
        }
    }

    public void DestroyProjectile()
    {
        if (isDestroyed) return;

        isDestroyed = true;
        if (vfx != null)
        {
            vfx.Stop(); // Stop spawning new particles
        }
        if (lightLerp != null)
        {
            lightLerp.StartLerp();
        }
        Destroy(gameObject, 3f);
    }

    //collider venant du dmg system
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit step 0");
        if(fadingAway) return;
        if (isDestroyed) return; // Prevent hit when destroying

        //Debug.Log("hit step 1");
        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            //Debug.Log("hit step 2");
            GameObject target = forwarder.ForwardedGameObject;

            if (includeDamageTags.Contains(target.tag))
            {

                float dmg = damage * LevelStateManager.Instance.PlayerDamageMultiplier;
                //Spawn effect client side
                HitEffects(dmg);

                if(!isReal) return; //only the machine of the player who fired solve collision

                HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                if (healthSystem)
                {                    
                    GameNetworkManager.Instance.RequestDamage(healthSystem, dmg);
                    //with a small delay because collider are big

                    DestroyProjectile();
                }
            }
        }

        //destroy projectile when hitting a wall
        else if ((includeTriggerLayers.value & (1 << other.gameObject.layer)) != 0) 
        {
            DestroyProjectile();
        }
    }
}