using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EagleController : MonoBehaviour
{
    EaglePathGenerator eaglePathGenerator;
    public float MinDistance=3f;
    public float speed = 0.01f;
    bool gotSpawner = false;
    Vector3 myNewTarget;
    public float damping = 100;

    Vector3 oldPosition;
    Vector3 currentPosition;
    public FixedTouchField TouchField;
    public FirstPersonController fps;
    void Start()
    {
        gotSpawner = false;
        oldPosition = Vector3.zero;
        currentPosition = Vector3.zero;
        StartCoroutine(WaitTillLoad());
       
    }

    IEnumerator WaitTillLoad()
    {
        yield return new WaitForSeconds(1);
        eaglePathGenerator = GameObject.FindGameObjectWithTag("EagleSpawner").GetComponent<EaglePathGenerator>();
        myNewTarget = eaglePathGenerator.GetRandomPoint(oldPosition, currentPosition);
        gameObject.transform.LookAt(myNewTarget);
        gotSpawner = true;
    }

    void GetNewPoint()
    {
        myNewTarget = eaglePathGenerator.GetRandomPoint(oldPosition, currentPosition);
        //float previousRotation = gameObject.transform.eulerAngles.y;
        //gameObject.transform.LookAt(myNewTarget);
        //gameObject.transform.Find("Body").transform.eulerAngles = new Vector3(gameObject.transform.rotation.x,previousRotation * -1,gameObject.transform.rotation.z);
        //StartCoroutine(ControlSmoothRotation(previousRotation));
    }

    void Update()
    {
        if (gotSpawner)
        {
            if (Vector3.Distance(gameObject.transform.position, myNewTarget) < MinDistance)
            {
                currentPosition=myNewTarget;
                GetNewPoint();
                oldPosition = currentPosition;
                //print(gameObject.transform.eulerAngles.y);
                //float previousRotation = gameObject.transform.eulerAngles.y;
                //gameObject.transform.LookAt(myNewTarget);
                //StartCoroutine(ControlSmoothRotation(previousRotation));
            }
            else
            {
                fps.m_MouseLook.LookAxis = TouchField.TouchDist;
                MoveTo();
                RotateTo();
                //gameObject.transform.Translate(0, 0, speed);
                //Quaternion currentRot = transform.rotation;
                //Quaternion targerRot = Quaternion.Euler(myNewTarget);
                //Quaternion smoothRot = Quaternion.Slerp(gameObject.transform.rotation, targerRot, Time.deltaTime / damping);
                //transform.rotation = smoothRot;
            }
        }
    }

    void MoveTo()
    {
        transform.position = Vector3.MoveTowards(transform.position, myNewTarget, Time.deltaTime * speed);
    }
    Quaternion looking = new Quaternion();
    void RotateTo()
    {
        Vector3 direction = (myNewTarget - transform.position).normalized;
        looking = Quaternion.LookRotation(direction);
        looking.x = 0;
        looking.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, looking, Time.deltaTime * damping);
    }

    IEnumerator ControlSmoothRotation(float previousRotation)
    {
        float difference = previousRotation;
        GameObject g= gameObject.transform.Find("Body").gameObject;
        while (difference > 0 )
        {
            yield return new WaitForSeconds(0.1f);
            if (difference>180)
            difference -= 0.5f;
            else
            difference += 0.5f;
            g.transform.eulerAngles = new Vector3(0, difference, 0);
        
        }

    }


}
