using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trigger", menuName = "Trigger")]
public class Trigger : ScriptableObject
{
    public string triggerName;

    // testing purpose
    public string hitInfo;

    public int angle;
    public int chargeTime;
    public bool isTriggerOn;

    public void print()
    {
        Debug.Log(triggerName + "," + hitInfo);
    }
}
