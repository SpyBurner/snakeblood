using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    public float radius = 0.1f;
    public LayerMask whatisground;

    void FixedUpdate()
    {
        Stats script = GetComponentInParent<Stats>();
        bool grounded = Physics2D.OverlapCircle(transform.position, radius, whatisground);
        //bool grounded = Physics2D.Raycast(transform.position, Vector2.down, radius, whatisground);
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
