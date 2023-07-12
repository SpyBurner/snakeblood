using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToStopLicking : MonoBehaviour
{
    public void StopLicking()
    {
        GetComponentInParent<Move>().isLicking = false;
    }
}
