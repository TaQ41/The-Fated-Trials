using PlayerFunctions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    /// <summary>
    /// Directly applies movement to a rigidbody on a 2d axis. This is the base WASD movement.
    /// </summary>
    public class FlatPlayerMovement : MonoBehaviour
    {
        private GameInput gameInput;

        [SerializeField]
        private Rigidbody PlayerRb;
        private Vector2 m_movement;

        
        [BoxGroup("Walk Movement"), HorizontalGroup("Walk")]
        public float MaxMoveSpeed_Walk;

        [HorizontalGroup("Walk")]
        public float WalkSpeedBuildupRate;

        [BoxGroup("Run Movement"), HorizontalGroup("Run")]
        public float MaxMoveSpeed_Run;

        [HorizontalGroup("Run")]
        public float RunSpeedBuildupRate;

        void OnEnable()
        {
            gameInput = new();
            gameInput.PlayerFunctions.Move.Enable();
            gameInput.PlayerFunctions.Move.performed += OnPlayerMove;
            gameInput.PlayerFunctions.Move.canceled += OnPlayerStopMove;
        }

        void OnDisable()
        {
            gameInput.PlayerFunctions.Move.Disable();
            gameInput.PlayerFunctions.Move.performed -= OnPlayerMove;
            gameInput.PlayerFunctions.Move.canceled -= OnPlayerStopMove;
        }

        void OnPlayerMove(InputAction.CallbackContext context)
        {
            m_movement = context.ReadValue<Vector2>();
        }

        void OnPlayerStopMove(InputAction.CallbackContext context)
        {
            m_movement = new Vector2(0, 0);
            
        }

        void FixedUpdate()
        {
            //PlayerRb.linearVelocity = moveSpeed * new Vector3(m_movement.x, 0f, m_movement.y).normalized;
        }
    }

    /*
    Let the player accelerate quickly to walking speed by applying a force. Cap off speed by clamping the velocity and follow
    orientation movement by the orientation defined in the current camera context. 


    */
}