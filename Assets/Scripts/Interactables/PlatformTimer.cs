using UnityEngine;

namespace Richie.GameProject
{
    public class PlatformTimer : MonoBehaviour
    {
        [SerializeField] private float _time;
        private SpriteRenderer _renderer;
        private bool _isActive;
        public float _counter;

        private Hook _hook;

        public event Fade OnFade;
        public delegate void Fade(float amount);

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _counter = _time;
        }

        private void Update()
        {
            if (!_isActive) return;
            _counter -= Time.deltaTime;

            OnFade?.Invoke(_counter);
            _renderer.color = new(_renderer.color.r, _renderer.color.g, _renderer.color.b, _counter);
            if (_counter <= 0)
            {
                if(_hook != null) _hook.StopHook();
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.TryGetComponent(out Hook hook))
                _hook = hook;
                
            _isActive = true;
        }

        private void OnDisable()
        {
            _counter = _time;
            _isActive = false;
            if (_renderer != null)
            _renderer.color = new(_renderer.color.r, _renderer.color.g, _renderer.color.b, _counter);
            OnFade?.Invoke(_counter);
        }
    }
}
