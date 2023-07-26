using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LaunchAttackTrigger : MonoBehaviour
{
    private Rigidbody2D rb;
    private Stats statsScript;
    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        statsScript = transform.parent.GetComponent<Stats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Stats>().TakeDamage(statsScript.damage, rb.velocity);

        rb.velocity = -rb.velocity;
        statsScript.TakeDamage(0, Vector2.zero);
    }
}
