using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class RayShooting : MonoBehaviour
    {
        public Transform fpsCam;
        public LineRenderer lineRenderer;
        public float range = 100f;
        public int damage = 10;
        // Update is called once per frame
        public PlayerInputHandler InputHandler;
        void start()
        {
        }
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                StartCoroutine(ShootRay());    // Shoot the raycast    
            }
            if (InputHandler.GetFireInputReleased())
            {
                StopCoroutine(ShootRay());    // Shoot the raycast    
            }
        }

        IEnumerator ShootRay()
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
            lineRenderer.enabled = true;

            yield return new WaitForSeconds(0.2f);

            lineRenderer.enabled = false;
        }

    }
}
