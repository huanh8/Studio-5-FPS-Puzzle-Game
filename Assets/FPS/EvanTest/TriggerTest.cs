using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTest : MonoBehaviour
{
    [SerializeField] UnityEvent startTriggerEvent;
    [SerializeField] UnityEvent afterTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            startTriggerEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            afterTriggerEvent.Invoke();
        }
    }
    public void FireTrigger()
    {
        Debug.Log("FireTrigger");
        startTriggerEvent.Invoke();

    }
    public void FireAfterTrigger()
    {
        afterTriggerEvent.Invoke();
    }
}
