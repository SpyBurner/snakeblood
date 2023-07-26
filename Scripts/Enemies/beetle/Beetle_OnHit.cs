using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle_OnHit : OnHitEffect
{

    private void Start()
    {
        statsScript = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void TakeEffect(ref int damage, ref Vector2 knockbackForce)
    {
        Debug.Log(knockbackForce.y);
        base.TakeEffect(ref damage,ref knockbackForce);
        if (knockbackForce.y > 0)
        {
            transform.Rotate(Vector3.forward, -180);
        }
        
        if (knockbackForce.y > 0 || transform.rotation.z == 0)
        {
            damage = 0;
        }
    }
}
