using UnityEngine.UI;
using UnityEngine;

namespace Richie.GameProject
{
    public class FadeInEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _fadeTime = 3;

        [Header("References")]
        [SerializeField] private GameObject _mainMenu;

        private float _counter;
        private Image _fade;

        // this is used in the end screen when the screen faded in //
        void Start()
        {
            _fade = GetComponent<Image>();
            _counter = _fadeTime;

            _mainMenu.gameObject.SetActive(false);
            _fade.color = new(_fade.color.r, _fade.color.g, _fade.color.b, _counter);
        }

        void Update()
        {
            _counter -= Time.deltaTime;
            _fade.color = new(_fade.color.r, _fade.color.g, _fade.color.b, _counter * _speed);

            if (_counter <= 0) gameObject.SetActive(false);
        }

        // when the fade object gets deactivated, the back to main menu button appears //
        private void OnDisable() => _mainMenu.gameObject.SetActive(true);
    }
}
