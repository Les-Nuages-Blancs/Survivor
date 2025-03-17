using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] public UnityEvent onStart;

    void Start()
    {
        onStart.Invoke();
    }
}
