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
    public bool friendlyFire = false;
    public float blastradiuscale = 1;
    private float blastRadius = 0.2f;

    public float blastForce;

    public float blastRagdollTime = 1;

    private Rigidbody2D rb;
    private Animator anim;
    private Stats statscript;

    private Light2D attachedLight;
    private float bomb_sprite_radius;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        attachedLight = GetComponent<Light2D>();
        bomb_sprite_radius = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        statscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        damage = (int)Mathf.Ceil(statscript.damage / 2.0f);
        blastForce = 2 * statscript.knockback;
    }

    public Material matDetonate;
    public void Detonate()
    {
        anim.Play("bomb_blast");
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        attachedLight.intensity = 5 * blastradiuscale;
        GetComponent<SpriteRenderer>().material = matDetonate;
        
    }

    [SerializeField] LayerMask mask;
    private bool disabled = false;
    private void Update()
    {
        //Radius debug
        //DrawCircle(blastRadius);

        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.bomb_blast") && !disabled)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            transform.localScale = transform.localScale * blastradiuscale;

            Vector3 newPos = transform.position;
            newPos.y -= bomb_sprite_radius;
            transform.position = newPos;

            blastRadius = GetComponent<SpriteRenderer>().bounds.size.y / 2 * blastradiuscale;

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, blastRadius, mask);
            float destroyDelay = anim.GetCurrentAnimatorStateInfo(0).length;
            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("bomb")) continue;

                Stats statsScript = col.GetComponent<Stats>();

                //Player settings
                if (col.CompareTag("Player"))
                {
                    if (!friendlyFire) damage = 0;
                }

                Vector2 newDir = ((Vector2)(col.transform.position - transform.position)).normalized; 

                if (statsScript != null)
                {
                    statsScript.TakeDamage(damage, newDir * blastForce);
                }
            }

            disabled = true;
            Destroy(gameObject, destroyDelay);
        }
        if (disabled)
        {
            attachedLight.intensity -= 0.3f;
        }
    }

    public float debugLineWidth = 0.1f;
    private void DrawCircle(float radius)
    {
        LineRenderer circle = GetComponent<LineRenderer>();
        if (circle == null) circle = gameObject.AddComponent<LineRenderer>();

        circle.startWidth = 0.01f;
        circle.endWidth = 0.01f;
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
}
