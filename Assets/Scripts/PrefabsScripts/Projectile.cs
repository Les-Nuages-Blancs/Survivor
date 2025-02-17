using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Vitesse du projectile
    
    [HideInInspector] public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    [SerializeField] public float damage = 5f;
    //[SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>(); TODO !

    [SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();

    private bool fadingAway = false;

    
    void FixedUpdate()
    {
        //float fac = isReal ? .5f : 0.25f; //tmp debug to see if isreal is correctly set up todo remove
        transform.Translate(/*fac */ Vector3.forward * speed * Time.deltaTime); // D�placer vers l'avant
    }

    //collider venant du dmg system
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit step 0");
        if(fadingAway) return;

        //Debug.Log("hit step 1");
        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            //Debug.Log("hit step 2");
            GameObject target = forwarder.ForwardedGameObject;

            if (includeDamageTags.Contains(target.tag))
            {
                //Debug.Log("hit step 3");
                //if(!isReal) return; //only the machine of the player who fired solve collision

                //Debug.Log("hit step 4");
                HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                if (healthSystem)
                {
                    //Debug.Log("hit step 5");
                    GameNetworkManager.Instance.RequestDamage(healthSystem, damage);
                    Destroy(gameObject, 0.1f); //with a small delay because collider are big
                }
            }
        }

        //destroy projectile when hitting a wall
        else if ((includeTriggerLayers.value & (1 << other.gameObject.layer)) != 0) 
        { 
            Destroy(gameObject);
        }

    }
}