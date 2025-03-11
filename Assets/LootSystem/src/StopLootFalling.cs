using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*

This script is an unholy hack and a disrepect to every good practice

However, it's likely :
- the quickest way to acheive what I want
- the most optimize computational and network solution to a problem that prevents the implementations of a cool effect

Thus I'm going with it



This script store Y coordinate at start and turns the rigid body kinematic after
*/

public class StopLootFalling : MonoBehaviour
{

    private float startY =-9999;
    private Rigidbody rgb;

    // Start is called before the first frame update
    void Start(){
        startY = transform.position.y;
        rgb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(transform.position.y < startY){
            rgb.isKinematic = true;
            Destroy(this);
        }
    }
}
