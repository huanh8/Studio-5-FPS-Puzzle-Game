using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Unity.FPS.UI
{
    public class StartMenu : MonoBehaviour
    {
        public GameObject playerObject;
        public GameObject storyManager;
        void OnDisable()
        {
            if (playerObject != null)
                playerObject.SetActive(true);
            if (storyManager != null)
                storyManager.SetActive(true);
        }
        void OnEnable()
        {
            if (playerObject != null)
                playerObject.SetActive(false);
            if (storyManager != null)
                storyManager.SetActive(false);
        }
    }
}