using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("CommonThings")]
    bool isTimeFreeze = false;
    public GameObject GaurdAI;
    public GameObject[] MissionTerrain;
    public GameObject ControllerCanvas;
    ShopController shopController;
    public Joystick joystick;

    int terrainLoadNu;

    void Awake()
    {
        saveload.Load();
        shopController = gameObject.GetComponent<ShopController>();
        SetMission();
        Initialize();
    }

    void Start()
    {
        
        InitializeMainMenu();
        ResetAllWonUI();
        isTimeFreeze = false;
        InitializeRadarIcon();
        InitializeFreezeIcon();
    }

    #region Radar Ability
    [Header("Radar Ability")]
    public GameObject RadarSystemPannel;
    public GameObject RadarButton;
    public Sprite NormalRadarSprite;
    public Sprite WorkingRadarSprite;
    public Image FillRateForRadar;
    bool isRadarActiveForClick = false;
    //system
    //when tap on radar then change the radar icon to working and after certain time startrefilling it

    void InitializeRadarIcon()
    {
        RadarButton.GetComponent<Image>().sprite = NormalRadarSprite;
        FillRateForRadar.fillAmount = 0;
        isRadarActiveForClick = true;
    }

    public void OnRadarButtonPressed()
    {
        if (isRadarActiveForClick)
        {
            isRadarActiveForClick = false;
            RadarButton.GetComponent<Image>().sprite = WorkingRadarSprite;
            RadarSystemPannel.SetActive(true);
            StartCoroutine(RadarDisable());
        }
    }

    IEnumerator RadarDisable()
    {
        yield return new WaitForSeconds(saveload.radarWorkingTime);
        RadarSystemPannel.SetActive(false);
        RadarButton.GetComponent<Image>().sprite = NormalRadarSprite;
        FillRateForRadar.fillAmount = 1;
        int time=saveload.radarCooldownTime;
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            FillRateForRadar.fillAmount = (float)time / (float)saveload.radarCooldownTime;
        }
        isRadarActiveForClick = true;
    }
    

    #endregion

    #region Freeze Ability

    [Header("Freeze Ability")]
    public GameObject FreezeTimeButton;
    public Sprite NormalFreezeSprite;
    public Sprite WorkingFreezeSprite;
    public Image FillRateForFrezze;

    bool isFreezeActiveForClick = false;
    //system
    //when tap on radar then change the radar icon to working and after certain time startrefilling it

    void InitializeFreezeIcon()
    {
        FreezeTimeButton.GetComponent<Image>().sprite = NormalFreezeSprite;
        FillRateForFrezze.fillAmount = 0;
        isFreezeActiveForClick = true;
    }


    public void OnAndroidTimeFrezeButtonPressed()
    {
        if (isFreezeActiveForClick)
        {
            isFreezeActiveForClick = false;
            FreezeTimeButton.GetComponent<Image>().sprite = WorkingFreezeSprite;
            isTimeFreeze = true;
            
            StartCoroutine(ShowFreezeTimeButton());
            FreezeEveryThing();
        }
        
            //isTimeFreeze = false;
            //UnFreezeEveryThing();
        
    }

    IEnumerator ShowFreezeTimeButton()
    {
        
        yield return new WaitForSeconds(saveload.freezeWorkingTime);
        RadarButton.GetComponent<Image>().sprite = NormalRadarSprite;
        UnFreezeEveryThing();
        FillRateForFrezze.fillAmount = 1;
        int time = saveload.freezeCooldownTime;
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            FillRateForFrezze.fillAmount = (float)time / (float)saveload.freezeCooldownTime;
        }
        isFreezeActiveForClick = true;
    }

    void FreezeEveryThing()
    {
        GameObject[] Allgaurds = GameObject.FindGameObjectsWithTag("Gaurd");
        foreach (GameObject g in Allgaurds)
        {
            g.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            g.GetComponent<NavMeshAgent>().speed = 0.005f;
            g.GetComponent<NavMeshAgent>().angularSpeed = 0.005f;
            g.GetComponent<Animator>().speed = 0.01f;
            g.GetComponent<GaurdController>().tempFireRate = 9999;
          //  g.GetComponent<GaurdController>().enabled = false;
        }
    }

    void UnFreezeEveryThing()
    {
        GameObject[] Allgaurds = GameObject.FindGameObjectsWithTag("Gaurd");
        foreach (GameObject g in Allgaurds)
        {
            g.GetComponent<NavMeshAgent>().speed = 0.5f;
            g.GetComponent<NavMeshAgent>().angularSpeed = 120f;
            g.GetComponent<Animator>().speed = 1;
            g.GetComponent<GaurdController>().tempFireRate = 0.7f;
           // g.GetComponent<GaurdController>().enabled = true;
        }
    }

    #endregion

    #region Settings

    [Header("Settings")]
    public GameObject SettingPannel;
    public Slider SensitiveSlider;
    public Slider AimSensitiveSlider;

    public void OnSettingButtonPressed()
    {
        SettingPannel.SetActive(true);
    }

    public void OnSliderValueChanged()
    {
        float sens = SensitiveSlider.GetComponent<Slider>().value;
        saveload.senstivity = sens;
    }

    public void OnAimSliderValueChanged()
    {
        float sens = AimSensitiveSlider.GetComponent<Slider>().value;
        saveload.aimSenstivity = sens;
    }

    public void OnCloseSettingsButtonPressed()
    {
        SettingPannel.SetActive(false);
        saveload.Save();
    }

    #endregion

    #region LoadLevel/Bot Count / Won Mechanism

    
    GameObject CurrentMissionTerrain;

    void SetMission()
    {
        print(saveload.currentLevel + "|" + MissionTerrain.Length);
        int missionTerrainLength = MissionTerrain.Length;
        if (missionTerrainLength < saveload.currentLevel)
        {
            //load random level
            int randomLevel = Random.Range(0, missionTerrainLength);
            MissionTerrain[randomLevel].SetActive(true);
            CurrentMissionTerrain = MissionTerrain[randomLevel];
            terrainLoadNu = randomLevel;
        }
        else
        {
            MissionTerrain[saveload.currentLevel - 1].SetActive(true);
            CurrentMissionTerrain = MissionTerrain[saveload.currentLevel - 1];
            terrainLoadNu = saveload.currentLevel - 1;
        }
        StartCoroutine(InitializeBotThings());
    }

    bool isGameOver = false;
    public GameObject[] AllBots;
    int botCount;
    int maxBotCount;

    IEnumerator InitializeBotThings()
    {
        yield return new WaitForSeconds(10);
        isGameOver = false;
        AllBots=GameObject.FindGameObjectsWithTag("Gaurd");
        maxBotCount = AllBots.Length;
        botCount = maxBotCount;
        StartCoroutine(CountForBots());
    }

    IEnumerator CountForBots()
    {
        yield return new WaitForSeconds(2);
        int counter = 0;
        foreach (GameObject g in AllBots)
        {
            if (g != null)
            {
                if (!g.GetComponent<GaurdController>().isDead)
                counter++;

            }
        }
        botCount = counter;
        print(counter);
        UpdateBotKilledUI();
        if (counter > 0)
        {
            StartCoroutine(CountForBots());
        }
        else
        {
            isGameOver = true;
            StartCoroutine( WonThingsHappen());
        }
    }

    [Header("bot Killed UI")]
    public Text BotStatus;
    void UpdateBotKilledUI()
    {
        BotStatus.text = "Bots:"+botCount.ToString() + "/" + maxBotCount.ToString();
    }

    [Header("Won")]
    public GameObject WonPannel;
    public GameObject MissionTextGO;
    public GameObject CompleteTextGO;
    public GameObject RestartMissionButtonGO;
    public GameObject NextMissionButtonGO;
    public GameObject CashEarnPannelGO;
    public Text EarnedCashText;
    public GameObject DoubleYourCashRewardAdButton;

    IEnumerator WonThingsHappen()
    {
        WonPannel.SetActive(true);
        ControllerCanvas.SetActive(false);
        MissionTextGO.SetActive(true);
        yield return new WaitForSeconds(1);
        CompleteTextGO.SetActive(true);
        yield return new WaitForSeconds(2);
        RestartMissionButtonGO.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        NextMissionButtonGO.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CashEarnPannelGO.SetActive(true);
        EarnedCashText.text = "+" + (maxBotCount * 100).ToString();
        saveload.money += (maxBotCount * 100);
        saveload.Save();
        yield return new WaitForSeconds(0.5f);
        //cheack before show this button
        DoubleYourCashRewardAdButton.SetActive(true);
    }

    void ResetAllWonUI()
    {
        MissionTextGO.SetActive(false);
        CompleteTextGO.SetActive(false);
        WonPannel.SetActive(false);
        RestartMissionButtonGO.SetActive(false);
        NextMissionButtonGO.SetActive(false);
        CashEarnPannelGO.SetActive(false);
        DoubleYourCashRewardAdButton.SetActive(false);
    }

    public void OnRestartMissionButtonPressed()
    {
        SceneManager.LoadScene("HuntGame");
    }

    public void OnNextMissionButtonPressed()
    {
        saveload.currentLevel++;
        saveload.Save();
        SceneManager.LoadScene("HuntGame");
    }

    public void OnDoubleCashButtonPressed()
    {
        //Show Ads
        saveload.money += (maxBotCount * 100);
        saveload.Save();
    }

    #endregion

    #region SetUpPlayerAnd Things

    [Header("SetUpthings")]
    public GameObject[] StartingPointsOnTerrain;
    public GameObject CaracterCompleteController;
    public GameObject CenematicCamera;
    public string[] animationNames;
    private Animator animCamera;
    string cameraanimationName = "";
    public GameObject StartMissionButton;
    public ControlsTutorial ct;

    void Initialize()//this dosent means game has started
    {
        CaracterCompleteController.SetActive(false);
        CenematicCamera.SetActive(true);
        foreach (GameObject g in StartingPointsOnTerrain)
        {
            g.SetActive(false);
        }
        ControllerCanvas.SetActive(false);
        animCamera = CenematicCamera.GetComponent<Animator>();
        
        cameraanimationName = animationNames[terrainLoadNu];
        animCamera.Play(cameraanimationName);
        animCamera.speed = 0.03f;
    }

    public void OnStartMissionButtonPressed()
    {
        
        CenematicCamera.SetActive(false);
        
        foreach (GameObject g in StartingPointsOnTerrain)
        {
            g.SetActive(true);
        }
        ControllerCanvas.SetActive(true);
        CaracterCompleteController.SetActive(true);
        StartMissionButton.SetActive(false);
        ct.isUIOn = false;
        MainMenuPannel.SetActive(false);
        BotCountGO.SetActive(true);

        StartCoroutine( UpdatePlayerToAllGaurds());
        ActiveOnlyBuyedWeapon();
        SetWeaponsStat();
        UpdateAndInitialzeWeapon();
             
        
    }

    IEnumerator UpdatePlayerToAllGaurds()
    {
        yield return new WaitForSeconds(10);
        GameObject PlayerObject=GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject g in AllBots)
        {
            g.GetComponent<GaurdController>().player = PlayerObject;
        }
    }
    

    #region Initializing Weapon Things

    void SetWeaponsStat()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapons");
        int counter = 0;
        
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i]!=null)
            {
                if (weapons[i].GetComponent<InteractiveWeapon>())
                {
                    for (int j = 0; j < shopController.WeaponsGO.Length; j++)
                    {
                        if (weapons[i].GetComponent<InteractiveWeapon>().label == shopController.WeaponsGO[j].GetComponent<InteractiveWeapon>().label)
                        {
                            weapons[i].GetComponent<InteractiveWeapon>().SetBulletNu(GetAmmoNu(j));
                        }
                    }
                }
            }
        }
    }

    void UpdateAndInitialzeWeapon()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapons");
        foreach (GameObject g in weapons)
        {
            if(g.GetComponent<InteractiveWeapon>())
            g.GetComponent<InteractiveWeapon>().InitializeWeapon();
        }
    }

    int GetAmmoNu(int nu)
    {
        if (nu == 2)
        {
            return saveload.akAmmo;
        }
        else if (nu == 1)
        {
            return saveload.rifleAmmo;
        }
        else if (nu == 4)
        {
            return saveload.sniperAmmo;
        }
        else if (nu == 3)
        {
            return saveload.shotgunAmmo;
        }
        else if (nu == 0)
        {
            return saveload.pistolAmmo;
        }
        else
        {
            return 0;
        }
    }

    bool GetWeaponBuyedOrNot(int nu)
    {
        if (nu == 2)
        {
            return saveload.isakBuyed;
        }
        else if (nu == 1)
        {
            return saveload.isrifleBuyed;
        }
        else if (nu == 4)
        {
            return saveload.issniperBuyed;
        }
        else if (nu == 3)
        {
            return saveload.isshotgunBuyed;
        }
        else if (nu == 0)
        {
            return saveload.ispistolBuyed;
        }
        else
        {
            return false;
        }
    }

    void ActiveOnlyBuyedWeapon()
    {
        GameObject[] MyBuyiedWeapons = GameObject.FindGameObjectsWithTag("Weapons");
        for (int i = 0; i < MyBuyiedWeapons.Length; i++)
        {
            if (MyBuyiedWeapons[i] != null)
            {
                if (MyBuyiedWeapons[i].GetComponent<InteractiveWeapon>())
                {
                    for (int j = 0; j < shopController.WeaponsGO.Length; j++)
                    {
                        if (MyBuyiedWeapons[i].GetComponent<InteractiveWeapon>().label == shopController.WeaponsGO[j].GetComponent<InteractiveWeapon>().label)
                        {
                            if (GetWeaponBuyedOrNot(j) == false)
                                MyBuyiedWeapons[i].SetActive(false);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #endregion

    #region MainMenu

    [Header("Main Menu")]
    public GameObject MainMenuPannel;
    public GameObject BotCountGO;
    public Text LevelText;
    public Text PlayerNameText;
    public Text MoneyText;

    void InitializeMainMenu()
    {
        MainMenuPannel.SetActive(true);
        BotCountGO.SetActive(false);
        LevelText.text = saveload.currentLevel.ToString();
        PlayerNameText.text = saveload.playerName;
        MoneyText.text = saveload.money.ToString();
    }

    public void OnShopButtonPressed()
    {
        gameObject.GetComponent<ShopController>().OnShopButtonPressed();
    }

    #endregion

    #region AbortMission

    [Header("Abort Mission")]
    public GameObject AbortMissionPannel;
    GameObject player;
    public void UserWantToAbortMission()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ControllerCanvas.SetActive(false);
        AbortMissionPannel.SetActive(true);
        joystick.ResetInput();
        
    }

    public void OnYesButtonPressed()
    {
        SceneManager.LoadScene("HuntGame");
        
    }

    public void OnNoButtonPressed()
    {
        //respawn player at starting
        GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        ControllerCanvas.SetActive(true);
        AbortMissionPannel.SetActive(false);
       player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = playerSpawnPoint.transform.rotation;
    }

    //restart button is on player stat 

    #endregion
}
