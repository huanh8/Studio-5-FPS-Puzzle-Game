using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioTrigger;
    public AudioSource audioSource;

    private bool newClip;
    
    // Start is called before the first frame update
    void Start()
    {
        newClip = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && newClip)
        {
            Debug.Log("In audio trigger");
            audioSource.Play();
            newClip = false;
        }
    }
}
