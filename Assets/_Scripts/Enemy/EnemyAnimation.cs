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
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", true);

    }

    public void PlayIdle()
    {
        anim.SetBool("Walk", false);
    }

    public void PlayWalk()
    {
        anim.SetBool("Walk", true);
        anim.SetBool("Attack", false);
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
