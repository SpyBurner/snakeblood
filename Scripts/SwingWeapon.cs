using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject weaponPrefab;
    public float swingtime = 1;
    public float angleStart = 0;
    public float angleEnd = 180;

    private float t;
    private float timeStart;
    private float increment;

    private bool attacking;
    private GameObject weaponObject = null;
    void Start()
    {
        attacking = false;
        increment = MathF.Abs(angleEnd-angleStart)/(60 * swingtime);
    }

    private int dir = 1;
    private void Update()
    {
        if (weaponObject != null)
        {
            weaponObject.transform.Rotate(0, 0, -increment * dir);

            if (Time.time - timeStart >= swingtime)
            {
                Destroy(weaponObject);
                attacking = false;
            }
        }
    }
    public void Swing()
    {
        if (!attacking)
        {
            timeStart = Time.time;
            weaponObject = GameObject.Instantiate(weaponPrefab, transform);
            weaponObject.transform.position = transform.position;
            attacking = true;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y >= transform.position.y) dir = -1;
            else dir = 1;

            weaponObject.transform.localScale = new Vector3(weaponObject.transform.localScale.x, weaponObject.transform.localScale.y * dir, weaponObject.transform.localScale.z);
        }
    }
}
