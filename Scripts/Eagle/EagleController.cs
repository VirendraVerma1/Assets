using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Material RedMarkerForGaurds;
    public Camera myCamera;
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
                
            }
            else
            {
                
                MoveTo();
                RotateTo();

                RaycastHit hit;

                if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 100.0f))
                {
                    if (hit.collider.gameObject.tag == "Gaurd")
                    {
                        GameObject Gaurd = hit.collider.gameObject;
                        if (Gaurd.transform.Find("TerroristMarker").gameObject != null)
                        {
                            Gaurd.transform.Find("TerroristMarker").gameObject.layer = LayerMask.NameToLayer("Marker");
                            Gaurd.transform.Find("TerroristMarker").gameObject.GetComponent<Renderer>().material = RedMarkerForGaurds;
                        }
                        print("found gaurd");
                    }
                    else
                    {
                        print(hit.collider.gameObject.name);
                    }
                }
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
