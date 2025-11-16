using PlayerFunctions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFunctionsCore
{
    /// <summary>
    /// Component that follows a specified "player" while updating this object's position the calculated results of the ThirdPersonCamera.cs
    /// Should be attached to the gameObject that contains the camera or is a parent of the camera.
    /// </summary>
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject PlayerPivot;
        public Vector3 RelativePivot;

        [SerializeField]
        private GameInput gameInput;

        [SerializeField]
        private ThirdPersonCamera m_thirdPersonCamera;

        private (Quaternion rotation, Vector3 position) cameraTransformProperties;
        private Vector2 deltaMouse;

        public float Orientation { get { return cameraTransformProperties.rotation.eulerAngles.y;} }

        void OnEnable()
        {
            gameInput = new();
            gameInput.Enable();
            gameInput.PlayerFunctions.Look.performed += OnPlayerLook;
            gameInput.PlayerFunctions.Look.canceled += OnPlayerLookCancel;
        }

        void OnDisable()
        {
            gameInput.Disable();
            gameInput.PlayerFunctions.Look.performed -= OnPlayerLook;
            gameInput.PlayerFunctions.Look.canceled -= OnPlayerLookCancel;
        }

        void OnPlayerLook(InputAction.CallbackContext context)
        {   
            deltaMouse = context.ReadValue<Vector2>();
        }

        void OnPlayerLookCancel(InputAction.CallbackContext context)
        {
            deltaMouse = new Vector2(0, 0);
        }

        void Update()
        {
            m_thirdPersonCamera.PivotPosition = PlayerPivot.transform.position + RelativePivot;

            // Get computed 3rdPersonCamera properties. (Non-deviated smoothing method)
            cameraTransformProperties = m_thirdPersonCamera.CalculateRotationAndPosition(deltaMouse, smoothingMethod: vec3 => Quaternion.Euler(vec3));

            // Apply properties to actual object.
            transform.SetPositionAndRotation(cameraTransformProperties.position, cameraTransformProperties.rotation);
        }
    }
}
