using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;

    public void OpenDoor()
    {
        animator.SetBool("isOpen", true);
    }
}
