using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionManager : MonoBehaviour
{
    // Notice:
    //
    // The following code could be replaced, inherited or even deleted 
    // once the ScriptableObject branch is implemented / merged into this branch.
    //
    // Although this is a temporary manager, the following functions inside it
    // could still be used for ScriptableObject trigger to reflect the lights.
    // 

    public LineRenderer lineRenderer;
    public GameObject HitEffect;
    public GameObject StartEffect;
    public float range = 1000f;
    public float HitOffset = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        HitEffect.SetActive(false);
        StartEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reflect(Vector3 hitPoint, Vector3 gunDirection)
    {
        // Debug.Log("Reflect");
        lineRenderer.enabled = true;
        StartEffect.SetActive(true);

        RaycastHit hit;

            // Vector3 lineStartPoint = lineRenderer.transform.position;
            Vector3 lineStartPoint = hitPoint;
            // for refraction:
            Vector3 direction = Quaternion.AngleAxis(30, Vector3.up) * gunDirection;

        if (Physics.Raycast(lineStartPoint, direction, out hit, range))
            {
                // testing event trigger purpose
                // could be replaced by activating ScriptableObject trigger
                if (hit.collider.gameObject.GetComponent<Trigger>())
                    hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();
                
                lineRenderer.SetPosition(0, lineStartPoint);
                lineRenderer.SetPosition(1, hit.point);
                HitEffect.SetActive(true);
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                HitEffect.transform.rotation = Quaternion.identity;

                // Notice:
                // The following code could be replaced once the ScriptableObject is implemented:
                //
                // if ScriptableObject name == "Reflection",
                // set isTrigger = true
                // then do Reflect() in the Trigger script (or doTrigger() function)
                //
                // if (hit.collider.gameObject.tag == "Reflection")
                //     hit.collider.gameObject.GetComponent<ReflectionManager>().Reflect(hit.point, direction);

            }
            else
            { //End laser position if doesn't collide with object
                var EndPos = lineStartPoint + direction * 10000;

                lineRenderer.SetPosition(0, lineStartPoint);
                lineRenderer.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                HitEffect.SetActive(false);
            }
    }
}
