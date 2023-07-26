using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OnHitEffect : MonoBehaviour
{
    protected Stats statsScript;
    protected Rigidbody2D rb;
    private void Start()
    {
        statsScript = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void TakeEffect(ref int damage, ref Vector2 knockbackForce)
    {
    }
}
