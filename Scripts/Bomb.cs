using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.AssetImporters;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float blastradiuscale = 1;
    private float blastRadius = 1;

    public float blastForce;

    public float blastRagdollTime = 1;

    private Rigidbody2D rb;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Detonate()
    {
        anim.Play("bomb_blast");
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
    }

    [SerializeField] LayerMask mask;
    private bool disabled = false;
    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.bomb_blast") && !disabled)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            transform.localScale = transform.localScale * blastradiuscale;

            blastRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2 * blastradiuscale;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, blastRadius, mask);

            float destroyDelay = anim.GetCurrentAnimatorStateInfo(0).length;
            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("bomb")) continue;
                if (col.CompareTag("Player")) col.gameObject.GetComponent<Move>().ragdoll(blastRagdollTime);
                Vector2 newDir = ((Vector2)(col.transform.position - transform.position)).normalized;

                if (col.attachedRigidbody != null)
                    col.attachedRigidbody.AddForce(newDir * blastForce);
            }

            disabled = true;
            Destroy(gameObject, destroyDelay);
        }

    }
}
