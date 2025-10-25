using System.Collections.Generic;
using UnityEngine;

namespace PlayerFunctionsCore
{
    [CreateAssetMenu(fileName = "ThirdPersonCameraRestraintSettings", menuName = "Scriptable Objects/ThirdPersonCameraRestraintSettings")]
    public class ThirdPersonCameraRestraintSettings : ScriptableObject
    {
        public List<LayerMask> CollidingWithLayers;
        public bool PassThroughAllLayers;
        public Vector3 LocalCameraCenterPoint;
    }
}
