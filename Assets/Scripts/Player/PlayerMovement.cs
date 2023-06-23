using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _coyoteTime = 0.15f;
    [SerializeField] private float _crouchOffsetY = -0.1f;
    [SerializeField] private Vector2 _crouchSize = new(0.3f, 0.7f);

    [Header("Jump Settings")]
    [SerializeField] private int _multiJumps = 1;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _jumpTime = 0.2f;
    [SerializeField] private float _jumpBuffer = 0.15f;

    [Header("Ground Sliding")]
    [SerializeField] private float _slideForce = 6f;
    [SerializeField] private float _timeToSlide = 4f;
    [SerializeField] private float _slideOffsetY = -0.3f;
    [SerializeField] private float _slideAssist = 4f;
    [SerializeField] private Vector2 _slideSize = new(0.5f, 0.3f);

    [Header("Wall Sliding")]
    [SerializeField] private float _slideBuffer = 0.15f;
    [SerializeField] private float _slideSpeed = -1f;

    [Header("Wall Jumping")]
    [SerializeField] private float _jumpOffHeight = 18f;
    [SerializeField] private float _jumpOffForce = 8f;

    [Header("Visuals")]
    [SerializeField] private GameObject _wallDust;
    [SerializeField] private GameObject _jumpDust;

    [Header("References")]
    [SerializeField] private Transform _headCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _feetCheck;
    [SerializeField] private Transform _groundCheck;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask _whatIsGround;

    public float LastInput;
    public bool IsGrappleFall;
    public bool IsFlippable { get; set; }
    public bool IsGrappling { get; set; }
    public bool IsActive { get; set; }
    public float GrappledVel { get; set; }

    // -- Movement & Input -- //
    private bool _resumeInput, _canInput, _isCrouching;
    private float _input, _coyoteTimer;

    // -- Jumping -- //
    private float _bufferTimer, _maxJumpTime, _jumpCounter;
    private bool _isFirstJump, _isBufferJump, _isJumping, _isBonking;
    private int _jumpsRemaining;
    public float _jumpedInput;
    public bool _hasJumped;

    // -- Sliding -- //
    private readonly float _velocityModifer = 8f, _speedToSlide = 0.12f;
    public float _velocityCounter, _slideTimer;
    private bool _canSlide, _isSliding;

    // -- Wall Movement -- //
    private bool _isOnWall, _isFromWall;

    // -- Misc -- //
    private readonly float _checkRadius = 0.1f, _fallMultiplier = 0.75f;
    private Vector2 _startColSize, _startColOffset;
    private float _startGravity, _magnitude;
    private GameObject _marker;

    private BoxCollider2D _collider;
    private Rigidbody2D _rb;

    // -- Events -- //
    public event Move OnMovement;
    public delegate void Move(float input);

    public event Jump OnJump;
    public delegate void Jump(bool state);

    public event WallSlide OnWall;
    public delegate void WallSlide(bool state);

    public event Crouch OnCrouch;
    public delegate void Crouch(bool state);

    public event Slide OnSlide;
    public delegate void Slide(bool state);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        _jumpsRemaining = _multiJumps;
        _startColSize = _collider.size;
        _startGravity = _rb.gravityScale;
        _startColOffset = _collider.offset;

        _marker = new("Jump Dust");
        _resumeInput = true;
        IsActive = true;
    }

    void Update()
    {
        FlipSprite();
        GroundCheck();

        Jumping();
        Crouching();
        WallMovement();
    }

    private void FixedUpdate()
    {
        GroundSlide();
        Movement();
    }

    private void GroundCheck()
    {
        if (!IsActive) return;
        if (!_isSliding) _input = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            if (!_isSliding) _canInput = true;

            _isFromWall = false;
            _isFirstJump = false;
            _coyoteTimer = _coyoteTime;
            _jumpsRemaining = _multiJumps;
            _rb.gravityScale = _startGravity;
            OnJump?.Invoke(false);

            IsFlippable = true;
            if (IsBonking()) _isBonking = true;
        }
        else
        {
            _isSliding = false;
            _coyoteTimer -= Time.deltaTime;
            _collider.size = _startColSize;
            _collider.offset = _startColOffset;
            _resumeInput = true;
        }

        if (_coyoteTimer > 0f && !IsGrounded())
        {   // -- disables gravity for coyote time -- //
            _rb.gravityScale = 0f;
        }
        else _rb.gravityScale = _startGravity;
    }

    private void Movement()
    {
        if (!IsActive) return;

        if (_canInput)
        {
            _magnitude = _rb.velocity.sqrMagnitude;
            _rb.velocity = new(_input * _speed, _rb.velocity.y);

            if (_magnitude > 0f && Mathf.Abs(_input) > 0f)
            {
                OnMovement?.Invoke(_input);
            }
            else OnMovement?.Invoke(0);
        }

        if (IsFlippable)
        {
            if (_input > 0) LastInput = _input;
            else if (_input < 0) LastInput = _input;
        }
    }

    private void Jumping()
    {
        if (!IsActive) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {   // -- disable input at start of jump -- //
            _isBonking = false;
            _resumeInput = false;
        }

        if (Input.GetKey(KeyCode.Space) && _coyoteTimer > 0f)
        {   // -- resume input before landing -- //
            _coyoteTimer = 0f;
            if (!_isBonking && !_resumeInput)
            {
                if (_input != 0f) _canInput = false;
                OnJump?.Invoke(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !IsTouching())
        {   // -- jump buffer -- //
            _bufferTimer = _jumpBuffer;
        }
        else _bufferTimer -= Time.deltaTime;

        if (_bufferTimer > 0f && IsGrounded())
        {
            _resumeInput = false;
            _isBufferJump = true;
            _bufferTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jumpsRemaining == 0f || _isBufferJump)
        {   // -- single jump -- //
            _isBufferJump = false;
            _jumpedInput = LastInput;
            if (!_isFirstJump)
            {
                _isJumping = true;
                _maxJumpTime = _jumpTime;
                OnJump?.Invoke(true);

                if (_isSliding || _isFromWall)
                {
                    _rb.velocity = new(LastInput * _jumpForce, _jumpHeight);
                }
                else _rb.velocity = new(_input * _jumpForce, _jumpHeight);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jumpsRemaining > 0f && !IsTouching())
        {   // -- multi jump -- //
            _isJumping = true;
            IsFlippable = true;
            _jumpedInput = LastInput;
            _maxJumpTime = _jumpTime;
            OnJump?.Invoke(true);

            if (_isSliding || _isFromWall || IsGrappleFall)
            {
                _rb.velocity = new(LastInput * _jumpForce, _jumpHeight);
            }
            else _rb.velocity = new(_input * _jumpForce, _jumpHeight);

            if (_coyoteTimer < 0f)
            {
                 if(!IsTouching()) _jumpsRemaining--;
                Instantiate(_jumpDust, transform.position + new Vector3(-0.1f, -0.48f, 0f), Quaternion.identity, _marker.transform);
            }
        }

        if (Input.GetKey(KeyCode.Space) && _isJumping)
        {   // -- jump higher -- //
            if (_maxJumpTime > 0f)
            {
                _maxJumpTime -= Time.deltaTime;

                float t = _jumpCounter / _jumpTime;
                float currentHeight = _jumpHeight;

                // -- decrease velocity -- //
                if (t > 0.5f) currentHeight = _jumpHeight * (1 - t);
                _rb.velocity += currentHeight * Time.deltaTime * new Vector2(0, -Physics2D.gravity.y);
            }
            else _isJumping = false;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isJumping = false;
                _coyoteTimer = 0f;

                // -- faster decent -- //
                if (_rb.velocity.y > 0f) _rb.velocity = new(_rb.velocity.x, _rb.velocity.y * _fallMultiplier);
            }

            _hasJumped = true;
            _isFirstJump = true;
        }
    }

    private void WallMovement()
    {
        if (IsTouching())
        {
            if (!_isOnWall)
            {
                _isOnWall = true;
                _slideTimer = _slideBuffer;
            }

            _canInput = false;
            _canSlide = false;
            _isJumping = false;
            _isCrouching = false;
            IsFlippable = false;

            OnWall?.Invoke(true);
            OnSlide?.Invoke(false);
        }
        else
        {
            _isOnWall = false;
            OnWall?.Invoke(false);
        }

        if (_isOnWall)
        {   // -- wall sliding -- //
            _slideTimer -= Time.deltaTime;
            _rb.velocity = new(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_slideSpeed, float.MaxValue));

            if (_input == LastInput) _slideTimer = _slideBuffer;

            if (_slideTimer < 0f || (Input.GetKey(KeyCode.S) && _input == 0f))
            {
                IsFlippable = true;
                LastInput = -LastInput;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {   // -- wall jumping -- //
                if (IsFeetOnwall())
                {   // -- adjust visual position -- //
                    Instantiate(_wallDust, _wallCheck.position + new Vector3(-0.2f * LastInput, -0.3f, 0f), Quaternion.identity, _marker.transform);
                }
                else Instantiate(_wallDust, _wallCheck.position + new Vector3(-0.2f * LastInput, 0f, 0f), Quaternion.identity, _marker.transform);

                _canSlide = true;
                _isFromWall = true;
                _jumpedInput *= -1;
                _rb.velocity = new Vector2(-LastInput * _jumpOffForce, _jumpOffHeight);

                LastInput = -LastInput;
                OnJump?.Invoke(true);
            }
        }
    }

    private void Crouching()
    {
        if (!_isSliding)
        {   // calculate a simulated speed value that is stored in "_velocity counter" //
            if (Mathf.Abs(_input) > 0)
            {
                if (Mathf.Pow(_velocityCounter, 2) < _rb.velocity.sqrMagnitude)
                {
                    _velocityCounter += _velocityModifer * Time.deltaTime;
                }
                else if (Mathf.Pow(_velocityCounter, 2) > _rb.velocity.sqrMagnitude) _velocityCounter -= _velocityModifer * Time.deltaTime;
            }
            else _velocityCounter = 0f;
        } // this reaches the players actual speed overtime //
        else if (_velocityCounter > 0f) _velocityCounter -= _velocityModifer * Time.deltaTime;

        if (!IsGrounded()) return;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.S))
        {   // crouching mechanic // 
            _isCrouching = true;
            OnCrouch?.Invoke(true);
            if (Mathf.Abs(_input) <= 0f && !_isSliding)
            {  // normal crouching when not sliding //
                _collider.size = _crouchSize;
                _collider.offset = new(_collider.offset.x, _crouchOffsetY);
            }
            else if (_rb.velocity.sqrMagnitude > Mathf.Pow(0.1f, 2) && _isSliding)
            {   // change _collider hitbox to a size specific to sliding // 
                _collider.size = _slideSize;
                _collider.offset = new(_collider.offset.x, _slideOffsetY);
            }
            else
            {   // sets the _collider to its default values //
                _collider.size = _startColSize;
                _collider.offset = _startColOffset;
            }

            // "_canSlide" is a bool that allows slides to be performed,
            // if the players simulated speed has to be higher than certain value before a slide can be performed //
            if (!_canSlide && _velocityCounter > _timeToSlide) _canSlide = true;
        }
        else
        {   // resets all values relating to crouching when the player is not crouching //
            _canSlide = true;
            _isCrouching = false;
            _collider.size = _startColSize;
            _collider.offset = _startColOffset;

            IsGrappleFall = false;
            OnCrouch?.Invoke(false);
        }
    }

    private void GroundSlide()
    {
        if (!_canSlide || !IsGrounded() || (_hasJumped && LastInput != _jumpedInput))
        {
            _canSlide = false;
            OnSlide?.Invoke(false);
            return;
        }

        if (_isCrouching && !IsGrappling)
        {   // this is a small boost given to a grappled slide //
            if (IsGrappleFall)
            {
                if (GrappledVel > _rb.velocity.x)
                {  // the boost is the difference beteen the highest grappled velocity, and the players current x velocity //
                    float extra = (GrappledVel - _rb.velocity.x) > _slideAssist ? _slideAssist : GrappledVel - _rb.velocity.x;
                    _rb.velocity = LastInput > 0 ? new(_rb.velocity.x + extra, _rb.velocity.y) : _rb.velocity = new(_rb.velocity.x - extra, _rb.velocity.y);
                }

                GrappledVel = 0;
                IsGrappleFall = false;
            }

            if (_rb.velocity.sqrMagnitude > Mathf.Pow(_speedToSlide, 2))
            {  // disabling player input makes the player slide // 
                _canInput = false;
                _isSliding = true;
                IsFlippable = false;

                // a small force against the player makes the player gradually stop sliding//
                _rb.AddForce(-transform.right * _slideForce); 
                OnSlide?.Invoke(true);
            }
            else
            {   // reset values if conditions are not met //
                _canSlide = false;
                _isSliding = false;
                _hasJumped = false;
                OnSlide?.Invoke(false);
            }
        }
        else
        {   // reset values when conditions are not met //
            _hasJumped = false;
            _isSliding = false;
            OnSlide?.Invoke(false);
        }
    }

    private void FlipSprite()
    {
        if (LastInput > 0)
        {
            transform.eulerAngles = new(0, 0, 0);
        }
        else if (LastInput < 0)
        {
            transform.eulerAngles = new(0, 180f, 0);
        }
    }

    public void GrappleExit(float input)
    {   // method use by the "PlayerGrappler" script to make for code cleaner // 
        _canSlide = true;
        _canInput = false;
        _hasJumped = true;
        _jumpedInput = input;

        IsActive = true;
        LastInput = input;
        IsGrappling = false;
        IsGrappleFall = true;
    }

    public bool IsGrounded()   
        => Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _whatIsGround);

    public bool IsBonking()
        => Physics2D.OverlapCircle(_headCheck.position, _checkRadius, _whatIsGround);

    public bool IsTouching()
        => (IsGrounded()) ? false : Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _whatIsGround);

    private bool IsFeetOnwall()
        => (IsGrounded()) ? false : Physics2D.OverlapCircle(_feetCheck.position, _checkRadius, _whatIsGround);
}