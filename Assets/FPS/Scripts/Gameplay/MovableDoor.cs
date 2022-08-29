using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDoor : GenericTriggerObject
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public override bool shouldLoop {get; set;}

    public override void doTrigger()
    {
        shouldLoop = false;
        Vector3 endPosition;
        if(shouldLoop)
        {
            endPosition = waypoints[(currentWaypoint + 1) % waypoints.Length].transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
            if(transform.position == endPosition)
            {
                currentWaypoint++;
            }
        }
        else {
            endPosition = waypoints[currentWaypoint + 1].transform.position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
            if(transform.position == endPosition)
            {
                if(currentWaypoint < waypoints.Length - 2)
                {
                    currentWaypoint++;
                }
            }
        }
    }
}
