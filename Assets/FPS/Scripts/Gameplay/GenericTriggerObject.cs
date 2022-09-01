using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericTriggerObject : MonoBehaviour
{
    public abstract bool shouldLoop {get; set;}
    public abstract void doTrigger();
    
}
