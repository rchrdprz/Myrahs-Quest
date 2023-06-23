using UnityEngine;

namespace Richie.GameProject
{
    public class DeathTrigger : MonoBehaviour
    {
        [SerializeField] private Respawner _respawner;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.TryGetComponent<PlayerMovement>(out _))
            {
                _respawner.Respawn();
            }
        }
    }
}
