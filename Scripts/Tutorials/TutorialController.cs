using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    //tutorial path
    //move player near to the enemy
    //then give task to shoot
    //after shot the enemy
    //highlight radar skill
    //move player to other position
    //give order to kill them
    //show freeze ability
    //now shoot other enemy
    //after killing show eagle vission
    //move player to that enemy
    //kill and end tutorial


    //check if first time
    void Start()
    {
        TutorialPannel.SetActive(false);
        if (saveload.isTutorial)
        {
            gc = gameObject.GetComponent<GameController>();
            MessageText.text = "";
            SetUpMarker();
        }
    }

    [Header("Marker")]
    public GameObject TutorialPannel;
    public Text MessageText; 
    public GameObject MarkerPrefab;
    public Transform[] MarkerPaths;
    public GameObject AimButton;
    public GameObject RadarSkill;
    public GameObject FreezeSkill;
    public GameObject FireButton;
    public GameObject EagleButton;
    public GameObject KnifeButton;
    public float minDistance = 5;
    public Transform FirstGaurdpoint;

    //second move
    public Transform MoveToSecondPosition;

    //third move
    public Transform MoveToThirdPosition;

    //fourth move
    public Transform MoveToFourthPosition;

    public Material GaurdMarkerYellow;
    public Material GaurdMarkerRed;

    int step = 0;
    GameObject Player=null;
    int counter = 0;
    int oldbotCount = 0;
    Transform markerTransform;
    GameController gc;
    int firebutton = 0;

    #region step1 move player near to the enemy

    void SetUpMarker()
    {
        if (MarkerPaths.Length > counter)
        {
            MarkerPrefab.transform.position = MarkerPaths[counter].transform.position;
            markerTransform = MarkerPaths[counter].transform;
            counter++;
            StartCoroutine(CalculateDistanceFromMarkerAndChange());
        }
        else
        {
            //UI tutorails
            step++;
            MarkerPrefab.SetActive(false);
            NextStepGiveTaskToShoot();
        }
    }

    IEnumerator CalculateDistanceFromMarkerAndChange()
    {
        float distance = 999999;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                distance = Vector3.Distance(Player.transform.position, markerTransform.position);
                if (distance < minDistance)
                    break;
            }
        }
        SetUpMarker();
    }

    #endregion

    #region Step 2 then give task to shoot

    void NextStepGiveTaskToShoot()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Aim on Gaurd";
        AimButton.SetActive(true);
        step++;
    }

    public void SecondAimButtonPressed()
    {
        if (step == 2)
        {
            TutorialPannel.SetActive(false);
            oldbotCount = gc.botCount;
            //box ke nearby jo bhi gaurd hoga uska markey higlight ho jayega
            
            GameObject[] Allgaurds = GameObject.FindGameObjectsWithTag("Gaurd");
            GameObject NearestGaurd=null;
            float minDistance = 999999;
            foreach(GameObject g in Allgaurds)
            {
                float distance=Vector3.Distance(FirstGaurdpoint.position, g.transform.position);
                if(distance<minDistance)
                {
                    minDistance = distance;
                    NearestGaurd = g;
                }
            }
            //set market
            NearestGaurd.transform.Find("TerroristMarker").GetComponent<Renderer>().material = GaurdMarkerRed;
            NearestGaurd.transform.Find("TerroristMarker").gameObject.layer = LayerMask.NameToLayer("OldMarker");
            StartCoroutine(WaitAndCountBot());
        }
        else if(step==3)
        {
            TutorialPannel.SetActive(false);
            StartCoroutine( NextStepHighlightRadarSkill());
        }
    }

    IEnumerator WaitAndCountBot()
    {
        int newBotCount = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            newBotCount = gc.botCount;
            if (newBotCount < 4)
                break;
        }
        NextStepDeactivateAimMode();
    }

    #endregion

    #region Step3 Deactivate aim mode

    void NextStepDeactivateAimMode()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Off Aim Button";
        AimButton.SetActive(true);
        step++;
    }

    #endregion

    #region Step4 highlight radar skill

    IEnumerator NextStepHighlightRadarSkill()
    {
        yield return new WaitForSeconds(1);
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Use Radar Skill";
        RadarSkill.SetActive(true);
    }

    public void SecondRadarSkillButtonPressed()
    {
        TutorialPannel.SetActive(false);
        MarkerPrefab.SetActive(true);
        MarkerPrefab.transform.position = MoveToSecondPosition.position;
        markerTransform = MoveToSecondPosition;
        StartCoroutine(CalculateDistanceFromMarkerAndChangeSecond());
    }

    IEnumerator CalculateDistanceFromMarkerAndChangeSecond()
    {
        float distance = 999999;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                distance = Vector3.Distance(Player.transform.position, markerTransform.position);
                if (distance < ((float)minDistance/2))
                    break;
            }
        }
        MarkerPrefab.SetActive(false);
        ShowUseFreezeAbility();
    }

    #endregion

    #region Step5 use freeze ability

    void ShowUseFreezeAbility()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Use Freeze Skill";
        FreezeSkill.SetActive(true);
        step++;
    }

    public void FreeqeAbilityButtonPressed()
    {
        if (firebutton == 0)
        {
            firebutton++;
            DeactivateAll();
            TutorialPannel.SetActive(false);
            ShowUseFireButton();
        }
    }

    #endregion

    #region Step6 use fire button to shoot

    void ShowUseFireButton()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Use Fire Button to Kill";
        FireButton.SetActive(true);
    }

    public void FireButtonPressed()
    {
        TutorialPannel.SetActive(false);
        oldbotCount = gc.botCount;
        StartCoroutine(WaitCountBotSecond());
    }

    IEnumerator WaitCountBotSecond()
    {
        int newBotCount = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            newBotCount = gc.botCount;
            if (newBotCount < 2)
                break;
        }
        ShowThirdMarker();
    }

    #endregion

    #region Step7 use eagle vission (currently not using)

    void ShowEagleButton()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Use your Eagle";
        EagleButton.SetActive(true);
    }

    public void OneagleButtonPressed()
    {
        TutorialPannel.SetActive(false);
    }

    #endregion

    #region Step8 use move towards Third Marker

    void ShowThirdMarker()
    {
        MarkerPrefab.SetActive(true);
        MarkerPrefab.transform.position = MoveToThirdPosition.position;
        markerTransform = MoveToThirdPosition;
        StartCoroutine(CalculateDistanceFromMarkerAndChangeThird());
    }

    IEnumerator CalculateDistanceFromMarkerAndChangeThird()
    {
        float distance = 999999;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                distance = Vector3.Distance(Player.transform.position, markerTransform.position);
                if (distance < ((float)minDistance/2))
                    break;
            }
        }
        MarkerPrefab.SetActive(false);
        ShowFourthMarker();
    }

    #endregion

    #region Step9 use move towards Fourth Marker

    void ShowFourthMarker()
    {
        MarkerPrefab.SetActive(true);
        MarkerPrefab.transform.position = MoveToFourthPosition.position;
        markerTransform = MoveToFourthPosition;
        StartCoroutine(CalculateDistanceFromMarkerAndChangeFourth());
    }

    IEnumerator CalculateDistanceFromMarkerAndChangeFourth()
    {
        float distance = 999999;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                distance = Vector3.Distance(Player.transform.position, markerTransform.position);
                if (distance < ((float)minDistance/4))
                    break;
            }
        }
        MarkerPrefab.SetActive(false);
        ShowKnifeButton();
    }

    #endregion

    #region Step10 Use Knife to kill last gaurd

    void ShowKnifeButton()
    {
        DeactivateAll();
        TutorialPannel.SetActive(true);
        MessageText.text = "Use your Knife to kill gaurd";
        KnifeButton.SetActive(true);
    }

    public void OnKnifeButtonPressed()
    {
        DeactivateAll();
        TutorialPannel.SetActive(false);
        saveload.isTutorial=false;
        saveload.Save();
    }

    #endregion

    #region common functions

    void DeactivateAll()
    {
        AimButton.SetActive(false);
        RadarSkill.SetActive(false);
        FreezeSkill.SetActive(false);
        FireButton.SetActive(false);
        EagleButton.SetActive(false);
        KnifeButton.SetActive(false);
    }

    #endregion

}
