using UnityEngine.UI;
using UnityEngine;

namespace Richie.GameProject
{
    public class LevelEnd : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _sceneIndex;
        private readonly float _maxTime = 1;

        [Header("References")]
        [SerializeField] private Image _fade;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerAnimations _playerAnim;

        private float _counter;
        private bool _isActive;

        private void Start()
        {
            _counter = 0;
            _fade.color = new(_fade.color.r, _fade.color.g, _fade.color.b, _counter);
            _fade.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_isActive) return;
            Activate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _fade.gameObject.SetActive(true);

            _isActive = true;
            _playerAnim.Grounded();
            _playerManager.Deactivate();
        }

        private void Activate()
        {
            _counter += Time.deltaTime;

            _fade.color = new(_fade.color.r, _fade.color.g, _fade.color.b, _counter);
            if (_counter >= _maxTime) _sceneLoader.LoadScene(_sceneIndex);
        }

        public void TheEnd()
        {
            _fade.gameObject.SetActive(true);

            _isActive = true;
            _playerAnim.Grounded();
            _playerAnim.ResetAnims();
            _playerManager.Deactivate();
        }
    }
}