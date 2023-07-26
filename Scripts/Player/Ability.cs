using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public GameObject bombPrefab;

    public float cooldownTime = 1f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;
    void Start()
    {
    }

    private bool isActive = false;
    private GameObject bomb = null;
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            isOnCooldown = false;
        }

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
                cooldownTimer = cooldownTime;

                bomb.GetComponent<Bomb>().Detonate();
            }
        }

    }
}
