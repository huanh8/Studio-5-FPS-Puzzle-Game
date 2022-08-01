using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Gameplay
{
    public class LightShoot_Gun : MonoBehaviour
    {
        public GameObject lightPrefab;
        public GameObject EffectPrefab;
        public Camera fpsCam;

        public float delayTime = 0.05f;

        public float range = 1000f;
        public float HitOffset = 0;

        // Update is called once per frame
        private PlayerInputHandler InputHandler;

        void Start()
        {
            lightPrefab.SetActive(false);
            GameObject player = GameObject.Find("Player");
            fpsCam = player.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
            InputHandler = player.GetComponent<PlayerInputHandler>();
        }
        void Update()
        {
            if (InputHandler.GetFireInputHeld())
            {
                ShootRay();
            }
            if (InputHandler.GetFireInputReleased())
            {
                lightPrefab.SetActive(false);
            }
        }

        void ShootRay()
        {
            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                //get the ditance between the hit point and the player
                float distance = Vector3.Distance(hit.point, transform.position);
                Debug.Log(distance);


                lightPrefab.SetActive(true);
                //change lightprefab's localscale.z to the distance between the hit point and the player
                lightPrefab.transform.localScale =
                    new Vector3(lightPrefab.transform.localScale.x, lightPrefab.transform.localScale.y, distance * 4.8f);

                //create a EffectPrefab at the hit point
                GameObject effect = Instantiate(EffectPrefab, hit.point + hit.normal * HitOffset, Quaternion.identity);
                Destroy(effect, delayTime);

            }
            else
            { //End laser position if doesn't collide with object

            }
        }

    }
}
