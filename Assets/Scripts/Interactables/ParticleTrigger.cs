using UnityEngine;

namespace Richie.GameProject
{
    public class ParticleTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 _offset;
        [SerializeField, Range(0, 100)] private int _threshold = 60;

        [Header("References")]
        [SerializeField] private GameObject _particles;

        private readonly Vector2Int _range = new (0, 101);

        private void OnTriggerEnter2D(Collider2D _)
        {
            int number = Random.Range(_range.x, _range.y);

            if (number < _threshold) return;
            Instantiate(_particles, new(transform.position.x + _offset.x, transform.position.y + _offset.y), Quaternion.Euler(-90, 0f, 0f), transform);
        } 
    }
}