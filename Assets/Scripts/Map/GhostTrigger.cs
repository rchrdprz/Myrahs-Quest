using UnityEngine;

namespace Richie.GameProject
{
    public class GhostTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _ghost;
        private Animator _animator;

        // used for activating the ghosts tutorials //
        private void Start()
        {
            _animator = _ghost.GetComponent<Animator>();
            _ghost.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision) => _ghost.SetActive(true);

        public void SetGhostInactive() => _ghost.SetActive(true);
    }
}
