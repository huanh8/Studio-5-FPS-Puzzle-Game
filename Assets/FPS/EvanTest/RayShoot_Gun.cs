using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class RayShoot_Gun : MonoBehaviour
    {
        public Transform fpsCam;
        public LineRenderer lineRenderer;
        public float range = 100f;

        // Update is called once per frame
        private PlayerInputHandler InputHandler;

        void Start()
        {
            GameObject player = GameObject.Find("Player");
            InputHandler = player.GetComponent<PlayerInputHandler>();
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
            }
        }

        void ShootRay()
        {
            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                lineRenderer.SetPosition(0, fpsCam.transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(0, fpsCam.transform.position);
                lineRenderer.SetPosition(1, fpsCam.transform.position + fpsCam.transform.forward * 1000);
            }
        }

    }
}
