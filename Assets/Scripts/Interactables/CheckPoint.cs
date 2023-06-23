using UnityEngine;

namespace Richie.GameProject
{
    public class CheckPoint : MonoBehaviour
    {
        private Animator _anim;
        private Respawner _respawner;
        private bool _isActive;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _respawner = transform.parent.GetComponent<Respawner>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActive) return;

            _respawner.CheckPoint = transform;
            _anim.SetBool("isActive", true);
            _isActive = true;
        }
    }
}