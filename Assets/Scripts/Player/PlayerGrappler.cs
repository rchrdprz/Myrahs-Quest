using UnityEngine;

namespace Richie.GameProject
{
    public class PlayerGrappler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _ropeLength = 8;
        [SerializeField] private float _climbSpeed = 6f;
        [SerializeField] private float _swingForce = 6f;
        [SerializeField] private float _returnSpeed = 50f;
        private readonly float _rotSmooth = 0.5f;
        private readonly float _colHeight = 0.5f;

        [Header("Grapple Slide")]
        [SerializeField] private float _slideBuffer = 0.75f;

        [Header("References")]
        [SerializeField] private GrappleSystem _grapple;
        private PlayerMovement _movement;
        private BoxCollider2D _collider;
        private PlayerAim _playerAim;
        private PlayerInput _input;
        private Rigidbody2D _rb;
        private Hook _hook;

        [Header("Layer Mask")]
        [SerializeField] private LayerMask _whatIsGround;

        private Vector2 _direction, _colliderDefault, _returnLocation;
        private bool _isAnchored, _isJumping, _isCrouching, _isInitialJump, _isReturning;
        private float _lastInput, _hookSide, _signedAngle, _currentLength, _storedGravity;
        private float _highestVel, _bufferTimer;

        private bool IsGrounded => _movement.IsGrounded();
        private bool IsBonking => _movement.IsBonking();
        private bool IsTouching => _movement.IsTouching();

        // -- Events -- //
        public event Swing OnSwing;
        public delegate void Swing(bool state);

        public event Jump OnJump;
        public delegate void Jump(float input);

        public event Anchor OnAnchor;
        public delegate void Anchor();

        public event Retract OnRetract;
        public delegate void Retract();

        private void Awake()
        {
            _input = new();
            _rb = GetComponent<Rigidbody2D>();
            _playerAim = GetComponent<PlayerAim>();
            _collider = GetComponent<BoxCollider2D>();
            _movement = GetComponent<PlayerMovement>();

            _isInitialJump = true;
            _colliderDefault = _collider.size;
            _storedGravity = _rb.gravityScale;
            _bufferTimer = _slideBuffer;

            _playerAim.OnCancel += Aim_OnCancel;
            _grapple.OnAchored += Grapple_OnAchored;

            _input.myrah.jump.performed += ctx => _isJumping = true;
            _input.myrah.jump.canceled += ctx => _isJumping = false;

            _input.myrah.leftCtrl.performed += ctx => _isCrouching = true;
            _input.myrah.leftCtrl.canceled += ctx => _isCrouching = false;
        }

        private void Update()
        {
            GrappleReturn();

            if (!_isAnchored) return;
            _direction = _input.myrah.move.ReadValue<Vector2>();

            SlideBuffer();
            HookDirection();
            PlayerRotate();
            AnchoredJump();
            RopeLength();
        }

        private void FixedUpdate()
        {
            if (!_isAnchored) return;
            RopeSwing();
        }

        private void SlideBuffer()
        {   // this keeps track of the highest speed acheived for a short duration //
            _bufferTimer -= Time.deltaTime;
            if (_rb.velocity.sqrMagnitude > Mathf.Pow(_highestVel, 2) && _bufferTimer > 0)
            {   // this value is given to the "PlayerMovement" script to reward a boost to the slide 
                _highestVel = _rb.velocity.magnitude;
            }
            else
            {   // reset buffer values //
                _highestVel = 0;
                _bufferTimer = _slideBuffer;
            }
        }

        private void GrappleReturn()
        {   // grapple hook returns if longer than the rope length and it does not get anchored //
            if (_grapple.Hook == null) return;
            if (!_isReturning && Vector2.Distance(_grapple.Hook.transform.position, transform.position) >= _ropeLength)
            {
                _grapple.StopHook();
                _isReturning = true;
                _returnLocation = transform.position;
                OnRetract?.Invoke();
            }

            if (_isReturning && Vector2.Distance(_grapple.Hook.transform.position, _returnLocation) > 1f)
            {  // grapple hook is force driven, so a force is added to make it travel back towards a return location //
                Vector3 _direction = (_returnLocation - (Vector2)_grapple.Hook.transform.position);
                _grapple.Hook.transform.GetComponent<Rigidbody2D>().gravityScale = 0f;
                _grapple.Hook.transform.GetComponent<Rigidbody2D>().AddForce(_direction * _returnSpeed);
            }
            else if (_isReturning && Vector2.Distance(_grapple.Hook.transform.position, _returnLocation) < 1f)
            {  // when the hook reaches the return location, the hook gets deactivated from this script and the "GrappleSystem" scipt
                _hookSide = 0f;
                _grapple.Deactivate();
                Deactivate();
            }
        }

        private void HookDirection()
        {   // calculated the direction the player is traveling in, to be used for flipping the character when detaching // 
            if (transform.InverseTransformDirection(_rb.velocity).x < 0)
            {
                _hookSide = _lastInput * -1;
            }
            else _hookSide = _lastInput;
        }

        private void PlayerRotate()
        {   // rotates the character based on the last input, gennerally at the beginning of the swing where the last input is calculated //
            if (_grapple.Hook == null || IsGrounded || IsGrappleBelow()) return;

            Vector3 _direction = (_hook.transform.position - transform.position);
            float angle = (Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
            float zRotation = Mathf.Lerp(angle - 90, transform.rotation.z, _rotSmooth);

            if (_lastInput > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            }
            else if (_lastInput < 0)
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, -zRotation));
        }

        private void RopeSwing()
        {   // adds a relative force in the direction the player want to swing //
            if (_hook is null || IsGrounded) return;

            if (_direction.x > 0)
            {
                _rb.AddRelativeForce(Vector2.right * _swingForce);
            }
            else if (_direction.x < 0) 
                _rb.AddRelativeForce(Vector2.left * _swingForce);
        }

        private void RopeLength()
        {  // adjust the distance joint "max distance" value to make the player appear to be rappeling or climbing //
            if (_grapple.Hook == null) return;

            if (_direction.y > 0 || _isJumping && !IsBonking)
            {   // climb when pressing W or Jumping Key //
                _currentLength -= _climbSpeed * Time.deltaTime;
                _hook.SetDistance(_currentLength);
            }
            else if (_direction.y < 0 || _isCrouching && _currentLength < _ropeLength && !IsGrounded)
            {   // rappel when pressing S or Crouch Key //
                _currentLength += _climbSpeed * Time.deltaTime;
                _hook.SetDistance(_currentLength);
            }
            else
            {   // used for making rope shorter automatically when jumping or the player is grounded // 
                float distance = Vector2.Distance(transform.position, _hook.transform.position);
                if (distance < _currentLength) _hook.SetDistance(distance);
            }
        }

        private void AnchoredJump()
        {   // anchored jump is the first jump the player performs which then makes the player swing //
            if (IsGrounded) return;

            IsGrappledBehind();
            _lastInput = _movement.LastInput;

            _isInitialJump = false;
            _movement.IsActive = false;
            _rb.gravityScale = _storedGravity;
            _collider.size = new(_collider.size.x, _colHeight);

            OnJump?.Invoke(_lastInput);
            if (IsTouching && IsGrounded || IsGrappleBelow())
            {  // if hook is below the player or if the player is grounded, input is available //
                _movement.IsActive = true;
                OnSwing?.Invoke(false);
            }
            else OnSwing?.Invoke(true);
        }

        private void Deactivate()
        {   // reset a list of values when deactiving the grappling hook //
            _hook = null;
            _isAnchored = false;
            _isReturning = false;
            _isInitialJump = true;
            _movement.GrappleExit(_hookSide);
            _collider.size = _colliderDefault;
            _movement.GrappledVel = _highestVel;
            _bufferTimer = _slideBuffer;

            _highestVel = 0f;
            OnSwing?.Invoke(false);
        }

        // -- Event Methods -- //
        private void Grapple_OnAchored(float distance)
        {
            _isAnchored = true;
            _currentLength = distance;
            _movement.IsGrappling = true;
            _lastInput = _movement.LastInput;
            _hook = _grapple.Hook.GetComponent<Hook>();

            OnAnchor?.Invoke();
        }

        private bool IsGrappleBelow() 
        {
            if (_grapple.Hook == null) return false;
            return _grapple.Hook.transform.position.y < transform.position.y;
        }

        private void IsGrappledBehind()
        {  // uses the signed angle to determine where the hoook is relative to the player //
            if (_grapple.Hook == null || !_isInitialJump) return;

            Vector3 direction = _grapple.Hook.transform.position - transform.position;

            if (_lastInput > 0)
            {
                _signedAngle = Vector2.SignedAngle(transform.up, direction) * -1;
            }
            else _signedAngle = Vector2.SignedAngle(transform.up, direction);

            // used for flipping the character in the swing direction on first jump //
            if (_signedAngle < 0) _movement.LastInput *= -1;
        }

        private void Aim_OnCancel() => Deactivate();

        private void OnEnable() => _input.Enable();

        private void OnDisable() => _input.Disable();
    }
}