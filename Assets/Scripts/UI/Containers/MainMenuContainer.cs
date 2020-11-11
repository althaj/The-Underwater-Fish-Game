using System.Collections;
using System.Collections.Generic;
using TUFG.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class MainMenuContainer : MonoBehaviour
    {
        [SerializeField] private Button loadGameButton;

        /// <summary>
        /// Is the container currently displaying main menu?
        /// </summary>
        public bool IsOpen { get; private set; }

        private GameObject mainMenuPanel;
        private GameObject newGamePopup;

        #region Unity methods
        private void Start()
        {
            mainMenuPanel = transform.GetChild(0).gameObject;
            newGamePopup = transform.GetChild(1).gameObject;

            mainMenuPanel.SetActive(false);
            newGamePopup.SetActive(false);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Open the new game window.
        /// </summary>
        private void OpenNewGamePopup()
        {
            CloseMenu();
            IsOpen = true;

            newGamePopup.SetActive(true);
            EventSystem.current.SetSelectedGameObject(newGamePopup.GetComponentInChildren<Button>().gameObject);
        }

        /// <summary>
        /// Close the new game popup.
        /// </summary>
        private void CloseNewGamePopup()
        {
            newGamePopup.SetActive(false);
            IsOpen = false;
        }
        #endregion

        #region Public methods
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

                loadGameButton.interactable = GameManager.Instance.AnySaveExists();
            }
        }

        /// <summary>
        /// Close the menu container.
        /// </summary>
        public void CloseMenu()
        {
            mainMenuPanel.SetActive(false);
            IsOpen = true;
        }

        /// <summary>
        /// Open the new game window. If there is no saved game, directly start a new game.
        /// </summary>
        public void NewGameCheck()
        {
            if (GameManager.Instance.AnySaveExists())
            {
                OpenNewGamePopup();
            }
            else
            {
                StartNewGame();
            }
        }

        /// <summary>
        /// Get back to main menu after showing the new game popup.
        /// </summary>
        public void BackToMenu()
        {
            CloseNewGamePopup();
            OpenMainMenu();
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartNewGame()
        {
            GameManager.Instance.NewGame();
            CloseMenu();
            CloseNewGamePopup();
        }

        /// <summary>
        /// Open the load game window.
        /// </summary>
        public void LoadGame()
        {
            GameManager.Instance.LoadGame();
            CloseMenu();
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
        #endregion
    } 
}
