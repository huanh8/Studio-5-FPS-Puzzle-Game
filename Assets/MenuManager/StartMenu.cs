using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Unity.FPS.UI
{
    public class StartMenu : MonoBehaviour
    {
        public GameObject playerObject;
        void OnDisable()
        {
            if(playerObject != null)
                playerObject.SetActive(true);
        }
        void OnEnable()
        {
            if(playerObject != null)
                playerObject.SetActive(false);
        }
    }
}