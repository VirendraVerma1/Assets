using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdDisance : MonoBehaviour
{
    public GameObject Gaurd;
    public float Distance;

    public GaurdDisance(GameObject gaurd,float distance)
    {
        Gaurd=gaurd;
        Distance=distance;
    }
}
