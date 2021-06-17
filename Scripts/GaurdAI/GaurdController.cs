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
    public GameObject player;
    public float botSpeed=1.2f;
    public float botAcceleration=1;
    public float botOnRunSpeed=2;
    public float botOnRunAcceleration = 2;

    void Start()
    {
        Initialization();
        StartCoroutine(InitailizePlayer());
    }

    IEnumerator InitailizePlayer()
    {
        int n=1;
        while (n > 0)
        {
            yield return new WaitForSeconds(1);
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                n = 0;
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
       
    }

    void Initialization()
    {
        isDead = false;
        GameObject g = GameObject.FindGameObjectWithTag("Player");
        if (g != null)
            player = g;
        //timer initialization
        buttlerTargetPosition = gameObject.GetComponent<NavMeshMovementOnClick>().TargetPoint;
        
        PatrolInitialize();
        InitializeGaurdType();
        InitializeWeaponSettings();
    }

    #region GaurdType

    [Header("Gaurd Type")]
    public string[] GaurTypeAll;
    public GameObject[] GaurdFBX;
    public Avatar[] GaurdAvatar;
    public RuntimeAnimatorController[] GaurdAnimatorController;
    public GameObject ParticleEffectForSucideBomber;

    [Header("Gaurd Stats")]
    public float[] GaurdsminDistanceOnAttack;
    public float[] GaurdswalkSpeed;
    public float[] GaurdsRunSpeed;
    public float[] GaurdsFireRate;
    public float[] GaurdsDamage;
    public float[] GaurdsHealth;

    string gaurdType="Patroller";
    RuntimeAnimatorController animcontroller;
    Avatar SelectedAvtar;
    float minDistanceAttack;
    float walkSpeed;
    float runSpeed;
    float gfireRate;
    float gdamage;
    float ghealth;

    void InitializeGaurdType()
    {
        foreach (GameObject g in GaurdFBX)
        {
            g.SetActive(false);
        }

        GaurdRandomness();
        ThingsSetUpForMotion();
        botSpeed = walkSpeed;
        botOnRunSpeed = runSpeed;
        gameObject.GetComponent<TargetHealth>().SetHealth(ghealth);
    }

    void GaurdRandomness()
    {
        
        int randomness = Random.Range(1, 100);
        // if(saveload.currentLevel<5)
        // {
        //     randomness = Random.Range(1, 60);
        // }
        // else if(saveload.currentLevel<10)
        // {
        //     randomness = Random.Range(1, 80);
        // }else
        // {
        //     randomness = Random.Range(1, 100); 
        // }

        if (randomness < 60)
        {
            //spawn normal
            GaurdFBX[0].SetActive(true);
            GaurdModel = GaurdFBX[0];
            animcontroller = GaurdAnimatorController[0];
            SelectedAvtar = GaurdAvatar[0];
            gaurdType = GaurTypeAll[0];
            minDistanceAttack = GaurdsminDistanceOnAttack[0];
            walkSpeed = GaurdswalkSpeed[0];
            runSpeed = GaurdsRunSpeed[0];
            gfireRate = GaurdsFireRate[0];
            gdamage = GaurdsDamage[0];
            ghealth = GaurdsHealth[0];
        }
        else if (randomness < 80)
        {
            //spawn melee
            GaurdFBX[3].SetActive(true);
            GaurdModel = GaurdFBX[3];
            animcontroller = GaurdAnimatorController[3];
            SelectedAvtar = GaurdAvatar[3];
            gaurdType = GaurTypeAll[3];
            minDistanceAttack = GaurdsminDistanceOnAttack[3];
            walkSpeed = GaurdswalkSpeed[3];
            runSpeed = GaurdsRunSpeed[3];
            gfireRate = GaurdsFireRate[3];
            gdamage = GaurdsDamage[3];
            ghealth = GaurdsHealth[3];
            
        }
        else if (randomness < 1)
        {
            //spawn grenadier
            GaurdFBX[2].SetActive(true);
            GaurdModel = GaurdFBX[2];
            animcontroller = GaurdAnimatorController[2];
            SelectedAvtar = GaurdAvatar[2];
            gaurdType = GaurTypeAll[2];
            minDistanceAttack = GaurdsminDistanceOnAttack[2];
            walkSpeed = GaurdswalkSpeed[2];
            runSpeed = GaurdsRunSpeed[2];
            gfireRate = GaurdsFireRate[2];
            gdamage = GaurdsDamage[2];
            ghealth = GaurdsHealth[2];
        }
        else if (randomness < 100)
        {
            //spawn bomber
            GaurdFBX[1].SetActive(true);
            GaurdModel = GaurdFBX[1];
            animcontroller = GaurdAnimatorController[1];
            SelectedAvtar = GaurdAvatar[1];
            gaurdType = GaurTypeAll[1];
            minDistanceAttack = GaurdsminDistanceOnAttack[1];
            walkSpeed = GaurdswalkSpeed[1];
            runSpeed = GaurdsRunSpeed[1];
            gfireRate = GaurdsFireRate[1];
            gdamage = GaurdsDamage[1];
            ghealth = GaurdsHealth[1];
            
        }
    }

    void ThingsSetUpForMotion()
    {
        //setup avtar
        anim.avatar = SelectedAvtar;

        //setup runtime animator controller
        anim.runtimeAnimatorController = animcontroller;
    }

    #endregion

    #region Gaurd Patrolling

    [Header("Patrol")]
    public bool isPatrol;
    public bool isRandomPatrol;
    public bool serialinversePatrol;
    public float minwaitingTimeOnPointPatrol;
    public float maxwaitingTimeOnPointPatrol;
    public Transform AssignedGaurdPoints;

    private int gaurdpatrolindex=0;
    private int totalPatrolLength=0;
    private Vector3 singlePatrolRotation=Vector3.zero;
    private bool firstTimeFace=false;

    void PatrolInitialize()
    {
        CheckAndSetPatrol();
        gameObject.GetComponent<NavMeshMovementOnClick>().SettargetPosition(buttlerTargetPosition);
        totalPatrolLength=AssignedGaurdPoints.transform.childCount;
        firstTimeFace=false;
        if(totalPatrolLength==1)
        {
            foreach (Transform item in AssignedGaurdPoints)
            {
                singlePatrolRotation=item.transform.eulerAngles;
                singlePatrolRotation.x=0;
                singlePatrolRotation.z=0;
            }
        }
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
            
        }
        
        foreach (Transform item in AssignedGaurdPoints)
        {
            if (i == tempIndex)
            {
                gaurdpatrolindex++;
                buttlerTargetPosition=item.transform.position;
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
                buttlerTargetPosition=item.transform.position;
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
            if (Vector3.Distance(gameObject.transform.position, buttlerTargetPosition) > 1.1f)
            {
                //walk
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                //gameObject.GetComponent<NavMeshAgent>().speed = botSpeed;
                //gameObject.GetComponent<NavMeshAgent>().acceleration = botAcceleration;
                //anim.applyRootMotion = false;
                //GaurdModel.transform.localPosition = new Vector3(0, 0.035f, 0);
               // GaurdModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isWalk", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);
                //WalkSound(0.8f);
                if (gaurdType == GaurTypeAll[0])
                {
                    SetAkforWalk();
                }
            }
            else
            {
                //idle
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

                //anim.applyRootMotion = true;
                //ButtlerModel.transform.localPosition = new Vector3(0, 0, 0);
                //ButtlerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
                anim.SetBool("isWalk", false);
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);

                if(firstTimeFace==false && totalPatrolLength==1)
                {
                    gameObject.transform.eulerAngles=singlePatrolRotation;
                   
                }

                if (!setNewTarget)//set new Waypoint
                {
                    setNewTarget = true;
                    StartCoroutine(SetNewWaypoints());
                }
                if (gaurdType == GaurTypeAll[0])
                {
                    SetAkforIdle();
                }
            }
            if (gaurdType == GaurTypeAll[3])
            {
                HideSword();
            }
        }
        else
        {
            if (player != null)
            buttlerTargetPosition=player.transform.position;
            float dis = Vector3.Distance(gameObject.transform.position, buttlerTargetPosition);
            //print(dis);
            if (dis > minDistanceAttack)//run
            {
                firstTimeFace = true;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                //gameObject.GetComponent<NavMeshAgent>().speed = botOnRunSpeed;
                //gameObject.GetComponent<NavMeshAgent>().acceleration = botOnRunAcceleration;

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
                if (gaurdType == GaurTypeAll[0])
                {
                    SetAkforRun();
                }
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
                if (gaurdType == GaurTypeAll[0])
                {
                    CheckAndFire();
                    SetAkforAttack();
                }
                else if (gaurdType == GaurTypeAll[1])
                    CheckandBomb();
                else if (gaurdType == GaurTypeAll[3])
                {
                    //ShowSword();
                    CheckandMelee();
                }
                else
                    CheckAndFire();
            }

            if (gaurdType == GaurTypeAll[3])
            {
                ShowSword();
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
           // CalculateBotsDistanceFromMe();
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
        tempFireRate = gfireRate;
        tempTimeFire = gfireRate;
        int ran = Random.Range(0, DeathAnimationsNames.Length);
        ranDeathAnimForPlayer = DeathAnimationsNames[ran];
        if (gaurdType == GaurTypeAll[3])
        {
            InitializeSword();
        }
    }

    #region Normal Shoot
    //----------normal AI attact shooting

    //set weapon
    public GameObject IdleAK;
    public GameObject WalkAK;
    public GameObject RunAK;
    public GameObject AttackAK;

    void SetAkforIdle()
    {
        IdleAK.SetActive(true);
        WalkAK.SetActive(false);
        RunAK.SetActive(false);
        AttackAK.SetActive(false);
    }
    void SetAkforWalk()
    {
        WalkAK.SetActive(true);
        RunAK.SetActive(false);
        IdleAK.SetActive(false);
        AttackAK.SetActive(false);
    }
    void SetAkforRun()
    {
        RunAK.SetActive(true);
        WalkAK.SetActive(false);
        IdleAK.SetActive(false);
        AttackAK.SetActive(false);
    }
    void SetAkforAttack()
    {
        AttackAK.SetActive(true);
        WalkAK.SetActive(false);
        RunAK.SetActive(false);
        IdleAK.SetActive(false);
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

    //--------end shooting

    #endregion

    #region("Bomber")
    //----------bomber

    void CheckandBomb()
    {
        //actually he is a sucide bomber
        tempTimeFire -= Time.deltaTime;
        if (tempTimeFire < 0)
        {
            tempTimeFire = tempFireRate;
            BombProcess();
        }

    }

    void BombProcess()
    {
        gameObject.transform.LookAt(player.transform);
        StartCoroutine(WaitAndShowEffectsOfBomb());
        
    }

    IEnumerator WaitAndShowEffectsOfBomb()
    {
        yield return new WaitForSeconds(1f);
        float distancefromplayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(distancefromplayer<2)
        player.GetComponent<PlayerStats>().TakeDamage((int)gdamage, gaurdWeapon.forceImpactOnPlayer, ranDeathAnimForPlayer);
        else if (distancefromplayer < 3)
            player.GetComponent<PlayerStats>().TakeDamage((int)(gdamage/2), gaurdWeapon.forceImpactOnPlayer, ranDeathAnimForPlayer);
        else if (distancefromplayer < 5)
            player.GetComponent<PlayerStats>().TakeDamage((int)(gdamage / 4), gaurdWeapon.forceImpactOnPlayer, ranDeathAnimForPlayer); 

        //self destruct
        gameObject.GetComponent<TargetHealth>().TakeDamageMy((ghealth+100));
    }

    //-------end bomber attack

    #endregion

    #region Sword

    public GameObject[] swords;

    void InitializeSword()
    {
        foreach (GameObject g in swords)
        {
            g.GetComponent<WeaponTrigger>().weaponDamage = gdamage;
            g.GetComponent<WeaponTrigger>().ranDeathAnimForPlayer = ranDeathAnimForPlayer;
        }
    }

    void ShowSword()
    {
        foreach (GameObject g in swords)
        {
            g.SetActive(true);
        }
    }

    void HideSword()
    {
        foreach (GameObject g in swords)
        {
            g.SetActive(false);
        }
    }

    void CheckandMelee()
    {
        tempTimeFire -= Time.deltaTime;
        if (tempTimeFire < 0)
        {
            tempTimeFire = tempFireRate;
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        //look at the player
        gameObject.transform.LookAt(player.transform);

    }

    #endregion

    #endregion

    #region Death System

    public string[] DeathAnimationsNames;
    public bool isDead = false;
    public GameObject MyMarker;
    GameObject CurrentParticleEffect;
    public void SetDead()
    {
        isDead = true;
        int random = Random.Range(0, DeathAnimationsNames.Length);
        string currentAnim = DeathAnimationsNames[random];
        anim.Play(currentAnim);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<GaurdController>().enabled = false;
        gameObject.GetComponent<FieldOfView>().enabled = false;
        gameObject.GetComponent<NavMeshMovementOnClick>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.transform.Find("Sphere").gameObject.SetActive(false);
        MyMarker.SetActive(false);
        if(gaurdType=="Bomber")
        CurrentParticleEffect=Instantiate(ParticleEffectForSucideBomber,gameObject.transform.position,gameObject.transform.rotation);
        /*
        GameObject MeshThings = gameObject.transform.Find("enemy 1").gameObject;
        MeshThings.transform.parent = null;
        foreach (Transform t in MeshThings.transform)
        {
            if (t.GetComponent<SkinnedMeshRenderer>())
            {
                t.gameObject.AddComponent<MeshCollider>();
                t.gameObject.AddComponent<Rigidbody>();
            }
        }*/

        StartCoroutine(DisableBot());
    }

    IEnumerator DisableBot()
    {
        yield return new WaitForSeconds(15);
        
        if(gaurdType=="Bomber")
        Destroy(CurrentParticleEffect);
        Destroy(gameObject);
    }

    #endregion

}