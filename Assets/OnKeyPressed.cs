using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnKeyPressed : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode = KeyCode.Tab;
    [SerializeField] public UnityEvent onKeyDown;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            onKeyDown.Invoke();
        }
    }
}
