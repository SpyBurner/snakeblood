using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float radius = 0.1f;
    public LayerMask whatisground;

    private Move moveScript;
    private void Start()
    {
        moveScript = transform.parent.GetComponent<Move>();
    }

    void FixedUpdate()
    {
        Stats script = GetComponentInParent<Stats>();
        bool grounded = Physics2D.OverlapCircle(transform.position, radius, whatisground);
        //bool grounded = Physics2D.Raycast(transform.position, Vector2.down, radius, whatisground);
        if (grounded && !script.isRagdoll)
        {
            script.isGrounded = true;
            moveScript.LaunchAttackBox.SetActive(false);
        }
        else
        {
            script.isGrounded = false;
        }
    }
}
