using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Gameplay;

namespace Unity.FPS.Gameplay
{
    public class ReflectionManager : MonoBehaviour
    {
        // Notice:
        //
        // The following code could be replaced, inherited or even deleted 
        // once the ScriptableObject branch is implemented / merged into this branch.
        //
        // Although this is a temporary manager, the following functions inside it
        // could still be used for ScriptableObject trigger to reflect the lights.
        //private List<ReflectionManager> triggerObjects;
        private ReflectionManager triggerObject;
        private ReflectionManager preTriggerObject;
        private LineRenderer[] lineRenderer;
        private GameObject[] HitEffect;
        private float range = 1000f;
        private float HitOffset = 0.01f;
        public int ANGLE = 60;

        private
        void Awake()
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

        [System.Obsolete]
        public void SetTriggerOn(RaycastHit _hitPoint, Vector3 _gunDirection, string _type, LaserColor _currentColor)
        {
            Reflect(_hitPoint, _gunDirection, _type, _currentColor);
        }
        public void SetTriggerOff()
        {
            for (int i = 0; i < lineRenderer.Length; i++)
            {
                ReflectTrigger reflectTrigger2 = lineRenderer[i].GetComponent<ReflectTrigger>();

                if (reflectTrigger2.triggerObject != null)
                {
                    reflectTrigger2.triggerObject.SetTriggerOff();
                    reflectTrigger2.triggerObject = null;
                }

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

        [System.Obsolete]
        public void Reflect(RaycastHit hitPoint, Vector3 gunDirection, string type, LaserColor currentColor)
        {

            for (int i = 0; i < lineRenderer.Length; i++)
            {
                RaycastHit[] hits;
                Ray ray;
                lineRenderer[i].enabled = true;

                Vector3 lineStartPoint = hitPoint.point;
                Vector3 direction = ReflectionDirection(gunDirection, hitPoint.normal, type, i);

                ReflectTrigger reflectTrigger = lineRenderer[i].GetComponent<ReflectTrigger>();

                ray = new Ray(lineStartPoint, direction);
                hits = Physics.RaycastAll(ray, range);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject != transform.gameObject)
                    {
                        // Debug.Log("Yes I am the object: " + hit.collider.gameObject.name);
                        // draw the raycast line
                        // Debug.DrawRay(lineStartPoint, direction * hit.distance, Color.white);

                        lineRenderer[i].SetPosition(0, lineStartPoint);
                        lineRenderer[i].SetPosition(1, hit.point);

                        Color myColor = ConvertColor.ConvertColorRGB(currentColor);
                        lineRenderer[i].materials[0].SetColor("_Color", myColor);

                        string tag = hit.collider.gameObject.tag;

                        if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                        {
                            if (preTriggerObject != null)
                            {
                                preTriggerObject.SetTriggerOff();
                                preTriggerObject = null;
                            }

                            reflectTrigger.triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager>();
                            reflectTrigger.triggerObject.SetTriggerOn(hit, direction, hit.collider.gameObject.tag, currentColor);

                            preTriggerObject = reflectTrigger.triggerObject;
                        }

                        if (hit.collider.gameObject.GetComponent<TriggerTest>())
                        {
                            Debug.Log("I am the trigger object: " + hit.collider.gameObject.name);
                            hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();
                        }


                        if (tag == "Lens")
                            HitEffect[i].SetActive(false);
                        else
                        {
                            HitEffect[i].SetActive(true);
                            HitEffect[i].GetComponent<ParticleSystem>().startColor = myColor;
                            HitEffect[i].transform.position = hit.point + hit.normal * HitOffset;
                            HitEffect[i].transform.rotation = Quaternion.identity;
                        }
                        break;
                    }
                }
            }
        }
    }
}
