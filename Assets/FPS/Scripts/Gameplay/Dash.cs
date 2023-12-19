using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.FPS.Gameplay
{
    public class Dash : MonoBehaviour
    {
        public Vector3 MoveDirection;

        [Header("Dash")]
        public const float MaxDashTime = 1.0f;
        public float DashDistance = 5;
        public float DashStoppingSpeed = 0.1f;
        public float DashCooldown = 1.0f;

        private bool isDashing = false;
        private float currentCooldown = 0.0f;
        private float currentDashTime = MaxDashTime;
        private float dashSpeed = 6;

        CharacterController controller;



        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDashing && currentCooldown <= 0 && Input.GetButtonDown(GameConstants.k_ButtonNameDash)) // q
            {
                isDashing = true;
                currentDashTime = 0;

                // only horizontal movement
                Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
                Vector3 horizontalDirection = horizontalVelocity.normalized;

                MoveDirection = horizontalDirection * DashDistance;
            }

            if (isDashing)
            {
                if (currentDashTime < MaxDashTime)
                {
                    currentDashTime += DashStoppingSpeed;
                }

                else
                {
                    isDashing = false;
                    currentCooldown = DashCooldown;
                    MoveDirection = Vector3.zero;
                }
                controller.Move(MoveDirection * Time.deltaTime * dashSpeed);
            }

            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }
    }
}

