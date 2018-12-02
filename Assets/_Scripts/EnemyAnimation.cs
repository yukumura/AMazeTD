using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{

    private Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation(string animation)
    {
        anim.Play(animation);
    }

    public void PlayAttack()
    {
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                Debug.Log("Attack");
                anim.SetTrigger("Attack");
                break;

            case 2:
                Debug.Log("Attack_02");
                anim.SetTrigger("Attack_02");
                break;
        }
    }

    public void PlayIdle()
    {
        anim.SetBool("Walk", false);
    }

    public void PlayWalk()
    {
        Debug.Log("Walk");
        anim.SetBool("Walk", true);
    }

    public void PlayStun()
    {
        anim.Play("Stun");
    }

    public void PlayDead()
    {
        anim.Play("Death");
    }
}
