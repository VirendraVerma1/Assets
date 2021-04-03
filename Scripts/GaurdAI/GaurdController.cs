using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GaurdController : MonoBehaviour
{
    //public Transform[] GaurdWayPoints;
    public Animator anim;
    Vector3 buttlerTargetPosition;
    public GameObject GaurdModel;
    GameObject player;
    public float botSpeed=1.2f;
    public float botAcceleration=1;
    public float botOnRunSpeed=2;
    public float botOnRunAcceleration = 2;

    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //timer initialization
        buttlerTargetPosition = gameObject.GetComponent<NavMeshMovementOnClick>().TargetPoint;
        
        PatrolInitialize();
        InitializeWeaponSettings();
    }

    #region Gaurd Patrolling

    [Header("Patrol")]
    public bool isPatrol;
    public bool isRandomPatrol;
    public bool serialinversePatrol;
    public float minwaitingTimeOnPointPatrol;
    public float maxwaitingTimeOnPointPatrol;
    public Transform AssignedGaurdPoints;

    private int gaurdpatrolindex=0;

    void PatrolInitialize()
    {
        CheckAndSetPatrol();
        gameObject.GetComponent<NavMeshMovementOnClick>().SettargetPosition(buttlerTargetPosition);
    }

    void CheckAndSetPatrol()
    {
        if(isPatrol&&AssignedGaurdPoints!=null)
        {
            if(!isRandomPatrol)
            {
                GetAndSetPatrolIndexSerial();
            }
            else
            {
                GetAndSetPatrolIndexRandom();
            }
        }
    }

    void GetAndSetPatrolIndexSerial()
    {
        int length=AssignedGaurdPoints.transform.childCount;
        if(gaurdpatrolindex>=(length))
        {
            gaurdpatrolindex=0;
        }
        int i=0;
        int tempIndex = gaurdpatrolindex;
        if (serialinversePatrol)
        {
            tempIndex = length - gaurdpatrolindex-1;
            print(tempIndex);
        }
        
        foreach (Transform item in AssignedGaurdPoints)
        {
            if (i == tempIndex)
            {
                gaurdpatrolindex++;
                buttlerTargetPosition=item.position;
                break;
            }
            i++;
        }
    }

    void GetAndSetPatrolIndexRandom()
    {
        int length=AssignedGaurdPoints.transform.childCount;
        int random=Random.Range(0,length);
        int i=0;
        foreach (Transform item in AssignedGaurdPoints)
        {
            if(i==random)
            {
                gaurdpatrolindex++;
                buttlerTargetPosition=item.position;
                break;
            }
            i++;
        }
    }

    #endregion

    bool setNewTarget = false;
    void Update()
    {
        gameObject.transform.position = GaurdModel.transform.position;
        gameObject.transform.rotation = GaurdModel.transform.rotation;
        CheckButtlerTarget();
    }

    void CheckButtlerTarget()
    {
        //print(Vector3.Distance(gameObject.transform.position, buttlerTargetPosition) + "|" + gameObject.GetComponent<FieldOfView>().isPlayer);
        if (gameObject.GetComponent<FieldOfView>().isPlayer == false || player.tag == "Untagged")
        {
            if (Vector3.Distance(gameObject.transform.position, buttlerTargetPosition) > 0.7f)
            {
                //walk
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                gameObject.GetComponent<NavMeshAgent>().speed = botSpeed;
                gameObject.GetComponent<NavMeshAgent>().acceleration = botAcceleration;
                //anim.applyRootMotion = false;
                //GaurdModel.transform.localPosition = new Vector3(0, 0.035f, 0);
               // GaurdModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isWalk", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);
                //WalkSound(0.8f);
            }
            else
            {
                //idle
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                //anim.applyRootMotion = true;
                //ButtlerModel.transform.localPosition = new Vector3(0, 0, 0);
                //ButtlerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);

                if (!setNewTarget)//set new Waypoint
                {
                    setNewTarget = true;
                    StartCoroutine(SetNewWaypoints());
                }
            }
        }
        else
        {
            float dis = Vector3.Distance(gameObject.transform.position, buttlerTargetPosition);
            //print(dis);
            if (dis > 7f)//run
            {

                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                gameObject.GetComponent<NavMeshAgent>().speed = botOnRunSpeed;
                gameObject.GetComponent<NavMeshAgent>().acceleration = botOnRunAcceleration;
                //WalkSound(0.2f);
                gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                //gameObject.transform.LookAt(player.transform.position);
                //anim.applyRootMotion = false;
                //GaurdModel.transform.localPosition = new Vector3(0, 0.035f, 0);
                //GaurdModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isRunning", true);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", false);

                Vector3 direaction = player.transform.position - this.transform.position;
                direaction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direaction), 0.1f);
                //ran
            }
            else //attack
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                Vector3 direaction = player.transform.position - this.transform.position;
                direaction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direaction), 0.1f);


                //anim.applyRootMotion = true;
                //GaurdModel.transform.localPosition = new Vector3(0, 0.035f, 0);
                //GaurdModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", true);
                CheckAndFire();
            }
        }
    }

   // public Transform TargetPosition;
    IEnumerator SetNewWaypoints()
    {
        float randomTimeWait = Random.Range(minwaitingTimeOnPointPatrol, maxwaitingTimeOnPointPatrol);
        yield return new WaitForSeconds(randomTimeWait);
        CheckAndSetPatrol();
        //TargetPosition.position = buttlerTargetPosition;
        gameObject.GetComponent<NavMeshMovementOnClick>().SettargetPosition(buttlerTargetPosition);
        setNewTarget = false;
    }

    #region FollowPlayer

    public void SetPlayerLocation(Vector3 positionPlayer)
    {
        if (player.tag == "Player")
        {
            buttlerTargetPosition = positionPlayer;
            gameObject.GetComponent<NavMeshMovementOnClick>().SettargetPosition(buttlerTargetPosition);
            CalculateBotsDistanceFromMe();
        }
    }

    public void SetRandomWaypoint()
    {
        if (!setNewTarget)//set new Waypoint
        {
            setNewTarget = true;
            StartCoroutine(SetNewWaypoints());
        }
    }

    float[] gaurdDistanceFromMe;
    GameObject[] gaurdObjects;
    void CalculateBotsDistanceFromMe()
    {
        gaurdObjects = GameObject.FindGameObjectsWithTag("Gaurd");

        //initialize GreaterValueTODistance;
        gaurdDistanceFromMe = new float[gaurdObjects.Length];
        for (int i = 0; i < gaurdObjects.Length; i++)
        {
            gaurdDistanceFromMe[i] = 9999;
        }

        //updating all the distance
        for (int i = 0; i < gaurdObjects.Length; i++)
        {
            if (gaurdObjects[i] != null&&gaurdObjects[i]!=gameObject)
            {
                gaurdDistanceFromMe[i] = Vector3.Distance(gameObject.transform.position, gaurdObjects[i].transform.position);
            }
        }

        //sorting all the distance
        for (int i = 0; i < gaurdObjects.Length; i++)
        {
            for (int j = 1; j < gaurdObjects.Length-1; j++)
            {
                if (gaurdDistanceFromMe[j] > gaurdDistanceFromMe[j + 1])
                {
                    float tempdis = gaurdDistanceFromMe[j];
                    gaurdDistanceFromMe[j] = gaurdDistanceFromMe[j + 1];
                    gaurdDistanceFromMe[j + 1] = tempdis;

                    GameObject tempgo = gaurdObjects[j];
                    gaurdObjects[j] = gaurdObjects[j + 1];
                    gaurdObjects[j + 1] = tempgo;
                }
            }
        }

        //rotate to the player
        for (int i = 0; i < (gaurdObjects.Length/3); i++)
        {
            //gaurdObjects[i].GetComponent<GaurdController>().buttlerTargetPosition = player.transform.position;
            gaurdObjects[i].transform.LookAt(player.transform);
        }
        
    }

    #endregion

    #region Firing Mechanism

    [Header("Firing")]
    GaurdWeapon gaurdWeapon;
    public GameObject Weapon;
    public float tempFireRate = 0.5f;
    public float tempTimeFire = 0.5f;
    string ranDeathAnimForPlayer;

    void InitializeWeaponSettings()
    {
        gaurdWeapon = Weapon.GetComponent<GaurdWeapon>();
        tempFireRate = gaurdWeapon.fireRate;
        int ran = Random.Range(0, DeathAnimationsNames.Length);
        ranDeathAnimForPlayer = DeathAnimationsNames[ran];
    }

    void CheckAndFire()// called from CheckButtlerTarget Attack
    {
        tempTimeFire -= Time.deltaTime;
        if (tempTimeFire < 0)
        {
            tempTimeFire = tempFireRate;
            FireBullet();
        }
    }

    void FireBullet()
    {

        //Raycast from the bullet holder
        RaycastHit hit;
        gaurdWeapon.BulletHolder.transform.LookAt(player.transform);
        Vector3 fwd = gaurdWeapon.BulletHolder.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(gaurdWeapon.BulletHolder.transform.position, fwd, out hit, 200.0f))
        {
            Debug.DrawLine(gaurdWeapon.BulletHolder.transform.position, hit.point);
            GameObject go = Instantiate(gaurdWeapon.FiringEffect, gaurdWeapon.BulletHolder.transform.position, gaurdWeapon.BulletHolder.transform.rotation);
            Destroy(go, 0.1f);
            if (hit.collider.gameObject.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(gaurdWeapon.bulletDamage, gaurdWeapon.forceImpactOnPlayer, ranDeathAnimForPlayer);
            }
        }

        //GameObject tempBullet = Instantiate(gaurdWeapon.Bullet, gaurdWeapon.BulletHolder.transform.position, gaurdWeapon.BulletHolder.transform.rotation);
        //tempBullet.transform.LookAt(player.transform);
    }

    #endregion


    public string[] DeathAnimationsNames;
    public void SetDead()
    {
        int random = Random.Range(0, DeathAnimationsNames.Length);
        string currentAnim = DeathAnimationsNames[random];
        anim.Play(currentAnim);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<GaurdController>().enabled = false;
        gameObject.GetComponent<FieldOfView>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.transform.Find("Sphere").gameObject.SetActive(false);
        StartCoroutine(DisableBot());
    }

    IEnumerator DisableBot()
    {
        yield return new WaitForSeconds(200);
        Destroy(gameObject);
    }


}
