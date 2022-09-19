using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleMove : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed;

    public void Update()
    {
        Vector3 endPosition = waypoints[(currentWaypoint + 1) % waypoints.Length].transform.position;
        float step = speed * Time.deltaTime;
        transform.LookAt(endPosition);
        transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        if(transform.position == endPosition)
        {
            currentWaypoint++;
        }
    }
}
