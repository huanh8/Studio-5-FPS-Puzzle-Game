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
        private ReflectionManager[] triggerObject;
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
            triggerObject = new ReflectionManager[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<LineRenderer>())
                {
                    lineRenderer[i] = transform.GetChild(i).GetComponent<LineRenderer>();
                    HitEffect[i] = transform.GetChild(i).GetChild(0).gameObject;
                }
            }
        }
        public void SetTriggerOn(RaycastHit hitPoint, Vector3 gunDirection, string type, LaserColor currentColor)
        {
            Reflect(hitPoint, gunDirection, type, currentColor);
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

        public void Reflect(RaycastHit hitPoint, Vector3 gunDirection, string type, LaserColor currentColor)
        {

            for (int i = 0; i < lineRenderer.Length; i++)
            {
                /*                 if (triggerObject[i] != null)
                                    triggerObject[i].SetTriggerOff(); */

                RaycastHit hit;
                lineRenderer[i].enabled = true;
                Vector3 lineStartPoint = hitPoint.point;

                Vector3 direction = ReflectionDirection(gunDirection, hitPoint.normal, type, i);

                if (Physics.Raycast(lineStartPoint, direction, out hit, range))
                {
                    if (hit.collider.gameObject != transform.gameObject)
                    {
                        lineRenderer[i].SetPosition(0, lineStartPoint);
                        lineRenderer[i].SetPosition(1, hit.point);

                        //change the color of the laser
                        Color customColor = new Color(0.4f, 0.9f, 0.7f, 1.0f);
                        lineRenderer[i].materials[0].SetColor("_Color", customColor);


                        string tag = hit.collider.gameObject.tag;
                        if (tag == "Lens")
                        {
                            HitEffect[i].SetActive(false);
                        }
                        else
                        {
                            HitEffect[i].SetActive(true);
                            HitEffect[i].transform.position = hit.point + hit.normal * HitOffset;
                            HitEffect[i].transform.rotation = Quaternion.identity;
                        }

                        if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                        {
                            triggerObject[i] = hit.collider.gameObject.GetComponent<ReflectionManager>();
                            triggerObject[i].SetTriggerOn(hit, direction, hit.collider.gameObject.tag, currentColor);
                        }
                        else
                        {
                            if (triggerObject[i] != null)
                                triggerObject[i].SetTriggerOff();
                        }

                        if (hit.collider.gameObject.GetComponent<Trigger>())
                            hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();

                    }
                }
            }
        }
    }
}
