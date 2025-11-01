using PlayerFunctions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFunctionsCore
{
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject PivotObj;

        [SerializeField]
        private GameInput gameInput;

        [SerializeField]
        private ThirdPersonCamera m_thirdPersonCamera;

        private (Quaternion rotation, Vector3 position) cameraTransformProperties;

        void OnEnable()
        {
            gameInput = new();
            gameInput.Enable();
            gameInput.PlayerFunctions.Look.performed += OnPlayerLook;
        }

        void OnDisable()
        {
            gameInput.Disable();
            gameInput.PlayerFunctions.Look.performed -= OnPlayerLook;
        }

        void OnPlayerLook(InputAction.CallbackContext context)
        {
            m_thirdPersonCamera.PivotPosition = PivotObj.transform.position;
            
            // Get deltaMouse.
            Vector2 deltaMouse = context.ReadValue<Vector2>();

            // Get computed 3rdPersonCamera properties. (Non-deviated smoothing method)
            cameraTransformProperties = m_thirdPersonCamera.CalculateRotationAndPosition(deltaMouse, smoothingMethod: vec3 => Quaternion.Euler(vec3));

            // Apply properties to actual object.
            transform.position = cameraTransformProperties.position;
            transform.localRotation = cameraTransformProperties.rotation;
        }
    }
}
