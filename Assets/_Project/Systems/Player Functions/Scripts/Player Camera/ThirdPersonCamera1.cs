using System;
using PlayerFunctions;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;

namespace PlayerFunctionsCore
{
    public class ThirdPersonCamera1 : MonoBehaviour
    {
        [SerializeField]
        private GameInput gameInput;

        // Rotate around this object
        [SerializeField]
        private GameObject go;

        public Vector2 Sensitivity;
        protected float xRotation, yRotation;
        private Vector2 deltaMouse;

        void OnEnable()
        {
            gameInput = new();
            gameInput.Enable();
            gameInput.PlayerFunctions.Look.performed += ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled += ResetDeltaMouse;
        }

        void OnDisable()
        {
            gameInput.Disable();
            gameInput.PlayerFunctions.Look.performed -= ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled -= ResetDeltaMouse;
        }

        private void ReadDeltaMouse(InputAction.CallbackContext context)
        {
            deltaMouse = context.ReadValue<Vector2>();
        }

        private void ResetDeltaMouse(InputAction.CallbackContext context)
        {
            deltaMouse = new(0, 0);
        }

        private void Update()
        {
            RotateCamera(deltaMouse, delegate(Vector3 value)
            {
                return Quaternion.Euler(value);
            });
        }

        public void RotateCamera(Vector2 deltaMouse, Func<Vector3, Quaternion> smoothingMethod)
        {
            Quaternion rotation;
            Vector3 position;

            rotation = smoothingMethod(CalculateRotation(deltaMouse));
            position = CalculatePosition(go.transform.position, 3f);
            transform.localPosition = position;
            transform.localRotation = rotation;
            go.transform.localRotation = rotation;
            return;
        }

        protected virtual Vector3 CalculateRotation(Vector2 deltaMouse)
        {
            float mouseX = deltaMouse.x * Sensitivity.x * Time.deltaTime;
            float mouseY = deltaMouse.y * Sensitivity.y * Time.deltaTime;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, -85f, 85f);
            return new Vector3(xRotation, yRotation, 0f);
        }

        protected virtual Vector3 CalculatePosition(Vector3 pivotPosition, float distance)
        {
            Vector3 ratios = new(
                Mathf.Sin(yRotation * Mathf.Deg2Rad) * Mathf.Cos(xRotation * Mathf.Deg2Rad),
                -1f * Mathf.Sin(xRotation * Mathf.Deg2Rad),
                Mathf.Cos(yRotation * Mathf.Deg2Rad) * Mathf.Cos(xRotation * Mathf.Deg2Rad)
            );

            return pivotPosition - (distance * ratios);
        }
    }
}