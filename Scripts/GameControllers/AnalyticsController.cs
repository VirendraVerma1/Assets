using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsController : MonoBehaviour
{
    
    void Start()
    {
        CheackAccount();
        StartCoroutine(UpdateThingsAtACertainTime());
    }

    
    

    #region account creation

    void CheackAccount()
    {
        if (saveload.accountID == " ")
        {
            //means create new account
            StartCoroutine(CreateAccountToServer());
        }
        
    }

    IEnumerator CreateAccountToServer()
    {
        saveload.playerName = "Player" + Random.Range(1111, 99999);
        saveload.Save();
        WWWForm form1 = new WWWForm();
        form1.AddField("name", saveload.playerName);
        WWW www = new WWW(saveload.serverLocation + saveload.serverCreateAccount, form1);
        yield return www;
        
        if (www.text != "" && www.text!=" " && !www.text.Contains("<"))
        {
            string ane = GetDataValue(www.text,"Created:");
            saveload.accountID = ane;
            saveload.Save();
        }
        
    }

    #endregion

    #region update playing info to server

    IEnumerator UpdateThingsAtACertainTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(15);
            saveload.timePlayed+=15;
            saveload.Save();
            if(saveload.accountID!=" ")
            {
                StartCoroutine(UpdateThingsToServer());
            }
            
        }
    }

    IEnumerator UpdateThingsToServer()
    {
        
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("name", saveload.playerName);
        form1.AddField("timeplayed", saveload.timePlayed);
        form1.AddField("MissionsCompleted", saveload.currentLevel);
        form1.AddField("ads", saveload.adsWatched);
        form1.AddField("Money", saveload.money);
        form1.AddField("PistolAmmo", saveload.pistolAmmo);
        form1.AddField("RifleAmmo", saveload.rifleAmmo);
        form1.AddField("ShotgunAmmo", saveload.shotgunAmmo);
        form1.AddField("SniperAmmo", saveload.sniperAmmo);
        form1.AddField("AKAmmo", saveload.akAmmo);
        WWW www = new WWW(saveload.serverLocation + saveload.serverUpdateStats, form1);
        yield return www;
        print(www.text);
    }

    #endregion
    string GetDataValue(string data, string index)  
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
