using System.Collections;
using UnityEngine;

namespace Richie.GameProject
{
    public class FallTrigger : MonoBehaviour
    {
        [SerializeField] private float _gravityScale = 5f;
        [SerializeField] private float _timeBetween = 1f;
        [SerializeField] private Rigidbody2D[] _links;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_links.Length <= 0) return;
            StartCoroutine(Dominoe());
        } 

        IEnumerator Dominoe()
        {
            for (int i = 0; i < _links.Length; i++)
            {
                _links[i].gravityScale = _gravityScale;
                _links[i].constraints = RigidbodyConstraints2D.None;
                _links[i].AddForce(Vector2.down);
                yield return new WaitForSeconds(_timeBetween);
            }
        }
    }
}