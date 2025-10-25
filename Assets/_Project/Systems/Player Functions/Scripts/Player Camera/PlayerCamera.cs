using PlayerFunctions;
using UnityEngine;

namespace PlayerFunctionsCore
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private GameInput gameInput;

        void OnEnable()
        {
            gameInput = new();
        }
    }
}
