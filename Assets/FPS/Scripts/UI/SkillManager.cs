using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.FPS.UI.SkillTreeManager;
using Unity.FPS.Gameplay;
using Unity.FPS.Game;
using System;

namespace Unity.FPS.UI
{
    public class SkillManager : MonoBehaviour
    {
        public int SkillId;

        public TMP_Text TitleText;
        public TMP_Text DescriptionText;

        public int[] ConnectedSkills;

        public LevelSystem LevelSystem;

        PlayerCharacterController characterController;
        List<GameObject> enemies = new List<GameObject>();

        private void Start()
        {
            characterController = FindObjectOfType<PlayerCharacterController>();

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemies.Add(enemy);
            }
        }

        private void Update()
        {
            GetComponent<Image>().color = SkillTree.SkillLevels[SkillId] >= SkillTree.SkillCaps[SkillId] ? Color.green
                : LevelSystem.SkillPoints >= 1 ? Color.yellow : Color.white;
        }

        public void UpdateUI()
        {
            TitleText.text = $"{SkillTree.SkillLevels[SkillId]}/{SkillTree.SkillCaps[SkillId]}\n{SkillTree.SkillNames[SkillId]}";
            DescriptionText.text = $"{SkillTree.SkillDescription[SkillId]}\nCost: {LevelSystem.SkillPoints}/1SP";
            //Debug.Log(LevelSystem.SkillPoints);

            if (ConnectedSkills != null && ConnectedSkills.Length > 0)
            {
                foreach (var connectedSkill in ConnectedSkills)
                {
                    Debug.Log(connectedSkill);

                    SkillTree.SkillList[connectedSkill].gameObject.SetActive(SkillTree.SkillLevels[SkillId] > 0);
                    
                    foreach (var connector in SkillTree.ConnectorList)
                    {
                        connector.SetActive(SkillTree.SkillLevels[SkillId] > 0);
                    }
                }
            }
        }

        public void Buy()
        {
            if (LevelSystem.SkillPoints < 1 || SkillTree.SkillLevels[SkillId] >= SkillTree.SkillCaps[SkillId])
            {
                Debug.Log("Not enough skill points");
                return;
            }

            LevelSystem.SkillPoints -= 1;
            SkillTree.SkillLevels[SkillId]++;
            
            ResourceType resourceType = (ResourceType)SkillId;
            UnlockSkill(resourceType);

            SkillTree.UpdateAllSkillUI();
        }

        public enum ResourceType
        {
            HPUp,
            DMGUp,
            Dash,
            Grapple,
            JumpUp,
            Jetpack
        }

        private void UnlockSkill(ResourceType skills)
        {
            switch (skills)
            {
                case ResourceType.HPUp:
                    HealthUp();
                    break;
                case ResourceType.DMGUp:
                    DamageUp();
                    break;
                case ResourceType.Dash:
                    characterController.GetComponent<Dash>().enabled = true;
                    break;
                case ResourceType.Grapple:
                    characterController.GetComponent<Grappling>().enabled = true;
                    break;
                case ResourceType.JumpUp:
                    JumpUp(2.5f);
                    break;
                case ResourceType.Jetpack:
                    characterController.GetComponent<Jetpack>().TryUnlock();
                    break;
                default:
                    break;
            }
        }

        public void HealthUp()
        {
            if (SkillTree.SkillLevels[SkillId] < 2)
            {
                characterController.GetComponent<Health>().MaxHealth *= 1.25f;
                characterController.GetComponent<Health>().CurrentHealth *= 1.25f;
            }

            else
            {
                characterController.GetComponent<Health>().MaxHealth *= 1.4f;
                characterController.GetComponent<Health>().CurrentHealth *= 1.4f;
            }

        }

        public void DamageUp()
        {
            if (SkillTree.SkillLevels[SkillId] < 2)
            {
                IncreaseDamage(1.25f);
            }

            else
            {
                IncreaseDamage(1.5f);
            }
        }

        private void IncreaseDamage(float dmgMultiplier)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    Damageable damageable = enemy.GetComponentInChildren<Damageable>();
                    if (damageable != null)
                    {
                        damageable.DamageMultiplier = dmgMultiplier;
                    }
                }
            }
        }

        public void JumpUp(float jumpIncrease)
        {
            characterController.GetComponent<PlayerCharacterController>().JumpForce += jumpIncrease;
        }
    }
}