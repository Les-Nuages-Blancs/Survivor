using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEvent_example : MonoBehaviour, ILootEvent
{
    [SerializeField] GameObject drop1;
    [SerializeField] int count = 3;

    /*private Player killer;*/

    public void setPlayer(/*Player killer*/){
        /*this.killer = killer;*/
    }


    public void LootResolution()
    {
        Debug.Log("Résolution du lootEvent d'example ! !");
        for(int i = 0; i <= count; i++){
            float x = Random.Range(-0.5f, 0.5f);
            float y = 1 + Random.Range(-0.5f, 0.5f);
            float z = Random.Range(-0.5f, 0.5f);
            GameObject.Instantiate(drop1, transform.position + new Vector3(x,y,z), Quaternion.identity);
        }
    }


    void Start(){
        LootResolution();
        Destroy(gameObject);
    }
}
