using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.AssetImporters;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bomb : MonoBehaviour
{
    public int damage = 0;
    public float blastradiuscale = 1;
    private float blastRadius = 1;

    public float blastForce;

    public float blastRagdollTime = 1;

    private Rigidbody2D rb;
    private Animator anim;

    private Light2D light;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        light = GetComponent<Light2D>();
    }

    public Material matDetonate;
    public void Detonate()
    {
        anim.Play("bomb_blast");
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        light.intensity = 5 * blastradiuscale;
        GetComponent<SpriteRenderer>().material = matDetonate;
    }

    [SerializeField] LayerMask mask;
    private bool disabled = false;
    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.bomb_blast") && !disabled)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            transform.localScale = transform.localScale * blastradiuscale;

            transform.position = new Vector3(contactPoint.x, contactPoint.y, transform.position.z);

            blastRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2 * blastradiuscale;
            
            //Radius debug
            //DrawCircle(blastRadius);

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, blastRadius, mask);
            float destroyDelay = anim.GetCurrentAnimatorStateInfo(0).length;
            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("bomb")) continue;
                if (col.CompareTag("Player"))
                {
                    col.gameObject.GetComponent<Move>().ragdoll(blastRagdollTime);

                    //Bomb dmg debug
                    col.gameObject.GetComponent<Stats>().HPchange(-damage);
                }
                Vector2 newDir = ((Vector2)(col.transform.position - transform.position)).normalized;

                if (col.attachedRigidbody != null)
                {
                    col.attachedRigidbody.AddForce(newDir * blastForce);
                }
                    //col.attachedRigidbody.velocity = Vector2.zero;
            }

            disabled = true;
            Destroy(gameObject, destroyDelay);
        }
        if (disabled)
        {
            light.intensity -= 0.3f;
        }
    }

    private void DrawCircle(float radius)
    {
        LineRenderer circle = gameObject.AddComponent<LineRenderer>();
        circle.positionCount = 100;

        float theta = 0;

        for (int i = 0; i < 100; ++i)
        {
            theta = i * 2 * Mathf.PI / 100;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);

            circle.SetPosition(i, transform.position +  new Vector3(x, y, 0));
            
        }
    }

    Vector2 contactPoint;
    private void OnCollisionStay2D(Collision2D collision)
    {
        contactPoint = collision.GetContact(0).point;
    }
}
