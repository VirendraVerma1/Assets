using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    bool isTimeFreeze = false;
    public GameObject GaurdAI;


    void Start()
    {
        isTimeFreeze = false;
    }

    private void Update() {

        
    }

    #region Radar Ability

    public GameObject RadarSystemPannel;
    public GameObject RadarButton;
    
    public void OnRadarButtonPressed()
    {
        RadarSystemPannel.SetActive(true);
        RadarButton.SetActive(false);
        StartCoroutine(RadarDisable());
    }

    IEnumerator RadarDisable()
    {
        yield return new WaitForSeconds(10);
        RadarSystemPannel.SetActive(false);
        RadarButton.SetActive(true);
    }
    

    #endregion

    #region Freeze Ability

    public GameObject FreezeTimeButton;
    public void OnAndroidTimeFrezeButtonPressed()
    {
        if (isTimeFreeze)
        {
            isTimeFreeze = false;
            UnFreezeEveryThing();
        }
        else
        {
            isTimeFreeze = true;
            FreezeTimeButton.SetActive(false);
            StartCoroutine(ShowFreezeTimeButton());
            FreezeEveryThing();
        }
    }

    IEnumerator ShowFreezeTimeButton()
    {
        
        yield return new WaitForSeconds(15);
        OnAndroidTimeFrezeButtonPressed();
        FreezeTimeButton.SetActive(true);
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
    }

    #endregion
}
