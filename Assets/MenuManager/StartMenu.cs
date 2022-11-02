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
        public GameObject mainMenu;

        public void OnStartGame()
        {
            if (playerObject != null)
                playerObject.SetActive(true);
            if (storyManager != null)
                storyManager.SetActive(true);
            if (mainMenu != null)
                mainMenu.SetActive(false);
        }
        public void reLoadTheSceen()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        public void QuitGame()
        {
            Application.Quit();
        }  
        //press ESC to Display the main menu
        void Update()
        {   

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                reLoadTheSceen();
                // release the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            // press key TAB
        }
    }
}