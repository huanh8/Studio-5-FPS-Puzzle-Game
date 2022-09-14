using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;

namespace Unity.FPS.Gameplay
{
    public class ReflectionManager3 : MonoBehaviour
    {

        public GameObject lightPrefab;
        private ReflectionManager3 preTriggerObject;
        public Transform[] firePoint;
        private GameObject[] HitEffect;
        private float range = 1000f;
        public float HitOffset = 0;
        public int ANGLE = 60;
        void Start()
        {
            // SetLineRenderer();
            SetTriggerOff();
        }
        void SetLineRenderer()
        {
            firePoint = new Transform[transform.childCount];
            // HitEffect = new GameObject[transform.childCount];

            //triggerObjects = new List<ReflectionManager>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<LineRenderer>())
                {
                    //firePoint[i] = transform.GetChild(i).GetComponent<LineRenderer>();
                    // HitEffect[i] = transform.GetChild(i).GetChild(0).gameObject;
                }
            }
        }

        [System.Obsolete]
        public void SetTriggerOn(RaycastHit hitPoint, Vector3 gunDirection, string type, LaserColor currentColor)
        {

            Reflect(hitPoint, gunDirection, type, currentColor);
        }

        public void SetTriggerOff()
        {
            for (int i = 0; i < firePoint.Length; i++)
            {
                ReflectTrigger3 reflectTrigger2 = firePoint[i].GetComponent<ReflectTrigger3>();

                if (reflectTrigger2.triggerObject != null)
                {
                    reflectTrigger2.triggerObject.SetTriggerOff();
                    reflectTrigger2.triggerObject = null;
                }
                lightPrefab.SetActive(false);
                //firePoint[i].enable = false;
                //HitEffect[i].SetActive(false);
                //triggerObjects = null;
            }
        }

        public Vector3 ReflectionDirection(Vector3 gunDirection, Vector3 hitNormal, string type, int lr=0)
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

        [System.Obsolete]
        public void Reflect(RaycastHit hitPoint, Vector3 gunDirection, string type, LaserColor currentColor)
        {

            for (int i = 0; i < firePoint.Length; i++)
            {
                RaycastHit hit;
                //lineRenderer[i].enabled = true;
                Vector3 lineStartPoint = hitPoint.point;

                Vector3 direction = ReflectionDirection(gunDirection, hitPoint.normal, type, i);

                ReflectTrigger3 reflectTrigger = firePoint[i].GetComponent<ReflectTrigger3>();

                Debug.DrawRay(lineStartPoint, direction, Color.blue);

                if (Physics.Raycast(lineStartPoint, direction, out hit, range))
                {

                    if (hit.collider.gameObject != transform.gameObject && hit.collider.gameObject != hitPoint.collider.gameObject)
                    {

                        float distance = Vector3.Distance(hit.point, lineStartPoint);
                        // draw the raycast line
                        Debug.DrawRay(lineStartPoint, direction * distance, Color.green);

                        lightPrefab.SetActive(true);

                        lightPrefab.transform.localScale =
                            new Vector3(lightPrefab.transform.localScale.x, distance, lightPrefab.transform.localScale.z);
                        lightPrefab.transform.position = lineStartPoint;

                        lightPrefab.transform.LookAt(hit.point);            

                        string tag = hit.collider.gameObject.tag;

                        /* 
                                                if (tag == "Lens")
                                                    HitEffect[i].SetActive(false);
                                                else
                                                {
                                                    HitEffect[i].SetActive(true);
                                                   HitEffect[i].GetComponent<ParticleSystem>().startColor = myColor;
                                                    HitEffect[i].transform.position = hit.point + hit.normal * HitOffset;
                                                    HitEffect[i].transform.rotation = Quaternion.identity;
                                                } */

                        if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                        {
                            if (preTriggerObject != null)
                                preTriggerObject.SetTriggerOff();

                            reflectTrigger.triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager3>();
                            reflectTrigger.triggerObject.SetTriggerOn(hit, direction, hit.collider.gameObject.tag, currentColor);

                            preTriggerObject = reflectTrigger.triggerObject;
                        }
                        /*                         else if (reflectTrigger.triggerObject != null)
                                                    reflectTrigger.triggerObject.SetTriggerOff(); */

                        if (hit.collider.gameObject.GetComponent<TriggerTest>())
                            hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();
                    }
                }
            }
        }
    }
}
