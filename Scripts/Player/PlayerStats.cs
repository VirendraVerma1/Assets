using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int MaxHealth = 100;
    public int Health=100;
    public GameObject DamageBloodUI;
    public GameObject FullDamageBloodUI;
    public Animator anim;
    public GameObject ControlUI;
    Transform StartingPosition;
    public GameObject GameOverPannel;
    public GameObject StatPannel;
    public Image HealthBar;
    public ControlsTutorial ct;
    public GameController gameController;
    void Start()
    {
       
        gameController=GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>();
        StartingPosition = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
        HealthBar.fillAmount = (float)Health / (float)MaxHealth;
        DamageBloodUI.SetActive(false);
        FullDamageBloodUI.SetActive(false);
        StartInitilize();
        OnAimOff();
        ShopPickDownButton();
        StartCoroutine(WaitToLoadAllGaurds());
    }

    #region stealth System

    [Header("Stealth")]
    public GameObject KillFromKnifeButton;
    public float NearestGaurdDistance=1f;
    GameObject nearestGaurd;
    IEnumerator WaitToLoadAllGaurds()
    {
        yield return new WaitForSeconds(10);
        InitializeGaurds();
        StartCoroutine(waitforstealth());
    }

    IEnumerator waitforstealth()
    {
        while(true)
        {
            yield return new WaitForSeconds(1); 
            UpdateGaurdDistance();
            nearestGaurd=MyNearstGaurd();
            if(nearestGaurd!=null)
            {
                KillFromKnifeButton.SetActive(true);
            }
            else
            {
                KillFromKnifeButton.SetActive(false);
            }
        }
    }

    public void OnButtonKillFromMyKnife()
    {
        for(int i=0;i<gaurdDistance.Count;i++)
        {
            if(gaurdDistance[i].Gaurd==nearestGaurd)
            {
                gaurdDistance[i].Gaurd=null;
            }
        }
        nearestGaurd.GetComponent<TargetHealth>().TakeDamageMy(200);
        KillFromKnifeButton.SetActive(false);
        saveload.knifeKilledStat++;
    }


    //-----------------common function
    List<GaurdDisance> gaurdDistance=new List<GaurdDisance>();
    GameObject[] Gaurds;
    void InitializeGaurds()
    {
        Gaurds=GameObject.FindGameObjectsWithTag("Gaurd");
        for(int i=0;i<Gaurds.Length;i++)
        {
            gaurdDistance.Add(new GaurdDisance(Gaurds[i],99999));
        }
    }

    void UpdateGaurdDistance()
    {
        for(int i=0;i<gaurdDistance.Count;i++)
        {
            if(gaurdDistance[i].Gaurd!=null)
            gaurdDistance[i].Distance=Vector3.Distance(gameObject.transform.position,gaurdDistance[i].Gaurd.transform.position);
        }
    }

    GameObject MyNearstGaurd()
    {
        float min=999999;
        GameObject g=null;
        for(int i=0;i<gaurdDistance.Count;i++)
        {
            if(gaurdDistance[i].Gaurd!=null)
            {
                if(min>gaurdDistance[i].Distance)
                {
                    min=gaurdDistance[i].Distance;
                    g=gaurdDistance[i].Gaurd;
                }
            }
        }
        if(min<=NearestGaurdDistance)
        {
            return g;
        }
        else
        {
            return null;
        }
        
    }

    #endregion
   
    public void TakeDamage(int dam,float impact, string ranDeathAnim)
    {
        if(gameController.isShieldActivateforclick)
        {
            Health -= dam;
            HealthBar.fillAmount = (float)Health / (float)MaxHealth;
            var rb = GetComponent<Rigidbody>();
            rb.AddForce(0, 0, impact, ForceMode.Impulse);
            StartCoroutine(DamageImpact());
            if (Health < 0)
            {
                //Dead
                FullDamageBloodUI.SetActive(true);
                //gameObject.GetComponent<CapsuleCollider>().enabled = false;
                DisableAllComponents();
                anim.enabled = true;
                ControlUI.SetActive(false);
                gameObject.tag = "Untagged";
                anim.Play(ranDeathAnim);
                Cursor.visible = true;
                ct.isUIOn = true;
                GameOverPannel.SetActive(true);
                gameController.SetCalculation();
                if(saveload.adsFrequency<0)
                {
                    FindObjectOfType<AdScript>().ShowIntertesialAdsSwitch();
                    saveload.adsFrequency=3;
                }
                else
                {
                    saveload.adsFrequency--;   
                }
                saveload.Save();
            }
        }  
    }

    IEnumerator DamageImpact()
    {
       
        DamageBloodUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        
        DamageBloodUI.SetActive(false);
    }

    public void OnContinueButtonPressed()
    {
        FindObjectOfType<AdScript>().ShowRewardVideoAdsSwitch();
        saveload.adsFrequency +=3;
        saveload.Save();
        StartInitilize();
    }

     void StartInitilize()
    {
        InitializePlayerFromStarting();
        FullDamageBloodUI.SetActive(false);
        //gameObject.GetComponent<CapsuleCollider>().enabled = false;
        EnableAllComponents();
        StatPannel.SetActive(false);
        GameOverPannel.SetActive(false);
        anim.enabled = false;
        ControlUI.SetActive(true);
        gameObject.tag = "Player";
        Health = MaxHealth;
        HealthBar.fillAmount = (float)Health / (float)MaxHealth;
    }

    void DisableAllComponents()
    {
        gameObject.GetComponent<BasicBehaviour>().enabled = false;
        gameObject.GetComponent<Footsteps>().enabled = false;
        gameObject.GetComponent<CoverBehaviour>().enabled = false;
        gameObject.GetComponent<ShootBehaviour>().enabled = false;
        gameObject.GetComponent<MoveBehaviour>().enabled = false;
        gameObject.GetComponent<AimBehaviour>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
    }

    void EnableAllComponents()
    {
        gameObject.GetComponent<BasicBehaviour>().enabled = true;
        gameObject.GetComponent<Footsteps>().enabled = true;
        gameObject.GetComponent<CoverBehaviour>().enabled = true;
        gameObject.GetComponent<ShootBehaviour>().enabled = true;
        gameObject.GetComponent<MoveBehaviour>().enabled = true;
        gameObject.GetComponent<AimBehaviour>().enabled = true;
        gameObject.GetComponent<Animator>().enabled = true;
    }

    void InitializePlayerFromStarting()
    {
        gameObject.transform.position = StartingPosition.transform.position;
        gameObject.transform.rotation = StartingPosition.transform.rotation;
    }

    public void OnRestartButtonPressed()
    {
        saveload.isrestartMission = true;
        saveload.Save();
        GameObject.FindGameObjectWithTag("MainController").GetComponent<GameController>().StartLoadingScreen("HuntGame");
    }

    #region CustomiZe android UI

    [Header("Customize android UI")]
    public GameObject PickUpButton;
    public GameObject ReloadButton;
    public GameObject FireButton;
    public GameObject SholderAimButton;

    public void ShopPickUpButton()
    {
        PickUpButton.SetActive(true);
    }

    public void ShopPickDownButton()
    {
        PickUpButton.SetActive(false);
    }

    
    public void OnAimHappen()
    {
        //ReloadButton.SetActive(true);
        //FireButton.SetActive(true);
        //SholderAimButton.SetActive(true);
    }

    public void OnAimOff()
    {
        //ReloadButton.SetActive(false);
        //FireButton.SetActive(false);
        //SholderAimButton.SetActive(false);
    }

    #endregion


}
