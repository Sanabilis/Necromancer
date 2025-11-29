using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController = null;
    [SerializeField] private float _speed = 45f;

    private float _horizontalMove;
    private bool _jump;
    private bool _keepJump;
    private bool _wantsToJump;
    private bool _jumpHeld;
    private bool _dash;
    private bool _wantsToDash;

    private Animator _anim;
    private Rigidbody2D _rb;
    private bool _collided;

    private float _attackTimer = 0f;
    private float _dashTimer = 0f;
    private float _jumpTimer = -1f;
    private bool _shouldJump = true;

    private int _dashCounter = 1;

    private PlayerCombat _playerCombat;

    [SerializeField] private InventorySO _inventory;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();

        _controls.Player.Jump.started += context =>
        {
            if (GameManager.Instance.IsGamePaused()) return;
            _wantsToJump = true;
            _jumpHeld = true;
        };
        _controls.Player.Jump.canceled += _ =>
        {
            _jumpHeld = false;
            _wantsToJump = false;
        };
        _controls.Player.Dash.performed += context =>
        {
            if (GameManager.Instance.IsGamePaused()) return;
            _wantsToDash = true;
        };
        _controls.Player.Dash.canceled += _ => _wantsToDash = false;
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerCombat = GetComponent<PlayerCombat>();
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused())
        {
            _horizontalMove = 0;
            _jump = false;
            _keepJump = false;
            _dash = false;
            return;
        }

        if (_attackTimer > 0f)
            _attackTimer -= Time.deltaTime;

        if (_dashTimer > 0f)
            _dashTimer -= Time.deltaTime;

        if (_jumpTimer > 0f)
            _jumpTimer -= Time.deltaTime;

        if (!_playerCombat.CanMove())
        {
            if (_playerCombat.IsStunned())
                _characterController.CannotMove();

            _horizontalMove = 0f;
            _jumpTimer = -1f;
            return;
        }

        _characterController.CanMove();
        if (_wantsToJump && _characterController.IsGrounded() &&
            !_characterController.IsDashing() && !_characterController.IsJumping())
        {
            _jumpTimer = 1f / 12f;
            _shouldJump = false;
            _anim.SetTrigger("Jump");
        }

        _wantsToJump = false;


        if (_wantsToDash && _dashTimer <= 0f && _dashCounter > 0)
        {
            if (_characterController.IsJumping())
                _anim.SetTrigger("JumpToDash");
            else if (_characterController.IsGrounded())
                _anim.SetTrigger("StandToDash");
            else
                _anim.SetTrigger("FallToDash");

            _dashTimer = 1f;
            _dashCounter--;
            _dash = true;
        }

        _wantsToDash = false;


        if (_jumpTimer <= 0f && !_shouldJump)
        {
            _jump = true;
            _shouldJump = true;
        }
        else if (_jumpHeld && _shouldJump)
        {
            _keepJump = true;
        }
        else
        {
            _keepJump = false;
        }

        _horizontalMove = _controls.Player.Movement.ReadValue<Vector2>().x * _speed;
    }

    void FixedUpdate()
    {
        if (_characterController.IsGrounded())
            _dashCounter = 1;

        if (!_playerCombat.CanMove())
        {
            _horizontalMove = 0f;
            _shouldJump = true;
        }

        _characterController.Move(_horizontalMove * Time.fixedDeltaTime, _jump, _keepJump, _dash);

        _jump = false;
        _dash = false;

        _anim.SetBool("IsDashing", _characterController.IsDashing());
        _anim.SetBool("IsRunning", _horizontalMove != 0f);
        _anim.SetBool("IsJumping", _characterController.IsJumping());
        _anim.SetBool("IsGrounded", _characterController.IsGrounded());
        _anim.SetBool("IsFalling", _characterController.IsFalling());
    }

    private void WantsToJump()
    {
        if (!_characterController.IsGrounded() || _characterController.IsDashing() ||
            _characterController.IsJumping()) return;

        _jumpTimer = 1f / 12f;
        _shouldJump = false;
        _anim.SetTrigger("Jump");
        _jumpHeld = true;
    }

    private void WantsToDash()
    {
        if (!(_dashTimer <= 0f) || _dashCounter <= 0) return;

        if (_characterController.IsJumping())
            _anim.SetTrigger("JumpToDash");
        else if (_characterController.IsGrounded())
            _anim.SetTrigger("StandToDash");
        else
            _anim.SetTrigger("FallToDash");

        _dashTimer = 1f;
        _dashCounter--;
        _dash = true;
    }

    public bool IsAboutToJump()
    {
        return !_shouldJump;
    }

    public InventorySO GetPlayerInventory()
    {
        return _inventory;
    }
    public void LoadPlayerInventory(InventorySO inventory)
    {
        _inventory = inventory;
    }

    public void AddItem(ItemSO item, int amount)
    {
        _inventory.AddItem(item, amount);
    }

    private void OnApplicationQuit()
    {
        _inventory.Container.Clear();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Destructible")
        {
            DestructibleWall sc = col.gameObject.GetComponent<DestructibleWall>();
            if (sc.type == DestructibleType.Fragile)
            {
                sc.Destroy();
            }
        }
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void OnDisable()
    {
        _controls.Disable();
    }
}