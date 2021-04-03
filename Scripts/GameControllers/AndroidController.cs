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

    

    void Awake()
    {
        basicBehaviour.isAndroid=isAndroid;
        thirdPersonOrbitCam.isAndroid=isAndroid;
        shootBehaviour.isAndroid=isAndroid;
        controlsTutorial.isAndroid=isAndroid;
        aimBehaviour.isAndroid=isAndroid;
        moveBehaviour.isAndroid=isAndroid;
    }

    public void OnAndroidPickButtonPressed()
    {
        GameObject[] go=GameObject.FindGameObjectsWithTag("Weapons");
        foreach(GameObject g in go)
        {
            if (g.GetComponent<InteractiveWeapon>())
            g.GetComponent<InteractiveWeapon>().OnAndroidPick=true;
        }
        StartCoroutine(DisableAllPickEvents(go));
    }

    IEnumerator DisableAllPickEvents(GameObject[] go)
    {
        yield return new WaitForSeconds(0.5f);
        foreach(GameObject g in go)
        {
            g.GetComponent<InteractiveWeapon>().OnAndroidPick=false;
        }
    }


    
}
