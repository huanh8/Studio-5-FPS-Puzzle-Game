using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDoor : GenericTriggerObject
{
    public Transform waypoint;
    public float step;

    public override void doTrigger()
    {
        for (int i = 0; i < 45; i++)
        {
            if (transform.position.y <= waypoint.position.y)
                transform.Translate(Vector3.up * Time.deltaTime);
        }
    }
}
