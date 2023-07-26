using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhp = 4;
    //[NonSerialized]
    public int hp;
    public float movespeed = 3.0f;
    public int damage = 1;
    public float responseTime = 0;

    public float knockback = 100;

    public float immunityTime = 1;
    private float immunityTimer = 0f;

    [NonSerialized]
    public bool isDead = false;
    //[NonSerialized]
    public bool isGrounded = false;
    //[NonSerialized]
    public bool isRagdoll = false;
    [SerializeField]
    private float ragdollTimer = 0;

    [NonSerialized]
    public bool isAttacking = false;

    [NonSerialized]
    public bool isStunned = false;
    private float stunTimer;

    public bool godmode = false;

    private Rigidbody2D rb;
    private OnHitEffect onhitScript;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onhitScript = GetComponent<OnHitEffect>();
        anim = GetComponent<Animator>();
        hp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        if (ragdollTimer > 0)
        {
            ragdollTimer -= Time.deltaTime;
        }
        else
        {
            isRagdoll = false;
        }

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            isStunned = false;
            if (anim != null)
            {
                anim.speed = 1;
            }
        }

        //Effect on death
        if (hp <= 0)
        {
            if (!gameObject.CompareTag("Player"))
            Destroy(gameObject);
        }
    }

    public void HPChange(int diff)
    {
        if (godmode) return;
        hp -= diff;
        if (hp <= 0)
        {
            isDead = true;
        }
    }

    public void ragdoll(float t)
    {
        ragdollTimer = t;
        isRagdoll = true;
        isGrounded = false;
    }

    public void stun(float t)
    {
        stunTimer = t;
        isStunned = true;
        if (anim != null){
            anim.speed = 0;
        }
    }
    private void takeKnockback(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void TakeDamage(int damage, Vector2 knockbackForce)
    {
        if (immunityTimer > 0) return;

        if (onhitScript != null)
            onhitScript.TakeEffect(ref damage, ref knockbackForce);

        ragdoll(immunityTime);

        takeKnockback(knockbackForce);

        HPChange(damage);
        if (!isStunned)
        {
            immunityTimer = immunityTime;
        }
    }

}
