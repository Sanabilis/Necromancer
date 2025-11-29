using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    public bool shouldRespawnEnemies = true;
    public bool isInteractable = true;
    public bool shouldHealPlayer = true;
    public GameObject playerReappearLocation = null;
    public PopUpMessage popup = null;
    public PopUpMessage gameSavedText = null;
    private bool _triggered;
    private float _cooldown = 2f;
    private float _timer = -1f;
    private bool _playerWantsToInteract;
    private GameObject _player;

    private InputMain _controls;

    void Awake()
    {
        _controls = new InputMain();

        _controls.Player.Interact.performed += _ => _playerWantsToInteract = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if ((!isInteractable || _playerWantsToInteract) &&
            _triggered && _timer <= 0 && !GameManager.Instance.IsGamePaused())
        {
            OnCheckpointTriggered();
        }

        _playerWantsToInteract = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _triggered = true;
            if (popup) popup.Display();
            _player = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _triggered = false;
            if (popup) popup.Hide();
        }
    }

    void OnCheckpointTriggered()
    {
        _timer = _cooldown;

        if (shouldHealPlayer)
        {
            _player.GetComponent<Damageable>().Heal();
            _player.GetComponent<CombatEffect>().PlayerGlow();
            _player.GetComponent<CombatEffect>().HealParticles();
            AudioManager.instance.Play("Heal");
        }

        GameManager.Instance.Save(playerReappearLocation != null
            ? playerReappearLocation.transform.position
            : _player.transform.position);

        if (shouldRespawnEnemies)
        {
            GameManager.Instance.RespawnEnemies();
        }

        if (gameSavedText) gameSavedText.Display();
    }

    void OnDrawGizmos()
    {
        if (!isInteractable) Gizmos.DrawIcon(transform.position, "Checkpoint", true);
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