using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    public class SkillTreeMenuManager : MonoBehaviour
    {
        public PlayerCharacterController characterController;

        public GameObject SkillTreeMenuRoot;

        public GameObject PauseMenuRoot;

        public TextMeshProUGUI SkillPointText;

        void Start()
        {
            SkillTreeMenuRoot.SetActive(false);
        }

        void Update()
        {
            // Lock cursor when clicking outside of menu
            if (!SkillTreeMenuRoot.activeSelf && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetButtonDown(GameConstants.k_ButtonNameSkillTree))
            {
                if (SkillTreeMenuRoot.activeSelf)
                {
                    SetSkillTreeMenuActivation(false);
                }
                else
                {
                    SetSkillTreeMenuActivation(true);
                    PauseMenuRoot.SetActive(false); // Close the Pause menu if open
                }
            }

            SkillPointText.text = "Skill Points: " + characterController.GetComponent<LevelSystem>().SkillPoints;

            bool anyMenuActive = SkillTreeMenuRoot.activeSelf || PauseMenuRoot.activeSelf;
            Cursor.lockState = anyMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = anyMenuActive;

            Time.timeScale = anyMenuActive ? 0f: 1f;
        }

        public void CloseSkillTreeMenu()
        {
            SetSkillTreeMenuActivation(false);
        }

        void SetSkillTreeMenuActivation(bool active)
        {
            SkillTreeMenuRoot.SetActive(active);

            if (SkillTreeMenuRoot.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
    }
}