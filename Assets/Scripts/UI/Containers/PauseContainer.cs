using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// Container displaying pause menu.
    /// </summary>
    public class PauseContainer : ContainerBehaviour
    {
        private GameObject pauseMenuPanel;

        #region Unity methods
        void Start()
        {
            pauseMenuPanel = transform.GetChild(0).gameObject;

            pauseMenuPanel.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Open the pause menu.
        /// </summary>
        public override void Open()
        {
            FindObjectOfType<PlayerMovement>().DisableInput();

            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                pauseMenuPanel.SetActive(true);
                IsOpen = true;

                EventSystem.current.SetSelectedGameObject(pauseMenuPanel.GetComponentInChildren<Button>().gameObject);
            }
        }

        /// <summary>
        /// Close the pause menu, resuming the game.
        /// </summary>
        public override void Close()
        {
            EventSystem.current.SetSelectedGameObject(null);

            FindObjectOfType<PlayerMovement>().EnableInput();
            pauseMenuPanel.SetActive(false);
            IsOpen = false;
        }

        /// <summary>
        /// Continue the game, close the pause menu.
        /// </summary>
        public void Continue()
        {
            UIManager.Instance.ClosePauseMenu();
        }

        /// <summary>
        /// Show inventory manager.
        /// </summary>
        public void ShowInventory()
        {
            UIManager.Instance.HidePauseMenu();
            UIManager.Instance.OpenInventory();
        }

        /// <summary>
        /// Show party manager.
        /// </summary>
        public void ShowParty()
        {
            UIManager.Instance.HidePauseMenu();
            UIManager.Instance.OpenPartyWindow();
        }

        /// <summary>
        /// Exit to the main menu.
        /// </summary>
        public void Exit()
        {
            UIManager.Instance.ClosePauseMenu();
            UIManager.Instance.OpenMainMenu();
        }

    } 
}
