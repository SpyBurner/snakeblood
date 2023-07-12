using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    public float movespeed;
    public float chargeforce;
    public float chargedelay;


    private Animator animator;
    private Rigidbody2D rb;

    private PhysicsMaterial2D material;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private bool isMoving = false;

    [NonSerialized]
    public bool isGrounded = false;

    private bool isCharging = false;
    private bool isCharged = false;
    private bool isAttacking = false;

    private float charge_time_start;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            transform.GetComponentInChildren<SwingWeapon>().Swing();
        }
        //position
        if (Time.time - ragdollStart >= ragdollTime)
        {
            isRagdoll = false;
        }

        if (isGrounded && !(isAttacking || isCharging))
        {
            Vector2 newdir = Vector2.zero;
            newdir.x += Input.GetAxisRaw("Horizontal") * movespeed;
            rb.velocity = new Vector2(newdir.x, rb.velocity.y);
            if (newdir.x != 0) isMoving = true; else isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            charge_time_start = Time.time;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - charge_time_start >= chargedelay)
            {
                isCharged = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            isCharged = false;
            isGrounded = false;
            if (Time.time - charge_time_start >= chargedelay){
                transform.position = transform.position + (0.2f * Vector3.up);
                Vector2 toward_cursor = ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
                Debug.Log(toward_cursor.magnitude);
                rb.AddForce(toward_cursor * chargeforce);
                Debug.Log(toward_cursor.magnitude);
                ragdoll(0.5f);
            }
        }

        if (rb.velocity.x != 0) transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        if (isCharging){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localScale = new Vector3(Mathf.Sign(mousePos.x - transform.position.x), 1, 1);
        }

        UpdateAnimation();
    }

    [NonSerialized] public bool isRagdoll = false;
    public float ragdollTime = 1;

    private float ragdollStart;
    public void ragdoll(float t)
    {
        ragdollStart = Time.time;
        ragdollTime = t;
        isRagdoll = true;
    }

    private string currentState = "player_idle";
    private string nextState = "";

    public bool isLicking = false;


    private void UpdateAnimation()
    {
        if (!isMoving)
        {
            int r = UnityEngine.Random.Range(0, 1000);
            if (r != 999 && !isLicking) nextState = "player_idle";
            else
            {
                nextState = "player_idle2";
                isLicking = true;
            }
        }
        if (isMoving && isGrounded)
        {
            nextState = "player_move";
        }

        if (isCharging)
        {
            nextState = "player_charging";
            if (isCharged)
            {
                nextState = "player_chargeReady";
            }
        }

        if (nextState == currentState) return;

        currentState = nextState;

        animator.Play(currentState);
    }

}
