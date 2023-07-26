using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    public float attackRange;

    public float attackCooldown;
    public bool isOnAttackCooldown;
    public float attackCooldownTimer;

    public LayerMask targetLayer;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Stats statscript;

    public virtual void Attack()
    {
    }
    

}
