using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleLook : MonoBehaviour
{
    public Transform head;

    public bool Pause = false;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            StartCoroutine(TurnHead(other.transform, 1.0f));
        }
    }

    IEnumerator TurnHead(Transform other, float timeToMove)
    {
        WaitForSeconds Wait = new WaitForSeconds(1f / 60);

        //while(true)
        //{
            head.transform.LookAt(other);
            yield return null;
        //}
    }
}
