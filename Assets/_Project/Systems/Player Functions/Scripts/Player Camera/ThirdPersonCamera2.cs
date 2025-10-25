using UnityEngine;
using System;

namespace PlayerFunctionsCore
{
    public class ThirdPersonCamera2 : MonoBehaviour
    {
        // Test File!!
        // This file is used to prototype the architecture for the final 3rdPersonCamera Core Functionality in this library.
        // Assembly - PlayerFunctionsCore.asmdef

        [SerializeField]
        private ThirdPersonCameraRestraintSettings settings;

        void Update()
        {
            UpdateCamera();
        }

        // Allow external scripts to specify a smoothing method for rotation and position in settings.
        public delegate Vector3 SmoothingMethod(Vector3 element);
        public void UpdateCamera(SmoothingMethod rotationMethod, SmoothingMethod positionMethod)
        {
            Vector3 rotation = rotationMethod(CalculateNextRotationAngle(transform.localRotation.eulerAngles, new(), new()));
            Vector3 position = positionMethod(CalculatePhysicalCameraPosition(rotation, settings.LocalCameraCenterPoint, 3f, false));
            transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
        }

        public void UpdateCamera()
        => UpdateCamera((value) => value, (value) => value);

        public Vector3 CalculateNextRotationAngle(Vector3 currentAngle, Vector2 deltaAngle, Vector2 sensitivity)
        {
            return new Vector3
            (
                currentAngle.x + (deltaAngle.x * sensitivity.x * Time.deltaTime),
                currentAngle.y + (deltaAngle.y * sensitivity.y * Time.deltaTime),
                0f
            );
        }

        public Vector3 CalculatePhysicalCameraPosition(Vector3 angle, Vector3 position, float distance, bool pass_through = false)
        {
            // Shoot a raycast from a point and get where it lands after some distance.
            Ray ray = new(position, angle);
            if (pass_through) return ray.GetPoint(distance);
            
            RaycastHit[] hits = Physics.RaycastAll(ray, distance);
            if (hits.Length == 0)
            {
                return ray.GetPoint(distance);
            }

            // Check each hit to see if the layer or object is capable of stopping it.
            foreach (RaycastHit hit in hits)
            {
                if (settings.CollidingWithLayers.Contains((LayerMask)hit.transform.gameObject.layer))
                {
                    Debug.Log("Viable Collision Detected!");
                }
            }

            return default;
        }
    }
}
