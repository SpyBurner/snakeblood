using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float knockback;
    public int damage;
    public float stunTime = 0.3f;

    public int cutDir;

    private Stats statscript;

    private void Start()
    {
        statscript = GetComponentInParent<Stats>();

        knockback = statscript.knockback;
        damage = statscript.damage;

    }

    List<GameObject> touched = new List<GameObject>();
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision == null || collision.attachedRigidbody == null) return;

        if (touched.Contains(gameObject)) return;
        else
        {
            touched.Add(gameObject);
        }

        Vector2 direction = ((Vector2)(collision.transform.position - transform.position)).normalized;
        Vector2 knockbackForce = direction * knockback + Vector2.up * knockback * cutDir;

        Stats enemyStats = collision.GetComponent<Stats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage, knockbackForce);
        }
    }
}
