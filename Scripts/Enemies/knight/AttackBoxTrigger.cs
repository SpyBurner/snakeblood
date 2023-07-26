using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AttackBoxTrigger : MonoBehaviour
{
    private Stats statsScripts;

    private void Start()
    {
        statsScripts = GetComponentInParent<Stats>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float attackDirection = Mathf.Sign(collision.transform.position.x - transform.parent.transform.position.x);
            Vector2 force = Vector2.right * attackDirection * statsScripts.knockback + Vector2.up * statsScripts.knockback * 1 / 2;
            collision.GetComponent<Stats>().TakeDamage(statsScripts.damage, force);

            gameObject.SetActive(false);
        }
    }
}
