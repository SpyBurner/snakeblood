using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class knight_Attack : BaseEnemyAttack
{
    public bool damageEnabled = false;

    private float attackDirection = 0;
    private Vector2 castDirection;

    private GameObject attackbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        statscript = GetComponent<Stats>();
        attackbox = transform.GetChild(1).gameObject;
        attackbox.SetActive(false);
    }
    private void Update()
    {
        //Debug Attack Range
        //drawline();
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            isOnAttackCooldown = false;
        }

        castDirection = Vector2.right * transform.localScale.x;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDirection, attackRange, targetLayer);
        if (hit.collider != null && !statscript.isAttacking && !isOnAttackCooldown)
        {
            Attack();
            attackDirection = Mathf.Sign(hit.collider.transform.position.x - transform.position.x);
        }

    }

    public override void Attack()
    {
        base.Attack();
        anim.Play("knight_attack");
        statscript.isAttacking = true;
    }

    public void EnableDamage()
    {
        damageEnabled = true;
        rb.AddForce(Vector2.right * attackDirection * statscript.knockback + Vector2.up * statscript.knockback * 1 / 2);

        attackbox.SetActive(true);
    }

    public void DisableDamage()
    {
        damageEnabled = false;
        statscript.isAttacking = false;
        anim.Play("knight_run");

        isOnAttackCooldown = true;
        attackCooldownTimer = attackCooldown;

        attackbox.SetActive(false);
    }

    private void drawline()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.positionCount = 2;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + (Vector3)castDirection * attackRange);
    }
}
