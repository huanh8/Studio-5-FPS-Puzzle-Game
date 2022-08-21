using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class RayTestLaser_Gun2 : MonoBehaviour
    {
        public GameObject HitEffect;
        public GameObject StartEffect;
        private ParticleSystem[] Hit;
        private Camera fpsCam;
        public LineRenderer lineRenderer;
        public float range = 1000f;
        public float HitOffset = 0;

        // Update is called once per frame
        private PlayerInputHandler InputHandler;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            fpsCam = player.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
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
            }
        }

        void ShootRay()
        {
            RaycastHit hit;

            Vector3 startPoint = fpsCam.transform.position;
            Vector3 direction = fpsCam.transform.forward;

            if (Physics.Raycast(startPoint, direction, out hit, range))
            {
                if (hit.collider.gameObject.GetComponent<Trigger>())
                    hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();

                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                HitEffect.SetActive(true);
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                HitEffect.transform.rotation = Quaternion.identity;

                // Notice:
                // The following code could be replaced once the ScriptableObject is implemented:
                //
                // if ScriptableObject name == "Reflection",
                // set isTrigger = true
                // then do Reflect() in the Trigger script (or doTrigger() function)
                //
                if (hit.collider.gameObject.tag == "Reflection")
                    hit.collider.gameObject.GetComponent<ReflectionManager>().Reflect(hit.point);

            }
            else
            { //End laser position if doesn't collide with object
                var EndPos = fpsCam.transform.position + fpsCam.transform.forward * 10000;

                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                HitEffect.SetActive(false);
            }
        }

    }
}
