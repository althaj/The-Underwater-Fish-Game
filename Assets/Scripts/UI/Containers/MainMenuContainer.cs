using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class MainMenuContainer : MonoBehaviour
    {
        /// <summary>
        /// Is the container currently displaying main menu?
        /// </summary>
        public bool IsOpen { get; private set; }

        private GameObject mainMenuPanel;

        private void Start()
        {
            mainMenuPanel = transform.GetChild(0).gameObject;

            mainMenuPanel.SetActive(false);
        }

        /// <summary>
        /// Open the main menu container.
        /// </summary>
        public void OpenMainMenu()
        {
            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                mainMenuPanel.SetActive(true);
                IsOpen = true;

                EventSystem.current.SetSelectedGameObject(mainMenuPanel.GetComponentInChildren<Button>().gameObject);
            }
        }

        /// <summary>
        /// Open the new game window.
        /// </summary>
        public void NewGame()
        {

        }

        /// <summary>
        /// Open the load game window.
        /// </summary>
        public void LoadGame()
        {

        }

        /// <summary>
        /// Open the options window.
        /// </summary>
        public void Options()
        {

        }

        /// <summary>
        /// Exit the game.
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }
    } 
}
