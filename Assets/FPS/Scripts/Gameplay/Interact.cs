using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public class Interact : MonoBehaviour
    {
        public Canvas canvas;

        public GameObject interactionObject;

        public WeaponController WeaponPrefab;

        [SerializeField] UnityEvent startTriggerEvent;
        [SerializeField] UnityEvent afterTriggerEvent;
        [SerializeField] UnityEvent pickupGunTriggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            interactionObject = other.gameObject;

            if(interactionObject.tag == "Kyle")
            {
                canvas.gameObject.SetActive(true);
                interactionObject.GetComponent<KyleMove>().StopMoving();
                interactionObject.GetComponent<Animator>().SetBool("isIdle", true);
            }
            else if(interactionObject.tag == "Prop Gun")
            {
                canvas.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other) {

            if(interactionObject.tag == "Kyle")
            {
                canvas.gameObject.SetActive(false);
                interactionObject.GetComponent<KyleMove>().StartMoving();
                interactionObject.GetComponent<Animator>().SetBool("isIdle", false);
                afterTriggerEvent.Invoke();
            }
            else if(interactionObject.tag == "Prop Gun")
            {
                canvas.gameObject.SetActive(false);
            }

            interactionObject = null;
        }

        private void Update() {
            if(Input.GetKeyDown("e") && interactionObject != null)
            {
                Debug.Log("E was pressed");
                if(interactionObject.tag == "Kyle")
                {
                    Debug.Log("It's Kyle");
                    startTriggerEvent.Invoke();
                }
                else if(interactionObject.tag == "Prop Gun")
                {
                    Debug.Log("It's the gun");
                    PlayerCharacterController byPlayer = gameObject.GetComponent<PlayerCharacterController>();
                    PlayerWeaponsManager playerWeaponsManager = byPlayer.GetComponent<PlayerWeaponsManager>();
                    pickupGunTriggerEvent.Invoke();

                    if (playerWeaponsManager)
                    {
                        if (playerWeaponsManager.AddWeapon(WeaponPrefab))
                        {
                            // Handle auto-switching to weapon if no weapons currently
                            if (playerWeaponsManager.GetActiveWeapon() == null)
                            {
                                playerWeaponsManager.SwitchWeapon(true);
                            }

                            Destroy(interactionObject);
                            canvas.gameObject.SetActive(false);
                        }
                    }

                }
            }
        }
    }
}
