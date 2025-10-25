using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFunctions.Camera
{

    /// <summary>
    /// 
    /// </summary>
    public class DirectCameraController : MonoBehaviour
    {
        [SerializeField]
        private GameInput gameInput;
        private Vector2 deltaMouse = new();

        private float xRotation = 0f;
        private float yRotation = 0f;

        void OnEnable()
        {
            gameInput = new();
            gameInput.PlayerFunctions.Look.Enable();
            gameInput.PlayerFunctions.Look.performed += ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled += ResetDeltaMouse;
        }

        void OnDisable()
        {
            gameInput.PlayerFunctions.Look.Disable();
            gameInput.PlayerFunctions.Look.performed -= ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled -= ResetDeltaMouse;
        }

        void ReadDeltaMouse(InputAction.CallbackContext context) => deltaMouse = context.ReadValue<Vector2>();
        void ResetDeltaMouse(InputAction.CallbackContext context) => deltaMouse = Vector2.zero;

        public void Update()
        {
            float xMouse = deltaMouse.x * 100f * Time.deltaTime;
            float yMouse = deltaMouse.y * 100f * Time.deltaTime;

            xRotation -= yMouse;
            yRotation += xMouse;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }

        public void RotateIn3rdPerson()
        {
            throw new NotImplementedException();
        }
    }
}