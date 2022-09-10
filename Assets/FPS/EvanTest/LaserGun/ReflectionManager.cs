using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;

namespace Unity.FPS.Gameplay
{
    public class ReflectionManager : MonoBehaviour
    {
        public Trigger trigger;
        private ReflectionManager triggerObject;
        private ReflectionManager preTriggerObject;
        private LineRenderer[] lineRenderer;
        private GameObject[] HitEffect;
        private float range = 1000f;
        public float HitOffset = 0;
        void Start()
        {
            SetLineRenderer();
            SetTriggerOff();
            trigger.isTriggerOn = false;
        }

        // void Update()
        // {
        //     if (trigger.isTriggerOn)
        //     {
        //         Reflect(hitPoint, gunDirection, type, currentColor);
        //     }
        // }

        void SetLineRenderer()
        {
            lineRenderer = new LineRenderer[transform.childCount];
            HitEffect = new GameObject[transform.childCount];

            //triggerObjects = new List<ReflectionManager>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<LineRenderer>())
                {
                    lineRenderer[i] = transform.GetChild(i).GetComponent<LineRenderer>();
                    HitEffect[i] = transform.GetChild(i).GetChild(0).gameObject;
                }
            }
        }

        [System.Obsolete]
        public void SetTriggerOn(RaycastHit hitPoint, Vector3 gunDirection, LaserColor currentColor)
        {
            Reflect(hitPoint, gunDirection, currentColor);
        }
        public void SetTriggerOff()
        {
            for (int i = 0; i < lineRenderer.Length; i++)
            {
                ReflectTrigger reflectTrigger2 = lineRenderer[i].GetComponent<ReflectTrigger>();

                if (reflectTrigger2.triggerObject != null){
                    reflectTrigger2.triggerObject.SetTriggerOff();
                    reflectTrigger2.triggerObject = null;
                }

                lineRenderer[i].enabled = false;
                HitEffect[i].SetActive(false);
                //triggerObjects = null;
            }
        }

        public Vector3 ReflectionDirection(Vector3 gunDirection, Vector3 hitNormal, int lr = 0)
        {
            if (trigger.triggerName == "Lens")
                return gunDirection;
            else if (trigger.triggerName == "Reflection")
                return Vector3.Reflect(gunDirection, hitNormal);
            else
            {
                if (lr == 0)
                    return Quaternion.AngleAxis(trigger.angle, Vector3.down) * gunDirection;
                else
                    return Quaternion.AngleAxis(-trigger.angle, Vector3.down) * gunDirection;
            }
        }

        [System.Obsolete]
        public void Reflect(RaycastHit hitPoint, Vector3 gunDirection, LaserColor currentColor)
        {

            for (int i = 0; i < lineRenderer.Length; i++)
            {
                RaycastHit hit;
                lineRenderer[i].enabled = true;
                Vector3 lineStartPoint = hitPoint.point;

                Vector3 direction = ReflectionDirection(gunDirection, hitPoint.normal, i);

                ReflectTrigger reflectTrigger = lineRenderer[i].GetComponent<ReflectTrigger>();
                
                if (Physics.Raycast(lineStartPoint, direction, out hit, range))
                {
                    if (hit.collider.gameObject != transform.gameObject)
                    {
                        // draw the raycast line
                        Debug.DrawRay(lineStartPoint, direction * hit.distance, Color.black);

                        lineRenderer[i].SetPosition(0, lineStartPoint);
                        lineRenderer[i].SetPosition(1, hit.point);

                        Color myColor = ConvertColor.ConvertColorRGB(currentColor);
                        lineRenderer[i].materials[0].SetColor("_Color", myColor);

                        // set (next) hitten object triggerName (hitName)
                        // if (next) hitObject has a ReflectioManager / (next) hitObject is a trigger, 
                        // get triggerName from ScriptableObject and set triggerName as hitName
                        string hitName = null;
                        if (hit.collider.gameObject.GetComponent<ReflectionManager>())
                        {
                            hitName = hit.collider.gameObject.GetComponent<ReflectionManager>().trigger.triggerName;
                        }
                        
                        if (hitName == "Lens")
                        HitEffect[i].SetActive(false);
                        else
                        {
                            HitEffect[i].SetActive(true);
                            HitEffect[i].GetComponent<ParticleSystem>().startColor = myColor;
                            HitEffect[i].transform.position = hit.point + hit.normal * HitOffset;
                            HitEffect[i].transform.rotation = Quaternion.identity;
                        }

                        if (hitName == "Reflection" || hitName == "Refraction" || hitName == "Lens")
                        {
                            if (preTriggerObject != null)
                                preTriggerObject.SetTriggerOff();

                            reflectTrigger.triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager>();
                            reflectTrigger.triggerObject.SetTriggerOn(hit, direction, currentColor);

                            preTriggerObject = reflectTrigger.triggerObject;
                        }
                        
                        

                        if (hit.collider.gameObject.GetComponent<TriggerTest>())
                            hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();
                    }
                }
            }
        }
    }
}
