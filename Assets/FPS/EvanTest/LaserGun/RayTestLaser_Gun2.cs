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
        public GameObject proceduralAudioObject;

        private ProceduralAudioController proceduralAudioController;
        private AudioSource proceduralAudioSource;
        float timer = 2.0f;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            InputHandler = player.GetComponent<PlayerInputHandler>();
            HitEffect.SetActive(false);
            StartEffect.SetActive(false);
            // Audio
            proceduralAudioController = proceduralAudioObject.GetComponent<ProceduralAudioController>();
            proceduralAudioSource = proceduralAudioObject.GetComponent<AudioSource>();
            proceduralAudioSource.mute = true;
            proceduralAudioController.autoPlay = false;
        }

        [System.Obsolete]
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                lineRenderer.enabled = true;
                ShootRay();
                StartEffect.SetActive(true);
                proceduralAudioSource.mute = false;
                proceduralAudioController.autoPlay = true;

                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    proceduralAudioController.autoPlay = false;
                }
            }
            if (InputHandler.GetFireInputReleased())
            {
                lineRenderer.enabled = false;
                HitEffect.SetActive(false);
                StartEffect.SetActive(false);
                proceduralAudioSource.mute = true;
                proceduralAudioController.autoPlay = false;
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
                //Debug.DrawRay(lineStartPoint, direction * hit.distance, Color.white);
                string tag = hit.collider.gameObject.tag;

                if (tag == "Reflection" || tag == "Refraction" || tag == "Lens")
                {
                    if (preTriggerObject != null)
                    {
                        preTriggerObject.SetTriggerOff();
                        preTriggerObject = null;
                    }

                    if (tag == "Reflection")
                        currentColor = LaserColor.GREEN;
                    if (tag == "Refraction")
                        currentColor = LaserColor.BLUE;
                    if (tag == "Lens")
                        currentColor = LaserColor.MAGENTA;

                    triggerObject = hit.collider.gameObject.GetComponent<ReflectionManager>();
                    triggerObject.SetTriggerOn(hit, direction, hit.collider.gameObject.tag, currentColor);
                    preTriggerObject = triggerObject;
                }
                else if (triggerObject != null)
                    triggerObject.SetTriggerOff();

                if (hit.collider.gameObject.GetComponent<TriggerTest>())
                    hit.collider.gameObject.GetComponent<TriggerTest>().FireTrigger();

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
