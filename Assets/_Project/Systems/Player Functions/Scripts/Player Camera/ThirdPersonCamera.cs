using System;
using UnityEngine;

namespace PlayerFunctionsCore
{
    /// <summary>
    /// Provides classes the ability to calculate values for 3rd person cameras while responding to collisions in the environment.
    /// </summary>
    [Serializable]
    public class ThirdPersonCamera
    {
        public Vector3 PivotPosition;
        public float PivotDistance;

        public Vector2 Sensitivity;
        public LayerMask CameraCollisionLayers;

        [Serializable]
        public struct VerticalClampSettings
        {
            public float bottom, top;

            [Sirenix.OdinInspector.Button]
            public void SetClampEqually(float value)
            {
                top = Mathf.Abs(value);
                bottom = -top;
            }
        }

        public VerticalClampSettings VerticalClamp;
        private float xRotation, yRotation;

        public (Quaternion rotation, Vector3 position) CalculateRotationAndPosition(Vector2 deltaMouse, Func<Vector3, Quaternion> smoothingMethod)
        {
            Vector3 rotationVector = CalculateRotation(deltaMouse);

            return (
                rotation: smoothingMethod(rotationVector),
                position: CalculatePosition(rotationVector)
            );
        }

        private Vector3 CalculateRotation(Vector2 deltaMouse)
        {
            float mouseX = deltaMouse.x * Sensitivity.x * Time.deltaTime;
            float mouseY = deltaMouse.y * Sensitivity.y * Time.deltaTime;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, VerticalClamp.bottom, VerticalClamp.top);
            if (yRotation > 360) yRotation -= 360;
            else if (yRotation < -360) yRotation += 360;

            return new Vector3(xRotation, yRotation, 0f);
        }

        private Vector3 CalculatePosition(Vector3 rotation)
        {
            // Multiply the rotation quaternion by the backwards vector to get a normalized direction looking behind the camera for raycasts.
            Vector3 direction = Quaternion.Euler(rotation) * Vector3.back;

            // Has an issue with clipping through objects when it is angled slightly above them and the object's surface is flat horizontal.
            Ray ray = new Ray(PivotPosition, direction);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, PivotDistance, CameraCollisionLayers))
            {
                return hitInfo.point;
            }

            return ray.GetPoint(PivotDistance);
        }
    }
}