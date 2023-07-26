using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_OnHit : OnHitEffect
{
    public float stunTime = 2;

    private Animator anim;

    public override void TakeEffect(ref int damage, ref Vector2 knockbackForce)
    {
        if (!statsScript.isStunned && statsScript.isAttacking)
        {
            damage = 0;
            statsScript.stun(stunTime);

            GetComponent<knight_Attack>().DisableDamage();
        }
    }

}
