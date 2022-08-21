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

    public void Print()
    {
        Debug.Log($"{triggerName}, {hitInfo}. Angle: {angle}, Charge time: {chargeTime}.");
    }
}
