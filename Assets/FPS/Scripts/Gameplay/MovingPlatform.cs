using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : GenericTriggerObject
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed;
    public override bool shouldLoop {get; set;}

    public override void doTrigger()
    {
        shouldLoop = true;
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
    
    //Allow player to move with platform
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other);
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other) 
    {
        other.transform.SetParent(null);
    }
}
