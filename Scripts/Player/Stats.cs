using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhp = 4;
    [NonSerialized]
    public int hp;
    public float movespeed = 3.0f;
    public int damage = 1;


    [NonSerialized]
    public bool isDead = false;
    void Start()
    {
        hp = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HPchange(int diff)
    {
        hp += diff;
        if (hp <= 0)
        {
            isDead = true;
        }
    }
}
