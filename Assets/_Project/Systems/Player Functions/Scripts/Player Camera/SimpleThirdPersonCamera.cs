using PlayerFunctions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFunctionsCore
{
    public class SimpleThirdPersonCamera : MonoBehaviour
    {
        protected GameInput gameInput;
        protected Vector2 m_deltaMouse;
        protected float m_xRotation;
        protected float m_yRotation;

        public Vector2 Sensitivity;

        [SerializeField]
        private float m_distance;

        public bool test_toggle;

        protected virtual void OnEnable()
        {
            gameInput = new();
            gameInput.PlayerFunctions.Look.Enable();
            gameInput.PlayerFunctions.Look.performed += ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled += ResetDeltaMouse;
        }

        protected virtual void OnDisable()
        {
            gameInput.PlayerFunctions.Look.Disable();
            gameInput.PlayerFunctions.Look.performed -= ReadDeltaMouse;
            gameInput.PlayerFunctions.Look.canceled -= ResetDeltaMouse;
        }

        protected virtual void ReadDeltaMouse(InputAction.CallbackContext context) => m_deltaMouse = context.ReadValue<Vector2>();
        protected virtual void ResetDeltaMouse(InputAction.CallbackContext context) => m_deltaMouse = Vector2.zero;

        void Update()
        {
            RotateCamera();
            //SetCameraDisplacement();
            
            if (test_toggle)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(180f, 180f, 0f));
            }
        }

        // 1st impl - just get the new rotation and move the camera back a set amount.
        protected virtual void RotateCamera()
        {
            Vector2 mouse = m_deltaMouse * Sensitivity * Time.deltaTime;
            m_xRotation -= mouse.y;
            m_yRotation += mouse.x;

            m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(m_xRotation, m_yRotation, 0f);
        }

        private void SetCameraDisplacement()
        {
            //transform.position = new(0f, 10f, 0f);
            Ray ray = new(transform.position, transform.localRotation.eulerAngles);
            Physics.Raycast(ray, out RaycastHit hitInfo, m_distance, (LayerMask)0b11);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            Debug.Log(Quaternion.Inverse(transform.rotation).eulerAngles);
            //transform.position = hitInfo.point;
        }
    }
}
