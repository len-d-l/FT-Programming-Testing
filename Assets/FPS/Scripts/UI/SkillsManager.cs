using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;


namespace Unity.FPS.UI
{
    public class SkillsManager : MonoBehaviour
    {
        PlayerCharacterController characterController;

        public enum SkillType
        {
            HPUp,
            HPUp2,
            DMGUp,
            DMGUp2,
            JumpUp,
            Jetpack,
            Dash,
            Grapple,
        }

        private List<GameObject> enemies = new List<GameObject>();

        private void Start()
        {
            characterController = FindObjectOfType<PlayerCharacterController>();

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemies.Add(enemy);
            }
        }

        public void Unlock(SkillType skills)
        {
            switch (skills)
            {
                case SkillType.HPUp:
                    HealthUp(1.25f);
                    break;
                case SkillType.HPUp2:
                    HealthUp(1.4f);
                    break;
                case SkillType.DMGUp:
                    DamageUp(1.25f);
                    break;
                case SkillType.DMGUp2:
                    DamageUp(1.5f);
                    break;
                case SkillType.JumpUp:
                    JumpUp(2.5f);
                    break;
                case SkillType.Jetpack:
                    characterController.GetComponent<Jetpack>().TryUnlock();
                    break;
                case SkillType.Dash:
                    characterController.GetComponent<Dash>().enabled = true;
                    break;
                case SkillType.Grapple:
                    characterController.GetComponent<Grappling>().enabled = true;
                    break;
                default:
                    break;
            }
        }

        public void HealthUp(float hpIncreasePercent)
        {
            characterController.GetComponent<Health>().MaxHealth *= hpIncreasePercent;
            characterController.GetComponent<Health>().CurrentHealth *= hpIncreasePercent;
        }

        public void DamageUp(float dmgMultiplier)
        {
            foreach (GameObject enemy in enemies)
            {
                Damageable damageable = enemy.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.DamageMultiplier = dmgMultiplier;
                }
            }
        }

        public void JumpUp(float jumpIncrease)
        {
            characterController.GetComponent<PlayerCharacterController>().JumpForce += jumpIncrease;
        }
    }

}