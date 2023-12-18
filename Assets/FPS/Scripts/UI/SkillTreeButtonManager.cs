using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Unity.FPS.UI
{
    public class SkillTreeButtonManager : MonoBehaviour
    {
        public Button[] OnePointButtons; // Array of upgrade buttons
        public Button[] TwoPointButtons;
        public Button myButton;
        private bool[] onePointButtonsPurchased;
        private bool[] twoPointButtonsAvailable;

        private LevelSystem levelSystem;
        
        public Jetpack jetpack;
        public PlayerCharacterController characterController;
        [Tooltip("To which enemies does the damage multiplier apply to")]
        public Damageable[] damageable;

        private void Start()
        {
            levelSystem = FindObjectOfType<LevelSystem>();
            onePointButtonsPurchased = new bool[OnePointButtons.Length];
            twoPointButtonsAvailable = new bool[TwoPointButtons.Length];

            for (int i = 0; i < OnePointButtons.Length; i++)
            {
                int index = i;
                OnePointButtons[i].onClick.AddListener(() => PurchaseOnePointButton(index));
            }

            for (int i = 0; i < TwoPointButtons.Length; i++)
            {
                int index = i;
                TwoPointButtons[i].interactable = false;
                TwoPointButtons[i].onClick.AddListener(() => PurchaseTwoPointButton(index));
            }

            for (int i = 0; i < OnePointButtons.Length; i++)
            {
                if (levelSystem.SkillPoints >= 1 && !OnePointButtons[i].interactable)
                {
                    OnePointButtons[i].interactable = true;
                }
            }

            for (int i = 0; i < TwoPointButtons.Length; i++)
            {
                if (levelSystem.SkillPoints >= 2 && !TwoPointButtons[i].interactable)
                {
                    TwoPointButtons[i].interactable = true;
                }
            }

            /*UpdateButtonInteractivity();*/
        }

        private void OnEnable()
        {
            for (int i = 0; i < OnePointButtons.Length; i++)
            {
                if (levelSystem.SkillPoints >= 1 && !OnePointButtons[i].interactable) 
                {
                    OnePointButtons[i].interactable = true;
                }
            }

            for (int i = 0; i < TwoPointButtons.Length; i++)
            {
                if (levelSystem.SkillPoints >= 2 && !TwoPointButtons[i].interactable)
                {
                    TwoPointButtons[i].interactable = true;
                }
            }
        }

        /*private void UpdateButtonInteractivity()
        {
            for (int i = 0; i < OnePointButtons.Length; i++)
            {
                OnePointButtons[i].interactable = levelSystem.SkillPoints >= 1 && !onePointButtonsPurchased[i];
                ChangeButtonColor(OnePointButtons[i], onePointButtonsPurchased[i] ? Color.green : Color.white);
            }

            for (int i = 0; i < TwoPointButtons.Length; i++)
            {
                TwoPointButtons[i].interactable = levelSystem.SkillPoints >= 2 && twoPointButtonsAvailable[i];
                ChangeButtonColor(TwoPointButtons[i], twoPointButtonsAvailable[i] ? Color.green : Color.white);
            }
        }*/

        private void PurchaseOnePointButton(int index)
        {
            int requiredSkillPoints = 1;
            if (levelSystem.SkillPoints >= requiredSkillPoints && !onePointButtonsPurchased[index])
            {
                levelSystem.SkillPoints -= requiredSkillPoints;
                onePointButtonsPurchased[index] = true;
                OnePointButtons[index].interactable = false;

                ChangeButtonColor(OnePointButtons[index], Color.green);

                if (levelSystem.SkillPoints < 1) 
                { 
                    for (int i = 0; i < OnePointButtons.Length; i++)
                    {
                        if (i != index)
                        {
                            OnePointButtons[i].interactable = false;
                        }
                    }
                }

                // Enable the corresponding two-point button
                if (index < TwoPointButtons.Length && levelSystem.SkillPoints >= 2)
                {
                    twoPointButtonsAvailable[index] = true;
                    /*CheckAndEnableTwoPointButtons();*/
                }
            }
        }

        private void PurchaseTwoPointButton(int index)
        {
            int requiredSkillPoints = 2;
            if (levelSystem.SkillPoints >= requiredSkillPoints && !onePointButtonsPurchased[index])
            {
                levelSystem.SkillPoints -= requiredSkillPoints;
                TwoPointButtons[index].interactable = false;

                ChangeButtonColor(TwoPointButtons[index], Color.green);

                if (levelSystem.SkillPoints < 2)
                {
                    for (int i = 0; i < TwoPointButtons.Length; i++)
                    {
                        if (i != index)
                        {
                            TwoPointButtons[i].interactable = false;
                        }
                    }
                }
            }
        }

        /*private void CheckAndEnableTwoPointButtons()
        {
            for (int i = 0; i < TwoPointButtons.Length; i++)
            {
                if (twoPointButtonsAvailable[i])
                {
                    int prerequisiteIndex = i;
                    if (twoPointButtonsAvailable[prerequisiteIndex])
                    {
                        TwoPointButtons[i].interactable = true;
                    }
                }
            }
        }*/

        private void ChangeButtonColor(Button button, Color color)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = color;
            cb.disabledColor = color;
            button.colors = cb;
        }

        public void HealthUp()
        {
            if (levelSystem.SkillPoints >= 1)
            {
                characterController.GetComponent<Health>().MaxHealth += 50;
                characterController.GetComponent<Health>().CurrentHealth += 50;

                levelSystem.SkillPoints--;

                myButton.enabled = false;
                ChangeButtonColor(myButton, Color.green);
            }
            
        }

        public void HealthUp2()
        {
            characterController.GetComponent<Health>().MaxHealth += 100;
            characterController.GetComponent<Health>().CurrentHealth += 100;
        }

        public void DMGUp()
        {
            foreach (var damageable in damageable)
            {
                if (damageable != null)
                {
                    damageable.DamageMultiplier = 1.5f;
                }
            }
        }

        public void DMGUp2()
        {
            foreach (var damageable in damageable)
            {
                if (damageable != null)
                {
                    damageable.DamageMultiplier = 2f;
                }
            }
        }

        public void JumpUp()
        {
            characterController.GetComponent<PlayerCharacterController>().JumpForce += 3;
        }

        public void JetpackUnlock()
        {
            jetpack.TryUnlock();
        }

        public void DashUnlock()
        {
            characterController.GetComponent<Dash>().enabled = true;
        }
    }
}
