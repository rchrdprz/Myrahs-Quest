using UnityEngine;

namespace Richie.GameProject
{
    public class FallingSpike : MonoBehaviour
    {
        [SerializeField] private Respawner _respawner;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _anim;
        private Vector3 _startPos;

        private void Start()
        {
            _rb.GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _startPos = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _anim.SetTrigger("isHit");
            if (other.transform.TryGetComponent<PlayerMovement>(out _))
                _respawner.Respawn();
        }

        public void Reset()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.position = _startPos;
            gameObject.SetActive(false);
        }
    }
}