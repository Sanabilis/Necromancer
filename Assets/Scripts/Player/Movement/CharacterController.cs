using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// From https://github.com/Brackeys/2D-Character-Controller
// customized

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 8.5f; // Amount of force added when the player jumps.
    [SerializeField] private float _dashForce = 13f; // Amount of force added when the player wants to dash.
    [SerializeField] private float _dashLength = 0.35f; // How long is the player in dash in seconds.
    [SerializeField] private float _fallLimit = -25f; // Limits the speed at which the player falls
    [SerializeField] private float _fallMultiplier = 2.5f; // Gravity scale multiplier when falling.
    [SerializeField] private float _jumpMultiplier = 2f; // Gravity scale multiplier when jumping.
    [SerializeField] private float _gravityMultiplier = 1f; // Gravity scale multiplier otherwise
    [SerializeField] private float _hangTime = 0.2f; // How long can the player still jump after falling off the platform.

    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f; // How much to smooth out the movement
    [SerializeField] private bool _airControl = true; // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask _whatIsGround = 0; // A mask determining what is ground to the character
    [SerializeField] private Transform _groundCheck = null; // A position marking where to check if the player is grounded.

    private const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool _grounded; // Whether or not the player is grounded.
    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true; // For determining which way the player is currently facing.
    private Vector3 _velocity = Vector3.zero;

    [Header("Events")] [Space] public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }

    private bool _jump;
    private bool _canDash;
    private bool _inDash;
    private float _dashTimer = -1f;
    private float _hangTimer;
    private bool _canMove = true;

    public float airSpeedMultiplier = 6f;
    public float groundSpeedMultiplier = 10f;

    public PlayerCombat combat;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, _whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                _canDash = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }

        if (_dashTimer > 0)
        {
            _rigidbody2D.gravityScale = 0f;
        }
        else
        {
            if (_inDash)
            {
                _inDash = false;
                _dashTimer = -1f;
                _rigidbody2D.velocity =
                    new Vector2(_rigidbody2D.velocity.x, 0f); // do not allow the player to "continue" jump after dash
            }

            if (_rigidbody2D.velocity.y < _fallLimit)
            {
                _rigidbody2D.gravityScale = 0f;
            }
            else if (_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.gravityScale = _fallMultiplier;
            }
            else if (_rigidbody2D.velocity.y >= 0 && !_jump)
            {
                _rigidbody2D.gravityScale = _jumpMultiplier;
            }
            else
            {
                _rigidbody2D.gravityScale = _gravityMultiplier;
            }
        }
    }


    public void Move(float move, bool jump, bool keepJump, bool dash)
    {
        _jump = keepJump;

        if (dash && _dashTimer <= 0f && _canDash)
        {
            _dashTimer = _dashLength;
            _inDash = true;
            _canDash = false;
            _rigidbody2D.velocity = new Vector2(0f, 0f);
            _rigidbody2D.AddForce(new Vector2(_dashForce * (_facingRight ? 1f : -1f), 0f), ForceMode2D.Impulse);
        }

        if (_dashTimer > 0f)
        {
            _dashTimer -= Time.deltaTime;
            return;
        }

        if (_grounded)
        {
            _hangTimer = _hangTime;
        }
        else
        {
            _hangTimer -= Time.deltaTime;
        }

        //only control the player if grounded or airControl is turned on
        if ((_grounded || _airControl) && _canMove)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * (_grounded ? groundSpeedMultiplier : airSpeedMultiplier), _rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity,
                _movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !_facingRight && !combat.IsAttacking())
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && _facingRight && !combat.IsAttacking())
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (jump && _hangTimer > 0)
        {
            // Add a vertical force to the player.
            _grounded = false;
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce - _rigidbody2D.velocity.y), ForceMode2D.Impulse);
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public bool IsGrounded()
    {
        return _grounded;
    }

    public bool IsDashing()
    {
        return _inDash;
    }

    public bool IsJumping()
    {
        return _rigidbody2D.velocity.y > 0.01f;
    }
	
	public bool IsFalling()
    {
        return _rigidbody2D.velocity.y < -0.01f;
    }

    public void CanMove()
    {
        _canMove = true;
    }

    public void CannotMove()
    {
        _canMove = false;
    }
}