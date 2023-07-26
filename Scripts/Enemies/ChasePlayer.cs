using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public GameObject target;

    private Rigidbody2D rb;

    private Stats statsScript;
    private float turningTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        statsScript = GetComponent<Stats>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private float lastSignDiffX = 0;
    private bool pendingTurn = false;
    void Update()
    {
        if (statsScript.isStunned) return;
        //Turning timer
        if (turningTimer > 0)
        {
            turningTimer -= Time.deltaTime;
        }

        if (transform.position.x != target.transform.position.x && statsScript.isGrounded && !statsScript.isAttacking)
        {
            Vector2 direction = Vector2.right;
            float signDiffX = Mathf.Sign(target.transform.position.x - transform.position.x);
            
            //Delayed turning
            

            if (signDiffX != lastSignDiffX && !pendingTurn)
            {
                if (lastSignDiffX != 0)
                {
                    pendingTurn = true;
                    turningTimer = statsScript.responseTime;
                }
            }

            if (turningTimer <= 0)
            {
                lastSignDiffX = signDiffX;
                pendingTurn = false;
            }

            direction = direction * statsScript.movespeed * lastSignDiffX;
            rb.velocity = new Vector2(direction.x, rb.velocity.y);
        }
        //Flip x

        if (rb.velocity.x != 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Sign(rb.velocity.x) * Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }

    }

}
