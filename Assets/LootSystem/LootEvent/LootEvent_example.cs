using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEvent_example : MonoBehaviour, ILootEvent
{
    [SerializeField] GameObject drop1;
    [SerializeField] GameObject drop2;

    /*private Player killer;*/

    public void setPlayer(/*Player killer*/){
        /*this.killer = killer;*/
    }


    public void LootResolution()
    {
        Debug.Log("Résolution du lootEvent d'example ! !");
        GameObject.Instantiate(drop1, transform.position, Quaternion.identity);
        GameObject.Instantiate(drop2, transform.position, Quaternion.identity);
    }


    void Start(){
        LootResolution();
        Destroy(gameObject);
    }
}
