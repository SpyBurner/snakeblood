using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject weaponPrefab;
    public float swingtime = 1;
    public float swingAngle = 60;

    private float t;
    private float timeStart;
    private float increment;

    [NonSerialized] public bool attacking;
    private GameObject weaponObject = null;
    void Start()
    {
        attacking = false;
    }

    private int dir = 1;
    private void Update()
    {
        increment = swingAngle / (60 * swingtime);
        if (weaponObject != null)
        {
            weaponObject.transform.Rotate(0, 0, increment * dir);

            if ((Time.time - timeStart) >= swingtime)
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

            float angle = -30;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y >= transform.position.y) dir = 1;
            else dir = -1;
            if (dir == 1) angle -= 180;

            weaponObject.GetComponent<Sword>().cutDir = dir;

            weaponObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
