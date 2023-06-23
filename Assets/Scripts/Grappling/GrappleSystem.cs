using UnityEngine;

namespace Richie.GameProject
{
    public class GrappleSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _force = 100f;

        [Header("References")]
        [SerializeField] private Transform _hands;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _grappleHook;
        [HideInInspector] public GameObject Hook;

        private PlayerAim _aim;
        private Rigidbody2D _rb;
        private LineRenderer _line;
        private PlayerGrappler _grappler;
        private Transform _startLocation;

        public event Anchored OnAchored;
        public delegate void Anchored(float distance);

        public event Release OnRelease;
        public delegate void Release();

        public event Cancel OnCancel;
        public delegate void Cancel();

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _aim = _player.GetComponent<PlayerAim>();
            _rb = _player.GetComponent<Rigidbody2D>();
            _grappler = _player.GetComponent<PlayerGrappler>();

            _startLocation = transform;
        }

        private void Start()
        {
            _aim.OnRelease += PlayerAim_OnRelease;
            _aim.OnCancel += PlayerAim_OnCancel;
            _grappler.OnJump += Grappler_OnJump;
        }

        private void Update() => DrawLine();

        private void DrawLine()
        {   // updates the lineRenderer to draw a line between the player and the hook //
            if (Hook == null || _line.positionCount <= 0) return;
            _line.SetPosition(0, _startLocation.position);
            _line.SetPosition(1, Hook.transform.position);
        }

        public void Deactivate()
        {   // resets values and start location of the hook //
            _line.positionCount = 0;
            _startLocation = transform;
            if (Hook != null) Hook.GetComponent<Hook>().Deactivate();
        }

        public void StopHook() => Hook.GetComponent<Hook>().StopHook();

        public void SetLength(float distance) 
            => Hook.GetComponent<Hook>().SetDistance(distance);


        // -- Event Methods -- //
        private void PlayerAim_OnRelease(Quaternion angle)
        {   // creates a hook that travels in the direction received from the "PlayerAim" script //
            if (Hook != null) Deactivate();

            Hook = Instantiate(_grappleHook, transform.position, angle);
            Hook.GetComponent<Rigidbody2D>().AddForce(Hook.transform.up * _force, ForceMode2D.Impulse);
            Hook.GetComponent<Hook>().OnHit += Hook_OnHit;
            Hook.name = "Grapple";

            // subscribe to the Onhit Event from the new hook //
            _line.positionCount = 2;
            OnRelease?.Invoke();
        }

        private void PlayerAim_OnCancel() => Deactivate();

        private void Grappler_OnJump(float input) => _startLocation = _hands;

        private void Hook_OnHit()
        {   // when Hook becomes collides, values are set and events are sent out to the "PlayerGrappler" script //
            float distance = Vector2.Distance(transform.position, Hook.transform.position);
            Hook.GetComponent<Hook>().Activate(_rb, distance);
            OnAchored?.Invoke(distance);
            OnCancel?.Invoke();
                
            // unsubscribes from the hook's event, as its no longer needed //
            Hook.GetComponent<Hook>().OnHit -= Hook_OnHit;
        }
    }
}