using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{

    void Awake()
    {
        saveload.Load();
        UpdateSettingInfoAndSendAnalitcsToServer();
    }


    void Start()
    {
        StartLoadingScreen("HuntGame");
    }

    void UpdateSettingInfoAndSendAnalitcsToServer()
    {
        if (saveload.accountID != " ")
        {
            //means account is created
            saveload.appOpen++;
            saveload.Save();
            StartCoroutine(SendSettingInfoToServer());
        }
    }

    IEnumerator SendSettingInfoToServer()
    {
        
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("Name", saveload.playerName);
        form1.AddField("AppOpen", saveload.appOpen);
        form1.AddField("NormalSenstivity", saveload.senstivity.ToString());
        form1.AddField("AimSenstivity", saveload.aimSenstivity.ToString());
        form1.AddField("AimAssist", saveload.isAimAssist.ToString());
        form1.AddField("Autofire", saveload.isAutoFire.ToString());
        WWW www = new WWW(saveload.serverLocation + saveload.serverUpdateRepeat, form1);
        yield return www;
    }


    #region Common Loading Screen

    [Header("Loading Screen")]
    public GameObject LoadingPannel;
    public Sprite[] LoadingImages;
    public Image LoadingBackground;
    public Image LoadingBar;

    public void StartLoadingScreen(string sceneName)
    {
        SetrandomBackground();
        StartCoroutine(WaitForFlashDelay(sceneName));
    }

    IEnumerator WaitForFlashDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        LoadingPannel.SetActive(true);
        AsyncOperation game = SceneManager.LoadSceneAsync(sceneName);

        while (game.progress < 1)
        {
            LoadingBar.fillAmount = game.progress;
            yield return new WaitForEndOfFrame();
        }
    }

    void SetrandomBackground()
    {
        int nu = Random.Range(0, LoadingImages.Length);
        LoadingBackground.sprite = LoadingImages[nu];
    }

    #endregion
   
}
