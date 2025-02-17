using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : NetworkBehaviour
{
    //A singleton that handles FakeNetworkObject (also called game object which we'll refer to as FNO) communication to network

    public static GameNetworkManager Instance {get; private set;}

    void Awake()
    {
        if(Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    public void RequestDamage(HealthSystem healthSystem, float damage){
        healthSystem.TakeDamageServerRPC(damage);
    }


}
