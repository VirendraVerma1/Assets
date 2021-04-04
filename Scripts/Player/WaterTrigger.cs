using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameController go = GameObject.FindGameObjectWithTag("MainController").gameObject.GetComponent<GameController>();
            go.UserWantToAbortMission();
        }
    }
}
