using UnityEngine;

namespace Richie.GameProject
{
    public class BirdSpawner : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _timer;
        [SerializeField] private Vector2 _offset = new(0.3f, 0.5f);

        [Header("References")]
        [SerializeField] private GameObject _birds;
        [SerializeField] private Transform _camera;

        private void Start() => _timer = _spawnTime;

        void Update() => Spawn();

        private void Spawn()
        {   // spawns the birds in the intro of the first level //
            _timer -= Time.deltaTime;

            if (_timer > 0) return;
            Instantiate(_birds, new(_camera.position.x + _offset.x, _camera.position.y + _offset.y), Quaternion.identity, _camera);
            gameObject.SetActive(false);
        }
    }
}