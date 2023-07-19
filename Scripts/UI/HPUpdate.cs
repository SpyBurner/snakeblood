using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class HPUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    private Animator anim;
    private Stats stats;
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        stats = target.GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("hp", stats.hp);
    }

}
