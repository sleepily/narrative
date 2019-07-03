using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Utility
{
    public class SimpleMouseRotatorModified : MonoBehaviour
    {
        // A mouselook behaviour with constraints which operate relative to
        // this gameobject's initial rotation.
        // Only rotates around local X and Y.
        // Works in local coordinates, so if this object is parented
        // to another moving gameobject, its local constraints will
        // operate correctly
        // (Think: looking out the side window of a car, or a gun turret
        // on a moving spaceship with a limited angular range)
        // to have no constraints on an axis, set the rotationRange to 360 or greater.
        public Vector2 rotationRange = new Vector3(70, 70);
        public Vector2 rotationRangeOffset = new Vector3(20, 0);
        public float rotationSpeed = 10;
        public float dampingTime = 0.2f;
        public bool autoZeroVerticalOnMobile = true;
        public bool autoZeroHorizontalOnMobile = false;
        
        private Vector3 m_TargetAngles;
        private Vector3 m_FollowAngles;
        private Vector3 m_FollowVelocity;
        private Quaternion m_OriginalRotation;

        private void Start() => m_OriginalRotation = transform.localRotation;

        private void Update() => CheckIfUpdateAllowed();

        void CheckIfUpdateAllowed()
        {
            // Prevent any movement if it's not allowed
            if (GameManager.GLOBAL.isPaused || GameManager.GLOBAL.inventory.isOpen)
                return;

            if (GameManager.GLOBAL.player.hasLockedMovement)
                return;

            if (GameManager.GLOBAL.dialogue.dialogueInProgress)
                return;

            if (GameManager.GLOBAL.dialogue.menuInProgress)
                return;

            Calculate();
        }

        void Calculate()
        {
            // we make initial calculations from the original local rotation
            transform.localRotation = m_OriginalRotation;

            // read input from mouse or mobile controls
            float inputH = CrossPlatformInputManager.GetAxis("Mouse X");
            float inputV = CrossPlatformInputManager.GetAxis("Mouse Y");

            // wrap values to avoid springing quickly the wrong way from positive to negative
            if (m_TargetAngles.y > 180)
            {
                m_TargetAngles.y -= 360;
                m_FollowAngles.y -= 360;
            }
            if (m_TargetAngles.x > 180)
            {
                m_TargetAngles.x -= 360;
                m_FollowAngles.x -= 360;
            }
            if (m_TargetAngles.y < -180)
            {
                m_TargetAngles.y += 360;
                m_FollowAngles.y += 360;
            }
            if (m_TargetAngles.x < -180)
            {
                m_TargetAngles.x += 360;
                m_FollowAngles.x += 360;
            }

            // with mouse input, we have direct control with no springback required.
            m_TargetAngles.y += inputH * rotationSpeed;
            m_TargetAngles.x += inputV * rotationSpeed;

            // clamp values to allowed range
            m_TargetAngles.y = Mathf.Clamp(m_TargetAngles.y, (-rotationRange.y * 0.5f) - rotationRangeOffset.y, (rotationRange.y * 0.5f) - rotationRangeOffset.y);
            m_TargetAngles.x = Mathf.Clamp(m_TargetAngles.x, (-rotationRange.x * 0.5f) - rotationRangeOffset.x, (rotationRange.x * 0.5f) - rotationRangeOffset.x);
            

            // smoothly interpolate current values to target angles
            m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, dampingTime);

            // update the actual gameobject's rotation
            transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
        }
    }
}
