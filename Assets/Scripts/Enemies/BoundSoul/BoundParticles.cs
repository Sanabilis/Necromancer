using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundParticles : MonoBehaviour
{
	public Transform target;

	private ParticleSystem _system;

	public Color lightBlue;
	public Color lightRed;
	public Color darkRed;

	private float _hitTimer = 0f;
	private bool _origColor = true;

	void Awake()
    {
		_system = GetComponent<ParticleSystem>();
    }

	void Update()
	{
		if (_hitTimer > 0f)
        {
			_hitTimer -= Time.deltaTime;
        }
		else if (!_origColor)
        {
			ResetColor();
		}

		if (target != null)
			transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x));
	}

	public void OnHit()
    {
		_hitTimer = 1f / 12f;
		_origColor = false;

		var col = _system.colorOverLifetime;

		Gradient grad = new Gradient();
		grad.SetKeys(new GradientColorKey[] { new GradientColorKey(lightBlue, 0f), new GradientColorKey(lightBlue, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });

		col.color = grad;
	}

	public void ResetColor()
    {
		_origColor = true;

		var col = _system.colorOverLifetime;

		Gradient grad = new Gradient();
		grad.SetKeys(new GradientColorKey[] { new GradientColorKey(lightRed, 0f), new GradientColorKey(darkRed, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0f), new GradientAlphaKey(0f, 1f) });

		col.color = grad;
    }
}
