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
        [SerializeField]private int bounceSize;

        // Update is called once per frame
        private PlayerInputHandler InputHandler;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            fpsCam = player.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
            InputHandler = player.GetComponent<PlayerInputHandler>();
            HitEffect.SetActive(false);
            StartEffect.SetActive(false);
            bounceSize = lineRenderer.positionCount; 
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
            Vector3 lineStartPoint = lineRenderer.transform.position;
            Vector3 startPoint = fpsCam.transform.position;
            Vector3 direction = fpsCam.transform.forward;

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

                    CheckTrigger(hit);
                    HitEffect.SetActive(true);
                    HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                    HitEffect.transform.rotation = Quaternion.identity;

                    //hit.collider.gameObject.GetComponent<Refraction>().Refract();
                    if (hit.collider.gameObject.tag != "Reflection")
                    {
                        for (int j = i + 1; j < bounceSize; j++)
                        {
                            lineRenderer.SetPosition(j, hit.point);
                        }
                        break;
                    }

                    if (hit.collider.gameObject.tag == "Refraction")
                    {
                        hit.collider.gameObject.GetComponent<RefractionTestManager>().ShootRefractionRay();
                    }
                }
                else
                { //End laser position if doesn't collide with object
                    var EndPos = lineRenderer.transform.position + fpsCam.transform.forward * 10000;
                    lineRenderer.SetPosition(1, EndPos);
                    HitEffect.transform.position = EndPos;
                    HitEffect.SetActive(false);
                }
            }

        }
        void CheckTrigger(RaycastHit hit)
        {
            if (hit.collider.gameObject.GetComponent<Trigger>())
                hit.collider.gameObject.GetComponent<Trigger>().FireTrigger();
        }

    }
}
