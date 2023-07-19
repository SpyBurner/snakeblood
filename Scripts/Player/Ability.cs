using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject bombPrefab;

    public float cooldownTime = 1f;
    private float cooldownStart;
    private bool isOnCooldown;
    void Start()
    {
        
    }

    private bool isActive = false;
    private GameObject bomb = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isOnCooldown)
        {
            if (!isActive){
                bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
                isActive = true;
            }
            else
            {
                isActive = false;
                isOnCooldown = true;
                cooldownStart = Time.time;

                bomb.GetComponent<Bomb>().Detonate();
            }
        }

        if (isOnCooldown)
        {
            if (Time.time - cooldownStart >= cooldownTime)
            {
                isOnCooldown = false;
            }
        }
    }
}
