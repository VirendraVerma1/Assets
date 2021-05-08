using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public float weaponDamage=20;
    public string ranDeathAnimForPlayer;

    void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage((int)weaponDamage, 10, ranDeathAnimForPlayer);
        }
    }
}
