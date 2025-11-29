using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivate : MonoBehaviour
{
    public Animator _anim;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameObject boss = GameObject.FindWithTag("Boss");
            if (boss != null)
            {
                AudioManager.instance.MusicPlay();
                boss.transform.Find("Graphics").gameObject.GetComponent<Animator>().SetTrigger("Activate"); ;
            }
            gameObject.SetActive(false);
        }
    }
}
