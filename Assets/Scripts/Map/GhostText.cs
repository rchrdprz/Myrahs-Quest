using UnityEngine;
using TMPro;

namespace Richie.GameProject
{
    public class GhostText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private SpriteRenderer _renderer;

        // used with the Grappling Hook ghost, and is alpha is set based on the ghost's alpha //

        private void Start() 
            => _renderer = GetComponent<SpriteRenderer>();

        private void Update() 
            => _text.color = new(_text.color.r, _text.color.g, _text.color.b, _renderer.color.a);

        private void OnEnable() => _text.gameObject.SetActive(true);

        private void OnDisable() => _text.gameObject.SetActive(false);
    }
}