using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class RayShoot_Gun : MonoBehaviour
    {
        public GameObject HitEffect;
        private ParticleSystem[] Hit;
        public Camera fpsCam;
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
            Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
        }
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                lineRenderer.enabled = true;
                ShootRay();
            }
            if (InputHandler.GetFireInputReleased())
            {
                lineRenderer.enabled = false;
                HitEffect.active = false;
            }
        }

        [System.Obsolete]
        void ShootRay()
        {
            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                HitEffect.active = true;
                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                HitEffect.transform.rotation = Quaternion.identity;
                //get the ditance between the hit point and the player
                float distance = Vector3.Distance(hit.point, transform.position);
                Debug.Log(distance);
                //HitEffect.transform.position = lineRenderer.GetPosition(1) + hit.normal * 0.1f;
            }
            else
            { //End laser position if doesn't collide with object
                var EndPos = fpsCam.transform.position + fpsCam.transform.forward * 10000;

                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                HitEffect.active = false;
            }
        }

    }
}
