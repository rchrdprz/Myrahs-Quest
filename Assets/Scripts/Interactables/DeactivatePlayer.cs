using UnityEngine;

namespace Richie.GameProject
{
    public class DeactivatePlayer : MonoBehaviour
    {
        [SerializeField] private LevelEnd _end;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.transform.TryGetComponent(out Rigidbody2D rb))
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                rb.constraints = RigidbodyConstraints2D.None;
            }

            _end.TheEnd();
        }
    }
}