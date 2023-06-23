using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class TextColor : MonoBehaviour
    {
        [SerializeField] private Color _active;
        [SerializeField] private Color _inactive;

        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.color = _inactive;
        }

        public void Active() => _text.color = _active;

        public void Inactive() => _text.color = _inactive;
    }
}