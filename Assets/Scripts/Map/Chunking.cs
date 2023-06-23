using UnityEngine;

namespace Richie.GameProject
{
    public class Chunking : MonoBehaviour
    {
        [SerializeField] private GameObject[] _previous;
        [SerializeField] private GameObject[] _next;

        // simple setting gameobjects active or inactive,
        // sometimes I needed to set two chunks inactive/Active at a time in the first level //

        private void OnTriggerEnter2D(Collider2D _)
        {
            if (_next.Length > 0)
            {
                foreach (var item in _next)
                    item.SetActive(true);
            }

            if (_previous.Length > 0)
            {
                foreach (var item in _previous)
                    item.SetActive(false);
            }
        }
    }
}