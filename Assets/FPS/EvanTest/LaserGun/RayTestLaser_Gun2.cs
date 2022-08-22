using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Unity.FPS.Gameplay;

namespace Unity.FPS.Gameplay
{
    public class RayTestLaser_Gun2 : MonoBehaviour
    {
        public GameObject HitEffect;
        public GameObject StartEffect;
        private ParticleSystem[] Hit;
        public LineRenderer lineRenderer;
        public float range = 1000f;
        public float HitOffset = 0;
        private ReflectionManager triggerObject;
        private PlayerInputHandler InputHandler;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            InputHandler = player.GetComponent<PlayerInputHandler>();
            HitEffect.SetActive(false);
            StartEffect.SetActive(false);
        }
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                lineRenderer.enabled = true;
                ShootRay();
                StartEffect.SetActive(true);

            }
            if (InputHandler.GetFireInputReleased())
            {
                lineRenderer.enabled = false;
                HitEffect.SetActive(false);
                StartEffect.SetActive(false);
                if (triggerObject != null)
                {
                    triggerObject.SetTriggerOff();
                }
            }
        }

        void ShootRay()
        {
            RaycastHit hit;
            Vector3 lineStartPoint = lineRenderer.transform.position;
            Vector3 direction = lineRenderer.transform.forward;

            if (Physics.Raycast(lineStartPoint, direction, out hit, range))
            {
                string tag = hit.collider.gameObject.tag;
                lineRenderer.SetPosition(0, lineStartPoint);
                lineRenderer.SetPosition(1, hit.point);
                // testing event trigger purpose
                // could be replaced by activating ScriptableObject trigger

                // Notice:
                // The following code could be replaced once the ScriptableObject is implemented:
                //
                // if ScriptableObject name == "Reflection",
                // set isTrigger = true
                // then do Reflect() in the Trigger script (or doTrigger() function)
                //

                if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                {
                    triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager>();
                    triggerObject.SetTriggerOn(hit, direction, hit.collider.gameObject.tag);
                }
                else if (triggerObject != null)
                {
                    triggerObject.SetTriggerOff();
                }


                if (hit.collider.gameObject.GetComponent<Trigger>())
                    hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();


                if (tag == "Lens")
                {
                    HitEffect.SetActive(false);
                }
                else
                {
                    HitEffect.SetActive(true);
                    HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                    HitEffect.transform.rotation = Quaternion.identity;
                }
            }
        }

    }
}
