using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // Update is called once per frame
    public float radius = 0.1f;
    public LayerMask whatisground;

    void FixedUpdate()
    {
        Move script = GetComponentInParent<Move>();
        bool grounded = Physics2D.OverlapCircle(transform.position, radius, whatisground);
        if (grounded && !script.isRagdoll)
        {
            script.isGrounded = true;
        }
        else
        {
            script.isGrounded = false;
        }
    }
}
