using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDoor : GenericTriggerObject
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;

    public override void doTrigger()
    {
        //lastWaypointSwitchTime = Time.time;

        Vector3 startPosition = waypoints [currentWaypoint].transform.position;
        Vector3 endPosition = waypoints [currentWaypoint + 1].transform.position;

        Debug.Log(startPosition);
        Debug.Log(endPosition);

        float pathLength = Vector3.Distance (startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector3.Lerp (startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        if (gameObject.transform.position.Equals(endPosition)) 
        {
            if (currentWaypoint < waypoints.Length - 2)
            {
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
            }
        }
    }
}
