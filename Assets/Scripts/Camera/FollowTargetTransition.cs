using UnityEngine;

namespace Richie.GameProject
{
    public class FollowTargetTransition : FollowTarget
    {
        [SerializeField] private Vector2 _transitionOffset;
        [SerializeField] private float _newOffset;
        private bool _isBase;

        public override void Transition(Transform target)
        {   // used for the cinematic entrance at the start of each level //
            Vector3 targetPos = new(target.position.x - _transitionOffset.x, _transitionOffset.y, -10f);
            Vector3 position = new(transform.position.x, transform.position.y, -10f);

            if (!_isBase && Vector3.Distance(position, targetPos) >= _newOffset)
            {
                transform.position = Vector3.Lerp(position, targetPos, _transitonTime * Time.fixedDeltaTime);
            }
            else
            {
                _isBase = true;
                base.Transition(target);
            }
        }
    }
}
