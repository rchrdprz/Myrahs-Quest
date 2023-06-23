using UnityEngine;

namespace Richie.GameProject
{
    public class ResetVisibility : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_objects.Length == 0) return;
            for (int i = 0; i < _objects.Length; i++)
            {
                _objects[i].SetActive(true);
                if (_objects[i].TryGetComponent(out Rigidbody2D rb))
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }
}