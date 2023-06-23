using System.Collections;
using UnityEngine;

namespace Richie.GameProject
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private PlayerInput _input;

        [SerializeField] private float _cooldown;
        private float _timer;

        public event Released OnRelease;
        public delegate void Released(Quaternion angle);
        
        public event Cancel OnCancel;
        public delegate void Cancel();

        private void Awake()
        {
            _input = new();
            _input.myrah.left.performed += ctx => Release();
            _input.myrah.left.canceled += ctx => OnCancel?.Invoke();
        }

        private void Release()
        {   // uses the cursors location to send a direction to the "GrapplingSystem" script 
            if (_timer > 0) return;
            Vector3 _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 _direction = (_mousePos - transform.position);

            float angle = (Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
            OnRelease?.Invoke(Quaternion.Euler(new Vector3(0f, 0f, angle - 90f)));

            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {  // small cooldown so the hook cannot be spammed as fast // 
            _timer = _cooldown;
            while (_timer >= 0)
            {  // this mechanic will probably need to be reworked // 
                _timer -= Time.deltaTime;
                yield return null;
            } 
        }

        private void OnEnable()
        {
            _timer = 0;
            _input.Enable();
        }

        private void OnDisable() => _input.Disable();
    }
}