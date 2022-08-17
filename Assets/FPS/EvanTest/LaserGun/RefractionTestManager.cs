using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionTestManager : MonoBehaviour
{
    public GameObject HitEffect;
    public GameObject StartEffect;
    private ParticleSystem[] Hit;
    public LineRenderer lineRenderer;
    public float range = 1000f;
    public float HitOffset = 0;
    [SerializeField]private int bounceSize;

    public bool isRefraction = false;
    
    // Start is called before the first frame update
    void Start()
    {

        bounceSize = lineRenderer.positionCount;
    }

    // Update is called once per frame
    void Update()
    {

            lineRenderer.enabled = isRefraction;
            HitEffect.SetActive(isRefraction);
            StartEffect.SetActive(isRefraction);

    }

    // could be replaced by isTriggerOn in Scriptable Object
    public void StartRefraction(Vector3 gunHitPoint, Vector3 gunDirection, bool isFiring)
    {

        isRefraction = isFiring;
        Vector3 lineStartPoint = lineRenderer.transform.position;
        Vector3 startPoint = gunHitPoint;
        Vector3 direction = Quaternion.AngleAxis(-120, Vector3.up) * gunDirection;

        lineRenderer.SetPosition(0, lineStartPoint);
 
        for (int i = 0; i < bounceSize; i++)
        {
            RaycastHit hit;

            Ray ray = new Ray(startPoint, direction);

            if (Physics.Raycast(ray, out hit, range))
            {
                startPoint = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
                lineRenderer.SetPosition(i + 1, hit.point);

                
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                HitEffect.transform.rotation = Quaternion.identity;

                // if (hit.collider.gameObject.tag == "Refraction")
                // {
                //     hit.collider.gameObject.GetComponent<RefractionTestManager>().StartRefraction(hit.point, direction);
                // }
                
                //hit.collider.gameObject.GetComponent<Refraction>().Refract();
                if (hit.collider.gameObject.tag != "Reflection")
                {
                    for (int j = i + 1; j < bounceSize; j++)
                    {
                        lineRenderer.SetPosition(j, hit.point);
                    }
                    break;
                }

                
            }
            else
            { //End laser position if doesn't collide with object
                    var EndPos = lineRenderer.transform.position + this.transform.forward * 10000;
                    lineRenderer.SetPosition(1, EndPos);
                    HitEffect.transform.position = EndPos;
                    HitEffect.SetActive(false);
            }
        }
    }
}
