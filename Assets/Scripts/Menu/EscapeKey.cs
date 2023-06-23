using UnityEngine;

namespace Richie.GameProject
{
    public class EscapeKey : MonoBehaviour
    {
        private PlayerInput _input;

        private void Awake()
        {
            _input = new();
            _input.myrah.Esc.performed += ctx => gameObject.SetActive(false);
        }

        private void OnEnable() => _input.Enable();

        private void OnDisable() => _input.Disable();
    }
}