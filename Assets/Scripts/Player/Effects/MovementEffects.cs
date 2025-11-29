using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffects : MonoBehaviour
{
	public GameObject echoEffect;

	public GameObject jumpParticles;
	public GameObject takeOffParticles;
	public GameObject landParticles;

	public GameObject dashParticles;

	private float _dashDir = 1;
	public Transform groundCheck;

	public void LandParticles()
	{
		Vector3 offset = new Vector3(0.25f, 0f, 0f);
		Vector3 pos;
		RaycastHit2D hit;

		hit = Physics2D.Raycast(transform.position - offset, -Vector3.up, 1.2f, LayerMask.GetMask("Ground"));

		if (hit.collider != null)
		{
			pos = hit.point;
			Instantiate(landParticles, pos + offset, Quaternion.identity, transform);
		}
		else
		{
			hit = Physics2D.Raycast(transform.position + offset, -Vector3.up, 1.2f, LayerMask.GetMask("Ground"));

			if (hit.collider != null)
			{
				pos = hit.point;
				Instantiate(landParticles, pos - offset, Quaternion.identity, transform);
			}
		}
	}

	public void JumpParticles()
	{
		Instantiate(takeOffParticles, groundCheck.position, Quaternion.identity, transform);
		Instantiate(jumpParticles, groundCheck.position, Quaternion.identity, transform);
	}

	public void DashParticles()
	{
		GameObject dashP = Instantiate(dashParticles, groundCheck.position, Quaternion.identity, transform);
		dashP.transform.localScale = new Vector3(_dashDir, 1f, 1f);
	}

	public void SetDashDir()
	{
		_dashDir = transform.localScale.x;
	}

	public void SpawnEcho()
	{
		GameObject echo = Instantiate(echoEffect, transform.position, Quaternion.identity);
		echo.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		echo.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		echo.GetComponent<SpriteRenderer>().sortingOrder = -2;
		echo.transform.localScale = transform.localScale;
	}

	public void JumpSound()
	{
		AudioManager.instance.Play("Jump");
	}

	public void LandSound()
    {
		AudioManager.instance.Play("Land");
	}

	public void StepSound()
	{
		AudioManager.instance.Play("Step");
	}
}
