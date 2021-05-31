using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;


public class saveload : MonoBehaviour
{
    //Server files
    public static string serverLocation = "http://kreasaard.atwebpages.com/OneManArmy/";
    public static string serverCreateAccount = "createaccount.php";
    public static string serverUpdateRepeat = "updaterepeat.php";
    public static string serverUpdateStats = "updatestats.php";
    public static string servergetRank = "getrank.php";
    public static string serveradnet = "adnet.php";
    public static string serverinitialize = "initialize.php";

    //Things for ------------------------ ShopMenu
    //no need to save
    public static int pistolBuyPrice = 50;
    public static int rifleBuyPrice = 1000;
    public static int akbuyPrice = 3000;
    public static int shotgunBuyPrice = 5000;
    public static int sniperBuyPrice = 10000;

    public static int pistolammobuyPrice = 5;
    public static int rifleammobuyPrice = 10;
    public static int akammobuyPrice = 50;
    public static int shotgunammobuyPrice = 100;
    public static int sniperammobuyPrice = 200;

    public static int pistolAmmoBuySet = 20;
    public static int rifleAmmoBuySet = 50;
    public static int akAmmoBuySet = 50;
    public static int shotgunAmmoBuySet = 20;
    public static int sniperAmmoBuySet = 20;


    //need save
    public static bool ispistolBuyed = true;
    public static bool isrifleBuyed = false;
    public static bool isakBuyed = false;
    public static bool isshotgunBuyed = false;
    public static bool issniperBuyed = false;

    public static int pistolAmmo = 100;
    public static int rifleAmmo = 200;
    public static int akAmmo = 200;
    public static int shotgunAmmo = 50;
    public static int sniperAmmo = 50;

    public static int currentEquipedWeapon=0;

    //----------------end shop things


    //-----------------ability things 
    //no need to save
    public static int freezebuyprice=100;
    public static int radarbuyprice=2000;
    public static int shieldbuyprice=5000;

    public static int freezeupgradeWorkingprice=100;
    public static int freezeupgradeCooldownprice=100;
    public static int radarupgradeWorkingprice=200;
    public static int radarupgradeCooldownprice=200;
    public static int shieldupgradeWorkingprice=300;
    public static int shieldupgradeCooldownprice=300;

    //save the data
    public static int radarWorkingTime = 10;
    public static int radarCooldownTime = 30;
    public static int freezeWorkingTime = 5;
    public static int freezeCooldownTime = 40;
    public static int shieldWorkingTime=10;
    public static int shieldCooldownTime=30;
    public static int doubleSprintWorkingTime=15;
    public static int doubleSprintCooldownTime=15;

    public static bool isfreezebuyed=true;
    public static bool isradarbuyed=false;
    public static bool isshieldbuyed=false;

    public static int freezelevel=1;
    public static int radarlevel=1;
    public static int shieldlevel=1;

    //end ability things


    public static string accountID = " ";
    public static string playerName = " ";

    public static float senstivity = 0.5f;
    public static float aimSenstivity = 1f;
    public static float eaglecamSenstivity=1f;

    public static int currentLevel = 1;
    public static int money = 1000;
    public static bool isrestartMission = false;
    public static int currentterrainIndex = 0;
    public static bool isAimAssist=true;
    public static bool isAutoFire=true;
    public static int appOpen=0;
    public static int adsWatched=0;

    public static int timePlayed=0;

    public static string current_filename = "info.dat";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + current_filename);
        Notebook_Data data = new Notebook_Data();


        data.AccountID = accountID;
        data.PlayerName = Encrypt(playerName);
        data.Senstivity = senstivity;
        data.AimSenstivity = aimSenstivity;
        data.EagleCamSenstivity = eaglecamSenstivity;
        data.CurrentLevel = currentLevel;
        data.Money = money;
        data.IsRestartMission = isrestartMission;
        data.CurrentTerrainIndex = currentterrainIndex;
        data.IsAutoFire=isAutoFire;
        data.IsAimAssist=isAimAssist;
        data.AppOpen=appOpen;
        data.TimePlayed=timePlayed;
        data.AdsWatched=adsWatched;

        //shop things
        data.IspistolBuyed = ispistolBuyed;
        data.IsakBuyed = isakBuyed;
        data.IsrifleBuyed = isrifleBuyed;
        data.IsshotgunBuyed = isshotgunBuyed;
        data.IssniperBuyed = issniperBuyed;

        data.PistolAmmo = pistolAmmo;
        data.RifleAmmo = rifleAmmo;
        data.AkAmmo = akAmmo;
        data.ShotgunAmmo = shotgunAmmo;
        data.SniperAmmo = sniperAmmo;

        data.CurrentEquipedWeapon=currentEquipedWeapon;

        //ability
        data.RadarWorkingTime=radarWorkingTime;
        data.RadarCooldownTime=radarCooldownTime;
        data.FreezeWorkingTime=freezeWorkingTime;
        data.FreezeCooldownTime=freezeCooldownTime;
        data.ShieldWorkingTime=shieldWorkingTime;
        data.ShieldCooldownTime=shieldCooldownTime;
        data.DoubleSprintCooldownTime=doubleSprintCooldownTime;
        data.DoubleSprintWorkingTime=doubleSprintWorkingTime;

        data.IsFreezeBuyed=isfreezebuyed;
        data.IsRadarBuyed=isradarbuyed;
        data.IsShieldBuyed=isshieldbuyed;

        data.FreezeLevel=freezelevel;
        data.ShieldLevel=shieldlevel;
        data.RadarLevel=radarlevel;


        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/" + current_filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + current_filename, FileMode.Open);/* */
            Notebook_Data data = (Notebook_Data)bf.Deserialize(file);

            accountID=data.AccountID;
            playerName=Decrypt(data.PlayerName);
            senstivity = data.Senstivity;
            aimSenstivity = data.AimSenstivity;
            eaglecamSenstivity = data.EagleCamSenstivity;
            money = data.Money;
            adsWatched=data.AdsWatched;
            currentLevel = data.CurrentLevel;
            currentterrainIndex = data.CurrentTerrainIndex;
            isrestartMission = data.IsRestartMission;
            isAimAssist=data.IsAimAssist;
            isAutoFire=data.IsAutoFire;
            appOpen=data.AppOpen;
            timePlayed=data.TimePlayed;

            //shopthings
            issniperBuyed=data.IssniperBuyed;
            isshotgunBuyed=data.IsshotgunBuyed;
            isrifleBuyed = data.IsrifleBuyed;
            isakBuyed = data.IsakBuyed;
            ispistolBuyed = data.IspistolBuyed;

            pistolAmmo=data.PistolAmmo;
            rifleAmmo=data.RifleAmmo;
            akAmmo=data.AkAmmo;
            shotgunAmmo=data.ShotgunAmmo;
            sniperAmmo=data.SniperAmmo;

            currentEquipedWeapon=data.CurrentEquipedWeapon;

            //ability
            radarWorkingTime = data.RadarWorkingTime;
            radarCooldownTime = data.RadarCooldownTime;
            freezeWorkingTime = data.FreezeWorkingTime;
            freezeCooldownTime = data.FreezeCooldownTime;
            shieldCooldownTime=data.ShieldCooldownTime;
            shieldWorkingTime=data.ShieldWorkingTime;
            doubleSprintWorkingTime=data.DoubleSprintWorkingTime;
            doubleSprintCooldownTime=data.DoubleSprintCooldownTime;

            isfreezebuyed=data.IsFreezeBuyed;
            isradarbuyed=data.IsRadarBuyed;
            isshieldbuyed=data.IsShieldBuyed;

            freezelevel=data.FreezeLevel;
            radarlevel=data.RadarLevel;
            shieldlevel=data.ShieldLevel;
            
            file.Close();

        }
        else
        {
            current_filename = "info.dat";
            accountID = " ";
            saveload.Save();

        }
    }

    private static string hash="9452@abc";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }

   
}


[Serializable]
class Notebook_Data
{
    public  string AccountID;
    public  string PlayerName;
    public float Senstivity;
    public float AimSenstivity;
    public float EagleCamSenstivity;
    public int CurrentLevel;
    public int Money;
    public bool IsRestartMission;
    public int CurrentTerrainIndex;
    public bool IsAutoFire;
    public bool IsAimAssist;
    public int TimePlayed;
    public int AppOpen;
    public int AdsWatched;

    //shop things
    public  bool IspistolBuyed  ;
    public  bool IsrifleBuyed  ;
    public  bool IsakBuyed  ;
    public  bool IsshotgunBuyed  ;
    public  bool IssniperBuyed  ;

    public  int PistolAmmo;
    public  int RifleAmmo;
    public  int AkAmmo;
    public  int ShotgunAmmo;
    public  int SniperAmmo ;
    public int CurrentEquipedWeapon;

    //ability
    public int RadarWorkingTime;
    public int RadarCooldownTime;
    public int FreezeWorkingTime;
    public int FreezeCooldownTime;
    public int ShieldWorkingTime;
    public int ShieldCooldownTime;
    public int DoubleSprintWorkingTime;
    public int DoubleSprintCooldownTime;

    public bool IsFreezeBuyed;
    public bool IsRadarBuyed;
    public bool IsShieldBuyed;

    public int FreezeLevel;
    public int RadarLevel;
    public int ShieldLevel;
    
}