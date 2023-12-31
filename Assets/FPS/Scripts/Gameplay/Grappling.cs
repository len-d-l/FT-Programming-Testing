using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;


namespace Unity.FPS.Gameplay
{
    public class Grappling : MonoBehaviour
    {
        private PlayerCharacterController characterController;
        [Header("References")]
        public Transform Camera;
        public Transform GunTip;
        public LayerMask Grappleable;
        public LineRenderer LineRenderer;

        [Header("Grappling")]
        public float MaxGrappleDistance;
        public float GrappleDelayTime;
        public float OvershootYAxis;

        private Vector3 grapplePoint;

        [Header("Cooldown")]
        public float GrappleCd;
        private float grappleCdTimer;

        [Header("Input")]
        private bool grappling;

        private void Start()
        {
            characterController = GetComponent<PlayerCharacterController>();
        }

        private void Update()
        {
            if (Input.GetButtonDown(GameConstants.k_ButtonNameGrapple))
            {
                StartGrapple();
            }

            if (grappleCdTimer > 0)
            {
                grappleCdTimer -= Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
            if (grappling)
            {
                LineRenderer.SetPosition(0, GunTip.position);
            }
        }

        private void StartGrapple()
        {
            if (grappleCdTimer > 0) return;

            grappling = true;

            characterController.Freeze = true;

            RaycastHit raycastHit;
            if (Physics.Raycast(Camera.position, Camera.forward, out raycastHit, MaxGrappleDistance, Grappleable))
            {
                grapplePoint = raycastHit.point;

                Invoke(nameof(ExecuteGrapple), GrappleDelayTime);
            }

            else
            {
                grapplePoint = Camera.position + Camera.forward * MaxGrappleDistance;

                Invoke(nameof(StopGrapple), GrappleDelayTime);
            }

            LineRenderer.enabled = true;
            LineRenderer.SetPosition(1, grapplePoint);
        }

        private void ExecuteGrapple()
        {
            characterController.Freeze = false;

            Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

            float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
            float highestPointOnArc = grapplePointRelativeYPos + OvershootYAxis;

            if (grapplePointRelativeYPos < 0)
            {
                highestPointOnArc = OvershootYAxis;
            }

            characterController.JumpToPosition(grapplePoint, highestPointOnArc);

            Invoke(nameof(StopGrapple), 1f);
        }

        private void StopGrapple()
        {
            characterController.Freeze = false;

            grappling = false;

            grappleCdTimer = GrappleCd;

            LineRenderer.enabled = false;
        }
    }
}