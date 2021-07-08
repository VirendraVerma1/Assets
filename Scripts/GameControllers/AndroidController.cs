using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
    public BasicBehaviour basicBehaviour;
    public ThirdPersonOrbitCam thirdPersonOrbitCam;
    public ShootBehaviour shootBehaviour;
    public ControlsTutorial controlsTutorial;
    public AimBehaviour aimBehaviour;
    public MoveBehaviour moveBehaviour;
    public bool isAndroid=true;
    private float movementDivide = 1;

    GameObject mainCamera;
    GameObject weaponCamera;
    GameObject currentHoldWeapon;
    
        
    void Awake()
    {
        basicBehaviour.isAndroid=isAndroid;
        thirdPersonOrbitCam.isAndroid=isAndroid;
        shootBehaviour.isAndroid=isAndroid;
        controlsTutorial.isAndroid=isAndroid;
        aimBehaviour.isAndroid=isAndroid;
        moveBehaviour.isAndroid=isAndroid;
        OnEagleCameraButtonPressed();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera");
        InitializeSounds();
        
    }

    public void OnAndroidPickButtonPressed()
    {
        GameObject[] go=GameObject.FindGameObjectsWithTag("Weapons");
        foreach(GameObject g in go)
        {
            if (g.GetComponent<InteractiveWeapon>())
            g.GetComponent<InteractiveWeapon>().OnAndroidPick=true;
        }
        StartCoroutine(HidePickUpButton());
        //StartCoroutine(DisableAllPickEvents(go));
       
    }

    IEnumerator HidePickUpButton()
    {
        
        yield return new WaitForSeconds(0.3f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ShopPickDownButton();
    }

    IEnumerator DisableAllPickEvents(GameObject[] go)
    {
        yield return new WaitForSeconds(0.5f);
        foreach(GameObject g in go)
        {
            g.GetComponent<InteractiveWeapon>().OnAndroidPick=false;
        }
    }

    public void InitializeCurrentWeapon(GameObject g)
    {
        print(g.name);
        currentHoldWeapon = g;
    }

    public void InitailizeWeaponCamera()
    {
        mainCamera.GetComponent<Camera>().enabled = false;
        weaponCamera.GetComponent<Camera>().enabled = true;
        //g.transform.SetParent(weaponCamera.transform);
        weaponCamera.transform.SetParent(currentHoldWeapon.transform);
        weaponCamera.transform.localPosition=Vector3.zero;
        weaponCamera.transform.localRotation = Quaternion.Euler( 0,-90,0);
        weaponCamera.transform.localPosition = currentHoldWeapon.GetComponent<InteractiveWeapon>().WeaponCameraHolder;
        
    }

    public void RemoveWeaponCamera()
    {
        weaponCamera.transform.parent = null;
        mainCamera.GetComponent<Camera>().enabled = true;
        weaponCamera.GetComponent<Camera>().enabled = false;
   }

    bool isEagleMode = false;
    public GameObject MainCamera;
    public GameObject EagleCamera;
    public GameObject[] AndroidControllers;
    

    public void OnEagleCameraButtonPressed()
    {
        if (isEagleMode)
        {
            isEagleMode = false;
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.GetComponent<ThirdPersonOrbitCam>().enabled = false;
            EagleCamera.GetComponent<Camera>().enabled = true;
            EagleCamera.GetComponent<FreeCam>().enabled = true;
            foreach(GameObject g in AndroidControllers)
            g.SetActive(false);
            
        }
        else
        {
            isEagleMode = true;
            MainCamera.GetComponent<Camera>().enabled = true;
            MainCamera.GetComponent<ThirdPersonOrbitCam>().enabled = true;
            EagleCamera.GetComponent<Camera>().enabled = false;
            EagleCamera.GetComponent<FreeCam>().enabled = false;
            foreach(GameObject g in AndroidControllers)
            g.SetActive(true);
            
        }
    }

    #region background sounds

    void InitializeSounds()
    {
        StartCoroutine(WaitAndChangeBackgroundSound());
    }

    IEnumerator WaitAndChangeBackgroundSound()
    {
        while(true)
        {
            int random=Random.Range(1,3);
            string temp="Ambidient"+random;
            FindObjectOfType<AudioManager>().Play(temp);
            
            yield return new WaitForSeconds(300);
        }
    }

    #endregion


    #region new fire mechanism

    public RectTransform HandleFire;
    public Joystick joystick;
    public void OnResetFireTouchButtonPressed()
    {
        HandleFire.localPosition = Vector3.zero;
        joystick.ResetInput();
    }

    #endregion

}
