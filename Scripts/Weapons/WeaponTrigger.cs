using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public float weaponDamage=20;
    public string ranDeathAnimForPlayer;
    bool nextAttack = false;

    void Start()
    {
        nextAttack = false;
    }

    void OnTriggerEnter(Collider col)
    {
        print(col.gameObject.name);
        if (col.tag == "Player" && nextAttack==false)
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage((int)weaponDamage, 10, ranDeathAnimForPlayer);
            nextAttack = true;
            StartCoroutine(NowCanAttack());
        }
    }

    IEnumerator NowCanAttack()
    {
        yield return new WaitForSeconds(1);
        nextAttack = false;
    }
}
