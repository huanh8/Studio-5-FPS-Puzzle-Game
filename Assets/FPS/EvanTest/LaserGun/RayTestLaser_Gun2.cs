using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public enum LaserColor
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
        CYAN,
        MAGENTA,
        WHITE,
    }
    public class RayTestLaser_Gun2 : MonoBehaviour
    {
        public GameObject HitEffect;
        public GameObject StartEffect;
        private ParticleSystem[] Hit;
        public LineRenderer lineRenderer;
        public float range = 1000f;
        public float HitOffset = 0;
        private ReflectionManager triggerObject;
        private ReflectionManager preTriggerObject;
        private PlayerInputHandler InputHandler;
        private LaserColor currentColor = LaserColor.RED;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            InputHandler = player.GetComponent<PlayerInputHandler>();
            HitEffect.SetActive(false);
            StartEffect.SetActive(false);
        }

        [System.Obsolete]
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
                    triggerObject.SetTriggerOff();
            }
        }

        [System.Obsolete]
        void ShootRay()
        {
            RaycastHit hit;
            Vector3 lineStartPoint = lineRenderer.transform.position;
            Vector3 direction = lineRenderer.transform.forward;

            if (Physics.Raycast(lineStartPoint, direction, out hit, range))
            {

                lineRenderer.SetPosition(0, lineStartPoint);
                lineRenderer.SetPosition(1, hit.point);
                
                // set hitten object triggerName
                // if this hitObject has a ReflectioManager / this hitObject is a trigger,
                // get triggerName from ScriptableObject and set triggerName = ScriptableObject.triggerName
                string triggerName = null;
                if (hit.collider.gameObject.GetComponent<ReflectionManager>())
                {
                    triggerName = hit.collider.gameObject.GetComponent<ReflectionManager>().trigger.triggerName;
                }

                if (triggerName == "Reflection" || triggerName == "Refraction" || triggerName == "Lens")
                {
                    if (preTriggerObject != null)
                        preTriggerObject.SetTriggerOff();

                    if (triggerName == "Reflection")
                        currentColor = LaserColor.GREEN;
                    if (triggerName == "Refraction")
                        currentColor = LaserColor.YELLOW;
                    if (triggerName == "Lens")
                        currentColor = LaserColor.CYAN;
                    triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager>();
                    triggerObject.SetTriggerOn(hit, direction, currentColor);

                    preTriggerObject = triggerObject;
                }
                else if (triggerObject != null)
                    triggerObject.SetTriggerOff();

                if (hit.collider.gameObject.GetComponent<TriggerTest>())
                    hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();

                if (triggerName == "Lens")
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
