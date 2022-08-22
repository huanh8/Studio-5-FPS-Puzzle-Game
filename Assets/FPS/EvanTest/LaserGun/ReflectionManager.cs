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

    private LineRenderer[] lineRenderer;
    private GameObject[] HitEffect;

    private float range = 1000f;
    public float HitOffset = 0;
    public int ANGLE = 60;

    void Start()
    {
        SetLineRenderer();
        SetTriggerOff();
    }
    void SetLineRenderer()
    {
        lineRenderer = new LineRenderer[transform.childCount];
        HitEffect = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<LineRenderer>())
            {
                lineRenderer[i] = transform.GetChild(i).GetComponent<LineRenderer>();
                HitEffect[i] = transform.GetChild(i).GetChild(0).gameObject;
            }
        }
    }
    public void SetTriggerOn(RaycastHit hitPoint, Vector3 gunDirection, string type)
    {
        Reflect(hitPoint, gunDirection, type);
    }
    public void SetTriggerOff()
    {
        for (int i = 0; i < lineRenderer.Length; i++)
        {
            lineRenderer[i].enabled = false;
            HitEffect[i].SetActive(false);
        }
    }

    public Vector3 ReflectionDirection(Vector3 gunDirection, Vector3 hitNormal, string type, int lr = 0)
    {
        if (type == "Lens")
            return gunDirection;
        else if (type == "Reflection")
            return Vector3.Reflect(gunDirection, hitNormal);
        else
        {
            if (lr == 0)
                return Quaternion.AngleAxis(ANGLE, Vector3.down) * gunDirection;
            else
                return Quaternion.AngleAxis(-ANGLE, Vector3.down) * gunDirection;
        }
    }

    public void Reflect(RaycastHit hitPoint, Vector3 gunDirection, string type)
    {

        for (int i = 0; i < lineRenderer.Length; i++)
        {
            RaycastHit hit;
            lineRenderer[i].enabled = true;
            Vector3 lineStartPoint = hitPoint.point;

            Vector3 direction = ReflectionDirection(gunDirection, hitPoint.normal, type, i);

            if (Physics.Raycast(lineStartPoint, direction, out hit, range))
            {

                if (hit.collider.gameObject.GetComponent<Trigger>())
                    hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();

                lineRenderer[i].SetPosition(0, lineStartPoint);
                lineRenderer[i].SetPosition(1, hit.point);
                HitEffect[i].SetActive(true);
                HitEffect[i].transform.position = hit.point + hit.normal * HitOffset;
                HitEffect[i].transform.rotation = Quaternion.identity;
            }

        }
    }
}
