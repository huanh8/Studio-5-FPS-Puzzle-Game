using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent myTriggerEvent;

    //collider with raycast

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Player")
        {
            myTriggerEvent.Invoke();
        }
    }
    public void FireTrigger()
    {
        myTriggerEvent.Invoke();
    }
}
