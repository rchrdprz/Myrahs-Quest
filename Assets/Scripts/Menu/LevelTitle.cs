using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class LevelTitle : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _appearTime;
        [SerializeField] private float _disapearTime;
        [SerializeField] private float _modifier;

        private TextMeshProUGUI _text;
        private float _counter = 0;
        private bool _deactivate;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();

            _counter -= _appearTime;
            _text.color = new(_text.color.r, _text.color.g, _text.color.b, 0);
        }

        private void Update() => Introduction();

        private void Introduction()
        {
            float temp = _counter * _modifier;

            if (!_deactivate && temp <= 1)
            {
                _counter += Time.deltaTime;
            }
            else if (!_deactivate && temp > 1)
            {
                _deactivate = true;
                _counter = _disapearTime;
            }
            else _counter -= Time.deltaTime;

            _text.color = new(_text.color.r, _text.color.g, _text.color.b, _counter);

            if (_deactivate && temp <= 0) gameObject.SetActive(false);
        }
    }
}