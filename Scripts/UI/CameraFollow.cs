using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float yOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        if (target != null)
        {
            transform.position = target.transform.position + Vector3.back * 10 + Vector3.up * yOffset;
        }
    }
}
