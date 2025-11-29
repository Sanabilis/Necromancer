using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController = null;

    [SerializeField] private GameObject _shieldIcon = null;

    private Animator _anim;
    private Player _movement;
    private Rigidbody2D _rb;
    private CameraScript _camScript;
    private Damageable _damageable;

    private float _blockTimer = 0f;
    private float _blockCooldown = 6f;
    private float _protectedTimer = 0f;
    private float _blockProtectionLength = 0.75f;
    private float _afterHitProtectionLength = 2f;

    private bool _isAttacking = false;
    private bool _isBlocking = false;
    private bool _hasBlocked = false;
    private bool _isStunned = false;

    private bool _shouldAttack = false;
    private int _attackIndex = 0;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();

        _controls.Player.Attack.performed += WantsToAttack;
        _controls.Player.Block.performed += WantsToBlock;
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _movement = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _camScript = GameObject.FindWithTag("Cinemachine").GetComponent<CameraScript>();
        _damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused()) return;

        if (_blockTimer > 0f)
            _blockTimer -= Time.deltaTime;

        if (_protectedTimer > 0f)
            _protectedTimer -= Time.deltaTime;

        _anim.SetBool("IsProtected", IsProtected());
    }

    private void WantsToBlock(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused()) return;

        if (!CanBlock()) return;

        _anim.SetTrigger("Block");
        _anim.SetBool("IsAttacking", false);

        GameObject[] attacks = GameObject.FindGameObjectsWithTag("PlayerAttack");

        foreach (GameObject a in attacks)
        {
            Destroy(a);
        }

        _isAttacking = false;
        _shouldAttack = false;
        _isBlocking = true;
        _hasBlocked = false;
    }

    private void WantsToAttack(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsGamePaused()) return;

        if (CanAttack())
        {
            _isAttacking = true;
            _anim.SetTrigger("Attack");
        }

        if (ShouldKeepAttacking())
        {
            _anim.SetBool("IsAttacking", true);
        }
    }

    public bool CanMove()
    {
        return !_isAttacking && !_anim.GetBool("IsAttacking") && !_isBlocking && !_isStunned;
    }

    public bool IsAttacking()
    {
        return _isAttacking || _anim.GetBool("IsAttacking");
    }

    public bool IsBlocking()
    {
        return _isBlocking;
    }

    public bool IsStunned()
    {
        return _isStunned;
    }

    public bool IsProtected()
    {
        return _protectedTimer > 0f;
    }

    public void ResetAttack()
    {
        _isAttacking = false;
    }

    public void ResetBlock()
    {
        _isBlocking = false;

        if (_hasBlocked)
        {
            SetBlockCooldown(_blockCooldown);
            _hasBlocked = false;
        }
        else
        {
            SetBlockCooldown(_blockCooldown / 2);
        }
    }

    public void ResetStun()
    {
        _isStunned = false;
    }

    public void ResetShouldAttack()
    {
        _shouldAttack = false;
    }

    private bool CanBlock()
    {
        return _blockTimer <= 0f && !_isBlocking && !_isStunned &&
               _characterController.IsGrounded() &&
               !_characterController.IsDashing() &&
               !_characterController.IsJumping() &&
               !_characterController.IsFalling() &&
               !_movement.IsAboutToJump();
    }

    private bool CanAttack()
    {
        return !_isAttacking && !_anim.GetBool("IsAttacking") &&
               !_isStunned && !_isBlocking &&
               _characterController.IsGrounded() &&
               !_characterController.IsDashing() &&
               !_characterController.IsJumping() &&
               !_characterController.IsFalling() &&
               !_movement.IsAboutToJump();
    }

    private bool ShouldKeepAttacking()
    {
        return IsAttacking() && _shouldAttack &&
               !_isStunned && !_isBlocking &&
               _characterController.IsGrounded() &&
               !_characterController.IsDashing() &&
               !_characterController.IsJumping() &&
               !_characterController.IsFalling() &&
               !_movement.IsAboutToJump();
    }

    public void ShouldAttack()
    {
        _shouldAttack = true;
        _anim.SetBool("IsAttacking", false);
    }

    public void GetHit(int dmg)
    {
        if (_isBlocking && !_hasBlocked)
        {
            AudioManager.instance.Play("Block");
            AudioManager.instance.Play("Heal");
            _anim.SetTrigger("BlockHit");
            _hasBlocked = true;
            _damageable.IncreaseCurrentHealth(10);
            SetBlockCooldown(_blockCooldown);
            _protectedTimer = _blockProtectionLength;
        }
        else if (_protectedTimer <= 0f && !_isBlocking)
        {
            AudioManager.instance.Play("Damaged");
            GetComponent<Damageable>().TakeDamage(dmg);
            _protectedTimer = _afterHitProtectionLength;
        }
    }

    public void Stun(Vector2 dir)
    {
        if (!_isBlocking || !_hasBlocked)
        {
            _anim.SetTrigger("Hit");
            _anim.SetBool("IsAttacking", false);

            _isStunned = true;
            _rb.velocity = dir;
            _camScript.Shake();

            GameObject[] attacks = GameObject.FindGameObjectsWithTag("PlayerAttack");

            foreach (GameObject a in attacks)
            {
                Destroy(a);
            }
        }
    }

    private void SetBlockCooldown(float length)
    {
        _blockTimer = length;
        _shieldIcon.GetComponent<Shield>().SetShieldTimer(length);
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void OnDisable()
    {
        _controls.Disable();
    }

    public void AttackSound()
    {
        _attackIndex++;
        _attackIndex %= 2;
        AudioManager.instance.Play("SwordSwing" + _attackIndex.ToString());
    }
}