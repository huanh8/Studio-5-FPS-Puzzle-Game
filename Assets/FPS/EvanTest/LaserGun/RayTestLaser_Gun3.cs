using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class RayTestLaser_Gun3 : MonoBehaviour
    {
        /*         public GameObject HitEffect;
                public GameObject StartEffect; */
        private ParticleSystem[] Hit;
        public Transform firePoint;
        public float range = 1000f;
        public float HitOffset = 0;
        private ReflectionManager3 triggerObject;
        private ReflectionManager3 preTriggerObject;
        private PlayerInputHandler InputHandler;
        private LaserColor currentColor = LaserColor.RED;
        public GameObject lightPrefab;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            InputHandler = player.GetComponent<PlayerInputHandler>();
            //HitEffect.SetActive(false);
            // StartEffect.SetActive(false);
        }

        [System.Obsolete]
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                //lineRenderer.enabled = true;
                ShootRay();
                // StartEffect.SetActive(true);

            }
            if (InputHandler.GetFireInputReleased())
            {
                //lineRenderer.enabled = false;
                //HitEffect.SetActive(false);
                // StartEffect.SetActive(false);
                lightPrefab.SetActive(false);
                if (triggerObject != null)
                    triggerObject.SetTriggerOff();
            }
        }

        [System.Obsolete]
        void ShootRay()
        {
            RaycastHit hit;
            Vector3 lineStartPoint = firePoint.transform.position;
            Vector3 direction = firePoint.transform.forward;

            //Debug.DrawRay(lineStartPoint, direction * 1f, Color.white);

            if (Physics.Raycast(lineStartPoint, direction, out hit, range))
            {

                /*                 lineRenderer.SetPosition(0, lineStartPoint);
                                lineRenderer.SetPosition(1, hit.point); */
    
                float distance = Vector3.Distance(hit.point, lineStartPoint);
                Debug.DrawRay(lineStartPoint, direction*distance, Color.black);

                lightPrefab.SetActive(true);
                //change lightprefab's localscale.z to the distance between the hit point and the player
                lightPrefab.transform.localScale =
                    new Vector3(lightPrefab.transform.localScale.x, distance, lightPrefab.transform.localScale.z);


                string tag = hit.collider.gameObject.tag;

                if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                {
                    if (preTriggerObject != null)
                    preTriggerObject.SetTriggerOff();

                    if (tag == "Reflection")
                        currentColor = LaserColor.GREEN;
                    if (tag == "Refraction")
                        currentColor = LaserColor.YELLOW;
                    if (tag == "Lens")
                        currentColor = LaserColor.CYAN;
                    if (hit.collider.gameObject.GetComponent<ReflectionManager3>())
                    {

                        triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager3>();
                        triggerObject.SetTriggerOn(hit, direction, hit.collider.gameObject.tag, currentColor);
                        preTriggerObject = triggerObject;
                    }
                }
                else if (triggerObject != null)
                    triggerObject.SetTriggerOff();

                if (hit.collider.gameObject.GetComponent<TriggerTest>())
                    hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();

                /*                 if (tag == "Lens")
                                {
                                    //HitEffect.SetActive(false);
                                }
                                else
                                {
                                   // HitEffect.SetActive(true);
                                    //.transform.position = hit.point + hit.normal * HitOffset;
                                //HitEffect.transform.rotation = Quaternion.identity;
                                } */
            }
        }
    }
}
