using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public float entryShakeAmplitude = 0.7f;
	public float entryShakeDuration = 0.5f;

	public float stayShakeAmplitude = 0.3f;
	public float stayshakeDuration = 0.2f;

	public float damageTickInterval;
	public int tickDamage;
	public int entryDamage;

	private float tickTimer;
	private CameraScript _cam;



	void Update()
    {
		if (tickTimer > 0f)
        {
			tickTimer -= Time.deltaTime;
		}
	}

	private void Start()
    {
		_cam = GameObject.FindWithTag("Cinemachine").GetComponent<CameraScript>();

	}

	// Update is called once per frame
	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
			case "Player":
				if (tickTimer <= 0f)
				{
					other.gameObject.GetComponent<PlayerCombat>().GetHit(entryDamage);
					Vector2 dir = new Vector2(-other.transform.localScale.x, 2f).normalized * 5f;
					other.gameObject.GetComponent<PlayerCombat>().Stun(dir);

					_cam.Shake(entryShakeAmplitude, entryShakeDuration);
					tickTimer = damageTickInterval;
				}

				break;
			case "Enemy":
				other.gameObject.GetComponent<EnemyScript>().Die();
				break;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		//Debug.Log(other);
		switch (other.gameObject.tag)
		{
			case "Player":
				if (tickTimer <= 0f)
				{
					other.gameObject.GetComponent<PlayerCombat>().GetHit(entryDamage);
					Vector2 dir = new Vector2(-other.transform.localScale.x, 2f).normalized * 5f;
					other.gameObject.GetComponent<PlayerCombat>().Stun(dir);

					_cam.Shake(stayShakeAmplitude, stayshakeDuration);
					tickTimer = damageTickInterval;
				}

				break;
		}
	}
}
