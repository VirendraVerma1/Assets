using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdWeapon : MonoBehaviour
{
    public Transform BulletHolder;
    public GameObject Bullet;
    public float forceImpactOnPlayer=10;
    public GameObject ParticleEffectOnPlayer;
    public GameObject FiringEffect;
    public float bulletDamage=1;
    public float fireRate=0.5f;

}
