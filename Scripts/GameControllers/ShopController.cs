using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Header("Common")]
    public GameObject ShopPannel;
    public GameObject WeaponCard;

    public Text MoneyText;

    public void OnShopButtonPressed()
    {
        ShopPannel.SetActive(true);
        MoneyText.text = saveload.money.ToString();
        InitializeGuns();
        InitializeSkills();
        GunsSectionButtonPressed();
    }

    public void OnShopCloseButtonPressed()
    {
        ShopPannel.SetActive(false);
    }

    [Header("ShopPannel")]
    public GameObject GunsPannel;
    public GameObject SkillPannel;

    public void GunsSectionButtonPressed()
    {
        GunsPannel.SetActive(true);
        SkillPannel.SetActive(false);
    }

    public void SkillsSectionButtonPressed()
    {
        GunsPannel.SetActive(false);
        SkillPannel.SetActive(true);
    }

    #region Guns

    [Header("Guns")]
    public Transform ShopPannelWeaponContainer;
    public GameObject[] WeaponsGO;

    void InitializeGuns()
    {
        GameObject[] shopContents = GameObject.FindGameObjectsWithTag("ShopContent");
        foreach (GameObject g in shopContents)
        {
            Destroy(g);
        }
        
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
                go.transform.Find("BuyAmmoButton").GetComponent<Button>().onClick.AddListener(() => OnBuyAmmoButtonPressed(n));
            }
            else
            {
                go.transform.Find("BuyAmmoButton").gameObject.SetActive(false);
                go.transform.Find("BuyGunButton").gameObject.SetActive(true);
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
        }
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

    #endregion

    #region Skills

    [Header("Skills")]
    public Transform ShopPannelSkillContainer;
    public string[] SkillsString;
    public Sprite[] SkillsSprite;
    

    
    void InitializeSkills()
    {
        GameObject[] shopContents = GameObject.FindGameObjectsWithTag("ShopContent");
        foreach (GameObject g in shopContents)
        {
            Destroy(g);
        }
        int i=0;
        foreach (var skillname in SkillsString)
        {
            GameObject go = Instantiate(WeaponCard);
            go.transform.SetParent(ShopPannelSkillContainer);
            go.transform.localScale = Vector3.one;

            go.transform.Find("WeaponStats").transform.Find("WeaponName").GetComponent<Text>().text = skillname;
            go.transform.Find("WeaponIcon").GetComponent<Image>().sprite = SkillsSprite[i];
            if (GetSkillBuyedOrNot(i))
                go.transform.Find("WeaponStats").transform.Find("WeaponAmmo").GetComponent<Text>().text = "Ammo:" + GetAmmoNu(i).ToString();

            if (GetSkillBuyedOrNot(i))
                go.transform.Find("WeaponStats").transform.Find("WeaponCost").GetComponent<Text>().text = GetAmmoBuyPrice(i).ToString();
            else
                go.transform.Find("WeaponStats").transform.Find("WeaponCost").GetComponent<Text>().text = GetWeaponBuyPrice(i).ToString();

            int n = i;
            if (GetSkillBuyedOrNot(i))
            {
                go.transform.Find("BuyAmmoButton").gameObject.SetActive(true);
                go.transform.Find("BuyGunButton").gameObject.SetActive(false);
                go.transform.Find("BuyAmmoButton").GetComponent<Button>().onClick.AddListener(() => OnBuyAmmoButtonPressed(n));
            }
            else
            {
                go.transform.Find("BuyAmmoButton").gameObject.SetActive(false);
                go.transform.Find("BuyGunButton").gameObject.SetActive(true);
                go.transform.Find("BuyGunButton").GetComponent<Button>().onClick.AddListener(() => OnBuyWeaponButtonPressed(n));
            }
            i++;
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
}
