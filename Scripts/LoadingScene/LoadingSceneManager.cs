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
    }


    void Start()
    {
        StartLoadingScreen("HuntGame");
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
