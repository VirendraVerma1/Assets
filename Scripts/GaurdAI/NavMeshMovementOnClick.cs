using UnityEngine;
using UnityEngine.AI;

public class NavMeshMovementOnClick : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 TargetPoint;
    // Update is called once per frame

    private GameObject temp;
    public bool setPosition = false;

    void Start()
    {
        TargetPoint = gameObject.transform.position;
    }
   
    public void SettargetPosition(Vector3 targetPosition)
    {
        
        //move our agent
        if(agent.enabled)
           agent.SetDestination(targetPosition);
          
        //print(agent.desiredVelocity + "velocity" + agent.stoppingDistance + "stopping distance" + agent.remainingDistance);
    }
}
