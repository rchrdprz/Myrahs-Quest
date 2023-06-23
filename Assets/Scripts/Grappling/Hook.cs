using UnityEngine;

namespace Richie.GameProject
{
    public class Hook : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private DistanceJoint2D _joint;

        public event HitCollision OnHit;
        public delegate void HitCollision();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _joint = GetComponent<DistanceJoint2D>();
            _joint.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {   // when the hook collides, the hook freeze in place and invokes an event //
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            OnHit?.Invoke();
        }

        public void StopHook()
        {   // method used for stopping the hook, so a new force applied //
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rb.constraints = RigidbodyConstraints2D.None;
        }

        public void Activate(Rigidbody2D target, float distance)
        {   // used for connected the player to the joint, settings the distance, and enabling the joint component //
            _joint.enabled = true;
            _joint.connectedBody = target;
            _joint.distance = distance;
        }

        public void SetDistance(float distance) => _joint.distance = distance;

        public void Deactivate()
        {
            _joint.enabled = true;
            Destroy(gameObject);
        }
    }
}