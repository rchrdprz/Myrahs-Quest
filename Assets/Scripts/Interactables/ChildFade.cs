using UnityEngine;

namespace Richie.GameProject
{
    public class ChildFade : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private float _defaultAlpha;

        private void Start()
        {   // used to fade the shadow behind the disappearing platforms //
            _renderer = GetComponent<SpriteRenderer>();
            PlatformTimer parent = GetComponentInParent<PlatformTimer>();

            parent.OnFade += Parent_OnFade;
            _defaultAlpha = _renderer.color.a;
        }

        private void Parent_OnFade(float amount)
        {
            float value = amount >= 1 ? _defaultAlpha : amount;
            _renderer.color = new(_renderer.color.r, _renderer.color.g, _renderer.color.b, value);
        } 
    }
}
