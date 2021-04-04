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
    //Things for ------------------------ ShopMenu
    //need no save
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

    //----------------end shop things


    //-----------------ability things 
    //save the data
    public static int radarWorkingTime = 15;
    public static int radarCooldownTime = 15;
    public static int freezeWorkingTime = 10;
    public static int freezeCooldownTime = 15;

    //end ability things


    public static string accountID = " ";
    public static string playerName = " ";

    public static float senstivity = 0.5f;
    public static float aimSenstivity = 1f;

    public static int currentLevel = 1;
    public static int money = 1000000;

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
        data.CurrentLevel = currentLevel;
        data.Money = money;

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

        //ability
        data.RadarWorkingTime=radarWorkingTime;
        data.RadarCooldownTime=radarCooldownTime;
         data.FreezeWorkingTime=freezeWorkingTime;
         data.FreezeCooldownTime=freezeCooldownTime;

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
            money = data.Money;
            currentLevel = data.CurrentLevel;

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

            //ability
            radarWorkingTime = data.RadarWorkingTime;
            radarCooldownTime = data.RadarCooldownTime;
            freezeWorkingTime = data.FreezeWorkingTime;
            freezeCooldownTime = data.FreezeCooldownTime;

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
    public int CurrentLevel;
    public int Money;

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

    //ability
    public  int RadarWorkingTime;
    public  int RadarCooldownTime;
    public  int FreezeWorkingTime;
    public  int FreezeCooldownTime;
    
}