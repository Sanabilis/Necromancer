using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackProjectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 3f;
    public float initialSpeed = 4f;
    public Transform center;

    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private float t = 0f;

    private PlayerCombat _player;
    public bool clockWise = true;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();

        if (!clockWise)
        {
            if (transform.position.y >= center.position.y)
                transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle((transform.position - center.position).normalized, Vector2.right));
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 360f - Vector2.Angle((transform.position - center.position).normalized, Vector2.right));
        }
        else
        {
            if (transform.position.y >= center.position.y)
                transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle((transform.position - center.position).normalized, Vector2.right));
            else
                transform.rotation = Quaternion.Euler(0f, 0f, 360f - Vector2.Angle((transform.position - center.position).normalized, Vector2.right));
        }

        _initialRotation = transform.rotation;

        if (!clockWise)
            _targetRotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + 90f);
        else
            _targetRotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z - 90f);
    }

    void FixedUpdate()
    {
        if (!clockWise)
            _targetRotation = Quaternion.Euler(0f, 0f, _initialRotation.eulerAngles.z + transform.parent.rotation.eulerAngles.z + 90f);
        else
            _targetRotation = Quaternion.Euler(0f, 0f, _initialRotation.eulerAngles.z + transform.parent.rotation.eulerAngles.z - 90f);

        transform.rotation = _targetRotation;

        if (!clockWise)
            transform.Rotate(0f, 0f, -90f);
        else
            transform.Rotate(0f, 0f, 90f);

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, t);
        Vector3 dir = (transform.position - center.position).normalized;
        transform.position = (transform.position + dir * Mathf.Lerp(speed, initialSpeed, t) * Time.fixedDeltaTime);

        t += Time.fixedDeltaTime / 2f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            PlayerCombat cSc = col.gameObject.GetComponent<PlayerCombat>();
            CharacterController mSc = col.gameObject.GetComponent<CharacterController>();

            if (!cSc.IsProtected() && !mSc.IsDashing() && !_player.IsProtected())
            {
                cSc.GetHit(damage);
                cSc.Stun(new Vector2(Mathf.Sign(_player.transform.position.x - transform.position.x), 2f).normalized * 5f);
            }

            Destroy(gameObject);
        }
    }
}