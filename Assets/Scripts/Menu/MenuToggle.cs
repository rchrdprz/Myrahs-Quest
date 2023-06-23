using UnityEngine;

namespace Richie.GameProject
{
    public class MenuToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        private PlayerInput _input;

        public event Pause OnPause;
        public delegate void Pause(bool state);

        private void Awake()
        {
            _input = new PlayerInput();
            _input.myrah.Tab.performed += ctx => SetMenu();
            _input.myrah.Esc.performed += ctx => SetMenu();
            _menu.SetActive(false);
        }

        private void SetMenu()
        {
            if (_menu.activeInHierarchy)
            {
                OnPause?.Invoke(false);
                _menu.SetActive(false);
            }
            else
            {
                OnPause?.Invoke(true);
                _menu.SetActive(true);
            }
        }

        private void OnEnable() => _input.Enable();

        private void OnDisable() => _input.Disable();
    }
}