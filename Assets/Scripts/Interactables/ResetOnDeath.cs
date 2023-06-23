using UnityEngine;

namespace Richie.GameProject
{
    public class ResetOnDeath : MonoBehaviour
    {
        [SerializeField] private Respawner _respawner;
        [SerializeField] private Transform _startPosition;
        private Rigidbody2D _rb;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.TryGetComponent<PlayerMovement>(out _))
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                transform.position = _startPosition.position;
                _respawner.Respawn();
            }
        }
    }
}
