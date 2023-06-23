using UnityEngine;

namespace Richie.GameProject
{
    public class FollowTarget : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private float _bounds = 0f;
        [SerializeField] private float _xStop = 290.5f;
        [SerializeField] private float _followTime = 2.5f;
        [SerializeField] protected Vector2 _targetOffset = new(4f, 1f);

        [Header("Transition Settings")]
        [SerializeField] private bool _transition;
        [SerializeField] private float _offset = 1f;
        [SerializeField] protected float _transitonTime = 0.5f;

        [Header("References")]
        [HideInInspector] public Transform Target;
        [SerializeField] Transform _player;

        protected Behavior _behavior;
        public delegate void Behavior(Transform target);

        public event Switch OnSwitch;
        public delegate void Switch();

        private void Awake()
        {
            Target = _player;
            _behavior = _transition ? Transition : Follow;
        } 

        private void FixedUpdate() => _behavior(Target);

        protected void Follow(Transform target)
        {   // lerps to target position, does not move passed a certain x position, but may move back //
            Vector3 targetPos = new(target.position.x + _targetOffset.x, _targetOffset.y, -10f);
            Vector3 position = new(transform.position.x, transform.position.y, -10f);

            if (transform.position.x >= _xStop && targetPos.x >= _xStop) return;

            if (Vector3.Distance(position, targetPos) > _bounds)
                transform.position = Vector3.Lerp(position, targetPos, _followTime * Time.fixedDeltaTime);
        }

        public virtual void Transition(Transform target)
        {   // used for the cinematic entrance at the start of each level, then switches to "Follow" method //
            Vector3 targetPos = new(target.position.x + _targetOffset.x, _targetOffset.y, -10f);
            Vector3 position = new(transform.position.x, transform.position.y, -10f);

            if (Vector3.Distance(position, targetPos) >= _offset)
            {
                transform.position = Vector3.Lerp(position, targetPos, _transitonTime * Time.fixedDeltaTime);
            }
            else
            {
                _behavior = Follow;
                OnSwitch?.Invoke();
            }
        }
    }
}