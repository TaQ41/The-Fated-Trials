using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFunctionsCore
{
    public class FlatPlayerMovement1 : MonoBehaviour
    {
        private PlayerFunctions.GameInput gameInput;

        [SerializeField]
        private PlayerCamera playerCamera;

        [SerializeField]
        private Rigidbody playerRb;
        private Vector2 m_movement;
        private Vector3 GetFlatMoveVec { get { return new Vector3(m_movement.x, 0f, m_movement.y);} }

        // Controls
        [Tooltip ("")]
        public float MoveSpeed_Walk;

        [Tooltip ("")]
        public float MoveSpeedChangeRate;

        public bool toggle;

        void OnEnable()
        {
            gameInput = new();
            gameInput.PlayerFunctions.Move.Enable();
            gameInput.PlayerFunctions.Move.performed += OnPlayerMove;
            gameInput.PlayerFunctions.Move.canceled += OnPlayerNoMove;

        }

        void OnDisable()
        {
            gameInput.PlayerFunctions.Move.Disable();
            gameInput.PlayerFunctions.Move.performed -= OnPlayerMove;
            gameInput.PlayerFunctions.Move.canceled -= OnPlayerNoMove;
        }

        void OnPlayerMove(InputAction.CallbackContext context)
        {
            m_movement = context.ReadValue<Vector2>();
        }

        void OnPlayerNoMove(InputAction.CallbackContext context)
        {
            m_movement = Vector2.zero;
        }

        /// <summary>
        /// Get a normalized direction from the playerCamera's current orientation and the movement input.
        /// WASD moves the rigidbody forward, left, and right based on the camera's y angle.
        /// </summary>
        /// <returns>A normalized Vector3 direction.</returns>
        private Vector3 GetCameraRelativeDirection()
        {
            Quaternion quaternion = Quaternion.Euler(new Vector3(0f, playerCamera.Orientation, 0f));
            return quaternion * new Vector3(m_movement.x, 0f, m_movement.y);
        }

        void FixedUpdate()
        {
            if (m_movement == Vector2.zero)
                return;

            playerRb.AddForce(10f * MoveSpeedChangeRate * GetCameraRelativeDirection(), ForceMode.Force);

            // Do this, but the velocity must also be signed to allow for consistent WS and AD directions.
            playerRb.linearVelocity = new Vector3(
                Math.Min(playerRb.linearVelocity.x, MoveSpeed_Walk),
                playerRb.linearVelocity.y,
                Math.Min(playerRb.linearVelocity.z, MoveSpeed_Walk)
            );
        }
    }
}