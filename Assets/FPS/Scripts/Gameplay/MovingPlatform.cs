using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath waypointPath;
    [SerializeField] private float speed;
    public bool canMove;

    private int targetWaypointIndex;
    private Transform prevWaypoint;
    private Transform targetWaypoint;
    private float timeToWaypoint;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove == true)
        {
            elapsedTime += Time.deltaTime;

            float elapsedPercentage = elapsedTime / timeToWaypoint;
            transform.position = Vector3.Lerp(prevWaypoint.position, targetWaypoint.position, elapsedPercentage);

            if(elapsedPercentage >= 1)
            {
                TargetNextWaypoint();
            }
        }
    }

    private void TargetNextWaypoint()
    {
        prevWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);
        targetWaypointIndex = waypointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(prevWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other) 
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other) 
    {
        other.transform.SetParent(null);
    }
}
