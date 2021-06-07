using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Common")]
    public GameObject ShopPannel;
    public GameObject WeaponCard;
    public int switchPannel=0;//0 means gun menu 1 means skills

    public Text MoneyText;

    public Text ResultText;

    void Start()
    {
        GunInitializeFromPannel();
    }

    public void OnShopButtonPressed()
    {
        saveload.money=10000;
        InitializeResult();
        ShopPannel.SetActive(true);
        MoneyText.text = saveload.money.ToString();
        
        
        if (switchPannel == 0)
            GunsSectionButtonPressed();
        else
            SkillsSectionButtonPressed();

        
        
    }

    public void OnShopCloseButtonPressed()
    {
        ShopPannel.SetActive(false);
    }

    [Header("ShopPannel")]
    public GameObject GunsPannel;
    public GameObject SkillPannel;

    public GameObject GunButton;
    public GameObject SkillBbutton;

    public void GunsSectionButtonPressed()
    {
        GunButton.GetComponent<Image>().color = Color.white;
        SkillBbutton.GetComponent<Image>().color = Color.grey;
        GunsPannel.SetActive(true);
        SkillPannel.SetActive(false);
        switchPannel = 0;
        GameObject[] shopContents = GameObject.FindGameObjectsWithTag("ShopContent");
        foreach (GameObject g in shopContents)
        {
            Destroy(g);
        }
        InitializeGuns();
    }

    public void SkillsSectionButtonPressed()
    {
        GunButton.GetComponent<Image>().color = Color.grey;
        SkillBbutton.GetComponent<Image>().color = Color.white;
        GunsPannel.SetActive(false);
        SkillPannel.SetActive(true);
        switchPannel = 1;
        GameObject[] shopContents = GameObject.FindGameObjectsWithTag("ShopContent");
        foreach (GameObject g in shopContents)
        {
            Destroy(g);
        }
        InitializeSkills();
    }

    #region Guns

    [Header("Guns")]
    public Transform ShopPannelWeaponContainer;
    public GameObject[] WeaponsGO;

    void InitializeGuns()
    {
        
        for (int i = 0; i < WeaponsGO.Length; i++)
        {
            GameObject go = Instantiate(WeaponCard);
            go.transform.SetParent(ShopPannelWeaponContainer);
            go.transform.localScale = Vector3.one;

            InteractiveWeapon tempWeaponScript = WeaponsGO[i].GetComponent<InteractiveWeapon>();
            go.transform.Find("WeaponStats").transform.Find("WeaponName").GetComponent<Text>().text = tempWeaponScript.label;
            go.transform.Find("WeaponIcon").GetComponent<Image>().sprite = tempWeaponScript.sprite;
            if (GetWeaponBuyedOrNot(i))
                go.transform.Find("WeaponStats").transform.Find("WeaponAmmo").GetComponent<Text>().text = "Ammo:" + GetAmmoNu(i).ToString();

            if (GetWeaponBuyedOrNot(i))
                go.transform.Find("WeaponStats").transform.Find("WeaponCost").GetComponent<Text>().text = GetAmmoBuyPrice(i).ToString();
            else
                go.transform.Find("WeaponStats").transform.Find("WeaponCost").GetComponent<Text>().text = GetWeaponBuyPrice(i).ToString();

            int n = i;
            if (GetWeaponBuyedOrNot(i))
            {
                go.transform.Find("BuyAmmoButton").gameObject.SetActive(true);
                go.transform.Find("BuyGunButton").gameObject.SetActive(false);
                go.transform.Find("EquipGunButton").gameObject.SetActive(true);
                go.transform.Find("BuyAmmoButton").GetComponent<Button>().onClick.AddListener(() => OnBuyAmmoButtonPressed(n));
                go.transform.Find("EquipGunButton").Find("Text").GetComponent<Text>().text = "Equip";
                go.transform.Find("EquipGunButton").GetComponent<Image>().color = Color.Lerp(Color.green, Color.grey, 0.5f);


                if (GetCurrentGunEquiped(i))
                {
                    go.transform.Find("EquipGunButton").Find("Text").GetComponent<Text>().text = "Equipped";
                    go.transform.Find("EquipGunButton").GetComponent<Image>().color = Color.grey;
                }
                else
                {
                    go.transform.Find("EquipGunButton").GetComponent<Button>().onClick.AddListener(() => OnEquipButtonPressed(n));
                }
            }
            else
            {
                go.transform.Find("BuyAmmoButton").gameObject.SetActive(false);
                go.transform.Find("BuyGunButton").gameObject.SetActive(true);
                go.transform.Find("EquipGunButton").gameObject.SetActive(false);
                go.transform.Find("BuyGunButton").GetComponent<Button>().onClick.AddListener(() => OnBuyWeaponButtonPressed(n));
            }
        }
    }

    void OnBuyAmmoButtonPressed(int n)
    {
        if (saveload.money >= GetAmmoBuyPrice(n))
        {
            saveload.money -= GetAmmoBuyPrice(n);
            SetUpdateAmmo(n);
            saveload.Save();
            OnShopButtonPressed();
        }else{
            
            StartCoroutine(ShowMsg("Not enough money"));
        }
    }

    void OnBuyWeaponButtonPressed(int n)
    {
        if (saveload.money >= GetWeaponBuyPrice(n))
        {
            saveload.money -= GetWeaponBuyPrice(n);
            SetBuyedWeapon(n);
            saveload.Save();
            OnShopButtonPressed();
        }else{
            
            StartCoroutine(ShowMsg("Not enough money"));
        }
    }

    void OnEquipButtonPressed(int n)
    {
        if (n == 2)
        {
            saveload.currentEquipedWeapon = 2;
        }
        else if (n == 1)
        {
            saveload.currentEquipedWeapon = 1;
        }
        else if (n == 4)
        {
            saveload.currentEquipedWeapon = 4;
        }
        else if (n == 3)
        {
            saveload.currentEquipedWeapon = 3;
        }
        else if (n == 0)
        {
            saveload.currentEquipedWeapon = 0;
        }
        print("Save Equip"+saveload.currentEquipedWeapon);
        saveload.Save();
        GunInitializeFromPannel();
        OnShopButtonPressed();
    }


    int GetAmmoBuyPrice(int nu)
    {
        if (nu == 2)
        {
            return saveload.akammobuyPrice;
        }
        else if (nu == 1)
        {
            return saveload.rifleammobuyPrice;
        }
        else if (nu == 4)
        {
            return saveload.sniperammobuyPrice;
        }
        else if (nu == 3)
        {
            return saveload.shotgunammobuyPrice;
        }
        else if (nu == 0)
        {
            return saveload.pistolammobuyPrice;
        }
        else
        {
            return 0;
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

    int GetWeaponBuyPrice(int nu)
    {
        if (nu == 2)
        {
            return saveload.akbuyPrice;
        }
        else if (nu == 1)
        {
            return saveload.rifleBuyPrice;
        }
        else if (nu == 4)
        {
            return saveload.sniperBuyPrice;
        }
        else if (nu == 3)
        {
            return saveload.shotgunBuyPrice;
        }
        else if (nu == 0)
        {
            return saveload.pistolBuyPrice;
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

    void SetUpdateAmmo(int nu)
    {
        if (nu == 2)
        {
            saveload.akAmmo+=saveload.akAmmoBuySet;
        }
        else if (nu == 1)
        {
           saveload.rifleAmmo+=saveload.rifleAmmoBuySet;
        }
        else if (nu == 4)
        {
            saveload.sniperAmmo+=saveload.sniperAmmoBuySet;
        }
        else if (nu == 3)
        {
            saveload.shotgunAmmo+=saveload.shotgunAmmoBuySet;
        }
        else if (nu == 0)
        {
            saveload.pistolAmmo+=saveload.pistolAmmoBuySet;
        }
        
    }

    void SetBuyedWeapon(int nu)
    {
        if (nu == 2)
        {
            saveload.isakBuyed=true;
        }
        else if (nu == 1)
        {
            saveload.isrifleBuyed=true;
        }
        else if (nu == 4)
        {
            saveload.issniperBuyed=true;
        }
        else if (nu == 3)
        {
            saveload.isshotgunBuyed=true;
        }
        else if (nu == 0)
        {
            saveload.ispistolBuyed=true;
        }

    }

    bool GetCurrentGunEquiped(int nu)
    {
        if (nu == 2 && saveload.currentEquipedWeapon==2)
        {
            return true;
        }
        else if (nu == 1&& saveload.currentEquipedWeapon==1)
        {
            return true;
        }
        else if (nu == 4 && saveload.currentEquipedWeapon==4)
        {
            return true;
        }
        else if (nu == 3 && saveload.currentEquipedWeapon==3)
        {
            return true;
        }
        else if (nu == 0 && saveload.currentEquipedWeapon==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Skills

    [Header("Skills")]
    public GameObject SkillCard;
    public Transform ShopPannelSkillContainer;
    public string[] SkillsString;
    public Sprite[] SkillsSprite;
    

    
    void InitializeSkills()
    {
        
        int i=0;
        foreach (var skillname in SkillsString)
        {
            
                GameObject go = Instantiate(SkillCard);
                go.transform.SetParent(ShopPannelSkillContainer);
                go.transform.localScale = Vector3.one;

                go.transform.Find("SkillStats").transform.Find("SkillName").GetComponent<Text>().text = skillname;
                go.transform.Find("SkillIcon").GetComponent<Image>().sprite = SkillsSprite[i];
                if (GetSkillBuyedOrNot(i))
                    go.transform.Find("LevelNuImage").transform.Find("Text").GetComponent<Text>().text = GetSkillLevel(i).ToString();


                if (GetSkillLevel(i) < 20)
                {
                    //pricing
                    if (GetSkillBuyedOrNot(i))
                    {
                        go.transform.Find("SkillStats").transform.Find("SkillCost").transform.GetComponent<Text>().text = GetSkillUpgradeCooldownPrice(i).ToString();
                        go.transform.Find("SkillStats").transform.Find("skillcooldown").gameObject.GetComponent<Text>().text=GetSkillCooldownTime(i).ToString();
                        go.transform.Find("SkillStats").transform.Find("skillduration").gameObject.GetComponent<Text>().text=GetSkillWorkingTime(i).ToString();
                    }
                    else
                    {
                        go.transform.Find("SkillStats").transform.Find("SkillCost").transform.GetComponent<Text>().text = GetSkillBuyPrice(i).ToString();
                    }

                    int n = i;
                    //buy and upgrade button
                    if (GetSkillBuyedOrNot(i))//it means i had already buy this skill
                    {
                        go.transform.Find("UpgradeCooldownButton").gameObject.SetActive(true);
                        go.transform.Find("UpgradeWorkingButton").gameObject.SetActive(true);
                        go.transform.Find("BuySkillButton").gameObject.SetActive(false);
                        go.transform.Find("UpgradeWorkingButton").GetComponent<Button>().onClick.AddListener(() => OnUpgradeSkillWorkingButtonPressed(n));
                        go.transform.Find("UpgradeCooldownButton").GetComponent<Button>().onClick.AddListener(() => OnUpgradeSkillCooldownButtonPressed(n));
                        go.transform.Find("UpgradeWorkingButton").transform.Find("Image").gameObject.SetActive(false);
                        go.transform.Find("UpgradeCooldownButton").transform.Find("Image").gameObject.SetActive(false);
                    }
                    else
                    {
                        go.transform.Find("BuySkillButton").gameObject.SetActive(true);
                        go.transform.Find("UpgradeWorkingButton").gameObject.SetActive(false);
                        go.transform.Find("UpgradeCooldownButton").gameObject.SetActive(false);
                        go.transform.Find("BuySkillButton").GetComponent<Button>().onClick.AddListener(() => OnBuySkillButtonPressed(n));

                    }
                }
                else
                {
                    go.transform.Find("BuySkillButton").gameObject.SetActive(false);
                    go.transform.Find("SkillStats").transform.Find("SkillCost").gameObject.SetActive(false);
                    go.transform.Find("SkillStats").transform.Find("skillcooldown").gameObject.GetComponent<Text>().text=GetSkillCooldownTime(i).ToString();
                    go.transform.Find("SkillStats").transform.Find("skillduration").gameObject.GetComponent<Text>().text=GetSkillWorkingTime(i).ToString();
                }
            
            i++;
        }
    }

    public void OnBuySkillButtonPressed(int n)
    {
        if (saveload.money >= GetSkillBuyPrice(n))
        {
            saveload.money -= GetSkillBuyPrice(n);
            SetBuySkill(n);
            saveload.Save();
            OnShopButtonPressed();
        }else{
            
            StartCoroutine(ShowMsg("Not enough money"));
        }
    }

    public void OnUpgradeSkillCooldownButtonPressed(int n)
    {
        if (saveload.money >= GetSkillUpgradeCooldownPrice(n))
        {
            saveload.money -= GetSkillUpgradeCooldownPrice(n);
            SetUpgradeCooldownSkill(n);
            SetAbilityLevel(n);
            saveload.Save();
            OnShopButtonPressed();
        }else{
            
            StartCoroutine(ShowMsg("Not enough money"));
        }
    }

    public void OnUpgradeSkillWorkingButtonPressed(int n)
    {
        if (saveload.money >= GetSkillUpgradeWorkingPrice(n))
        {
            saveload.money -= GetSkillUpgradeWorkingPrice(n);
            SetUpgradeWorkingSkill(n);
            SetAbilityLevel(n);
            saveload.Save();
            OnShopButtonPressed();
        }else{
            
            StartCoroutine(ShowMsg("Not enough money"));
        }
    }

    void SetBuySkill(int n)
    {
        if (n == 0)
        {
            saveload.isfreezebuyed = true;
            
        }
        else if (n == 1)
        {
            saveload.isradarbuyed = true;
            
        }
        else if (n == 2)
        {
            saveload.isshieldbuyed = true;
        }
        
    }

    void SetUpgradeCooldownSkill(int n)
    {
        if (n == 0)
        {
            saveload.freezeCooldownTime -= 1;
        }
        else if (n == 1)
        {
            saveload.radarCooldownTime -= 1;
        }
        else if (n == 2)
        {
            saveload.shieldCooldownTime -=1;
        }
        
    }

    void SetUpgradeWorkingSkill(int n)
    {
        if (n == 0)
        {
            saveload.freezeWorkingTime += 1;
        }
        else if (n == 1)
        {
            saveload.radarWorkingTime += 1;
        }
        else if (n == 2)
        {
            saveload.shieldWorkingTime += 1;
        }
        
    }

    void SetAbilityLevel(int n)
    {
        if (n == 0)
        {
            saveload.freezelevel++;
        }
        else if (n == 1)
        {
            saveload.radarlevel++;
        }
        else if (n == 2)
        {
            saveload.shieldlevel++;
        }
    }


    bool GetSkillBuyedOrNot(int n)
    {
        bool flag=false;
        if(n==0)
        flag=saveload.isfreezebuyed;
        else if(n==1)
        flag=saveload.isradarbuyed;
        else if(n==2)
        flag=saveload.isshieldbuyed;

        return flag;
    }

    int GetSkillBuyPrice(int n)
    {
        int flag=0;
        if(n==0)
        flag=saveload.freezebuyprice;
        else if(n==1)
        flag=saveload.radarbuyprice;
        else if(n==2)
        flag=saveload.shieldbuyprice;

        return flag;
    }

    int GetSkillLevel(int n)
    {
        int flag=0;
        if(n==0)
        flag=saveload.freezelevel;
        else if(n==1)
        flag=saveload.radarlevel;
        else if(n==2)
        flag=saveload.shieldlevel;

        return flag;
    }

    int GetSkillCooldownTime(int n)
    {
        int flag=0;
        if(n==0)
        {
            flag=saveload.freezeCooldownTime;
        }
        else if(n==1)
        {
            flag=saveload.radarCooldownTime;
        }
        else if(n==2)
        {
            flag=saveload.shieldCooldownTime;
        }

        return flag;
    }

    int GetSkillWorkingTime(int n)
    {
        int flag=0;
        if(n==0)
        {
            flag=saveload.freezeWorkingTime;
        }
        else if(n==1)
        {
            flag=saveload.radarWorkingTime;
        }
        else if(n==2)
        {
            flag=saveload.shieldWorkingTime;
        }

        return flag;
    }

    int GetSkillUpgradeCooldownPrice(int n)
    {
        int flag=0;
        if(n==0)
        {
            flag=saveload.freezelevel*saveload.freezeupgradeCooldownprice;
        }else if(n==1)
        {
            flag=saveload.radarlevel*saveload.radarupgradeCooldownprice;
        }
        else if(n==2)
        {
            flag=saveload.shieldlevel*saveload.shieldupgradeCooldownprice;
        }

        return flag;
    }

    int GetSkillUpgradeWorkingPrice(int n)
    {
        int flag=0;
        if(n==0)
        {
            flag=saveload.freezelevel*saveload.freezeupgradeWorkingprice;
        }else if(n==1)
        {
            flag=saveload.radarlevel*saveload.radarupgradeWorkingprice;
        }
        else if(n==2)
        {
            flag=saveload.shieldlevel*saveload.shieldupgradeWorkingprice;
        }

        return flag;
    }


    #endregion

    #region Result

    void InitializeResult()
    {
        ResultText.text="";
    }

    IEnumerator ShowMsg(string msg)
    {
        ResultText.text=msg;
        yield return new WaitForSeconds(3);
        ResultText.text="";
    }

    #endregion

    #region SelectWeapon

    [Header("Weapons Selector")]

    public InteractiveWeapon MySelectedWeapon;
    public Image MainMenuSelectedWeaponImage;
    public Text MainMenuSelectedWeaponAmmoText;

    public void OnWeaponChangeButtonPressed()
    {
        switchPannel=0;
        OnShopButtonPressed();
    }

    

    void GunInitializeFromPannel()
    {
       if (saveload.currentEquipedWeapon == 2)
        {
            MySelectedWeapon = WeaponsGO[2].GetComponent<InteractiveWeapon>();
            MainMenuSelectedWeaponImage.sprite=WeaponsGO[2].GetComponent<InteractiveWeapon>().sprite;
            MainMenuSelectedWeaponAmmoText.text=GetAmmoNu(2).ToString();
        }
        else if (saveload.currentEquipedWeapon == 1)
        {
            MySelectedWeapon = WeaponsGO[1].GetComponent<InteractiveWeapon>();
            MainMenuSelectedWeaponImage.sprite=WeaponsGO[1].GetComponent<InteractiveWeapon>().sprite;
            MainMenuSelectedWeaponAmmoText.text=GetAmmoNu(1).ToString();
        }
        else if (saveload.currentEquipedWeapon == 4)
        {
            MySelectedWeapon = WeaponsGO[4].GetComponent<InteractiveWeapon>();
            MainMenuSelectedWeaponImage.sprite=WeaponsGO[4].GetComponent<InteractiveWeapon>().sprite;
             MainMenuSelectedWeaponAmmoText.text=GetAmmoNu(4).ToString();
        }
        else if (saveload.currentEquipedWeapon == 3)
        {
            MySelectedWeapon = WeaponsGO[3].GetComponent<InteractiveWeapon>();
            MainMenuSelectedWeaponImage.sprite=WeaponsGO[3].GetComponent<InteractiveWeapon>().sprite;
             MainMenuSelectedWeaponAmmoText.text=GetAmmoNu(3).ToString();
        }
        else if (saveload.currentEquipedWeapon == 0)
        {
            MySelectedWeapon = WeaponsGO[0].GetComponent<InteractiveWeapon>();
            MainMenuSelectedWeaponImage.sprite=WeaponsGO[0].GetComponent<InteractiveWeapon>().sprite;
             MainMenuSelectedWeaponAmmoText.text=GetAmmoNu(0).ToString();
        }
    }
    public void InitializeGun()
    {
        StartCoroutine(WaitAndEquipWeapon());
    }

    IEnumerator WaitAndEquipWeapon()
    {
        yield return new WaitForSeconds(1);
        GameObject[] AllUnlockedWeapons=GameObject.FindGameObjectsWithTag("Weapons");

        foreach(GameObject g in AllUnlockedWeapons)
        {
            if(g.GetComponent<InteractiveWeapon>().label==MySelectedWeapon.label)
            {
                g.GetComponent<InteractiveWeapon>().PickThisWeapon();
                print("Initializ Weapon "+g.GetComponent<InteractiveWeapon>().label);
            }
        }
    }

    #endregion

}
