using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayUnityEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onDelayedEvent;
    [SerializeField] private float delayTime = 1f;

    private Coroutine delayCoroutine;

    public void StartDelayEvent()
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }
        delayCoroutine = StartCoroutine(DelayRoutine());
    }

    private IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(delayTime);
        onDelayedEvent?.Invoke();
    }
}