using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DestroyAfterTimeLocal : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    [SerializeField] private UnityEvent callDestroy;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        callDestroy.Invoke();
    }

    public void BasicDestroy()
    {
        Destroy(gameObject);
    }
}
