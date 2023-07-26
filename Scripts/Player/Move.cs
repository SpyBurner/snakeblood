using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Move : MonoBehaviour
{
    public float movespeed;
    public float chargeforce;
    public float chargedelay;

    private Animator animator;
    private Rigidbody2D rb;

    private PhysicsMaterial2D material;
    private SwingWeapon swingScript;
    private Stats statsScript;

    [NonSerialized]
    public GameObject LaunchAttackBox;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swingScript = GetComponentInChildren<SwingWeapon>();
        statsScript = GetComponentInChildren<Stats>();

        LaunchAttackBox = transform.GetChild(transform.childCount - 1).gameObject;
        LaunchAttackBox.SetActive(false);
    }


    private bool isMoving = false;

    private bool isCharging = false;
    private bool isCharged = false;

    private float charge_time_start;

    // Update is called once per frame
    void Update()
    {
        if (!statsScript.isDead)
        {
            InputHandle();
        }
        UpdateAnimation();
    }

    private void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && statsScript.isGrounded && !isCharging)
        {
            transform.GetComponentInChildren<SwingWeapon>().Swing();
        }


        Vector2 newdir = Vector2.zero;
        if (statsScript.isGrounded && !statsScript.isRagdoll)
        {
            if (!(swingScript.attacking || isCharging))
                newdir.x += Input.GetAxisRaw("Horizontal") * movespeed;
            rb.velocity = new Vector2(newdir.x, rb.velocity.y);
        }
        if (newdir.x != 0) isMoving = true; else isMoving = false;

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
            statsScript.isGrounded = false;
            if (Time.time - charge_time_start >= chargedelay)
            {
                transform.position = transform.position + (0.2f * Vector3.up);
                Vector2 toward_cursor = ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position)).normalized;
                rb.AddForce(toward_cursor * chargeforce);
                statsScript.ragdoll(0.5f);
                LaunchAttackBox.SetActive(true);
            }
        }

        if (rb.velocity.x != 0 && isMoving) transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        if (isCharging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localScale = new Vector3(Mathf.Sign(mousePos.x - transform.position.x), 1, 1);
        }
    }


    private string currentState = "player_idle";
    private string nextState = "";

    [NonSerialized] public bool isLicking = false;
    private void UpdateAnimation()
    {
        if (statsScript.isDead)
        {
            nextState = "player_dead";
        }
        else
        {
            if (!isMoving && statsScript.isGrounded && !statsScript.isDead)
            {
                int r = UnityEngine.Random.Range(0, 1000);
                if (r != 999 && !isLicking) nextState = "player_idle";
                else
                {
                    nextState = "player_idle2";
                    isLicking = true;
                }
            }
            if (isMoving && statsScript.isGrounded)
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

            if (!statsScript.isGrounded) {
                nextState = "player_launching";
            }

            if (nextState == "player_launching")
            {
                // Calculate the direction from this object to the target
                Vector2 direction = (rb.velocity).normalized;   
                // Calculate the angle in degrees between the current forward direction and the desired direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Create a rotation based on the calculated angle
                if (transform.localScale.x > 0)
                {
                    angle = angle - 135 - 180;
                }
                else
                {
                    angle = angle - 45 - 180;
                }
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // Apply the rotation to this object's transform
                transform.GetChild(0).transform.rotation = rotation;
            }
            else
            {
                transform.GetChild(0).transform.rotation = Quaternion.identity;
            }
        }

        if (nextState == currentState) return;
        currentState = nextState;

        

        animator.Play(currentState);
    }

}
