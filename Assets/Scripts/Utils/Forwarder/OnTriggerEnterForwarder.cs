using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterForwarder : MonoBehaviour
{
    [SerializeField] private GameObject forwardedGameObject;

    public GameObject ForwardedGameObject => forwardedGameObject;
}
