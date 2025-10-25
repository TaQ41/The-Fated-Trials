using System;
using UnityEngine;

namespace PlayerFunctionsCore
{
    public class ThirdPersonCamera
    {
        public Vector3 PivotPosition;
        public float PivotDistance;

        public Vector2 Sensitivity;
        public (float top, float bottom) VerticalClamp = (-90f, 90f);
        private float xRotation, yRotation;

        public (Quaternion rotation, Vector3 position) CalculateRotationAndPosition(Vector2 deltaMouse, Func<Vector3, Quaternion> smoothingMethod)
        {
            return (
                rotation: smoothingMethod(CalculateRotation(deltaMouse)),
                position: CalculatePosition(PivotPosition)
            );
        }

        public Vector3 CalculateRotation(Vector2 deltaMouse)
        {
            float mouseX = deltaMouse.x * Sensitivity.x * Time.deltaTime;
            float mouseY = deltaMouse.y * Sensitivity.y * Time.deltaTime;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, VerticalClamp.bottom, VerticalClamp.top);
            return new Vector3(xRotation, yRotation, 0f);
        }

        public Vector3 CalculatePosition(Vector3 pivot)
        {
            return pivot;
        }
    }
}
