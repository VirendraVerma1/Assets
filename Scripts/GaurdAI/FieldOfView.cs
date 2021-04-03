using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;
    float playerDisableTimer = 5;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
    public Transform FromPoint;
	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

    public bool isPlayer = false;
    public GameObject Player;

	void Start() {
        //Player= GameObject.FindGameObjectWithTag("Player");
		StartCoroutine ("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(FromPoint.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - FromPoint.position).normalized;
            if (Vector3.Angle(FromPoint.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(FromPoint.position, target.position);
                

                if (!Physics.Raycast(FromPoint.position, dirToTarget, dstToTarget, obstacleMask))
                {
					visibleTargets.Add (target);
                   
                    gameObject.GetComponent<GaurdController>().SetPlayerLocation(target.position);
                     playerDisableTimer = 5;
                     if (!isPlayer)
                     {
                       
                        
                    //     Player.GetComponent<AudioPlayer>().StopAllOtherAudio();
                    //     gameObject.GetComponent<AudioPlayer>().StopAllOtherAudio();
                    //     gameObject.GetComponent<AudioPlayer>().StartAllOtherAudio();
                    //     gameObject.GetComponent<AudioPlayer>().PlayAudioWithMultipleName("Seen");
                         StartCoroutine(DisablePlayer());
                    //     StartCoroutine(RunAudio());
                    //     GameObject.Find("GameController").GetComponent<AudioController>().isChasing = true;
                     }
                     isPlayer = true;
                   
				}
			}
		}
	}
    // float playerDisableTimer = 5;
    // IEnumerator RunAudio()
    // {
    //     yield return new WaitForSeconds(2);
    //     gameObject.GetComponent<AudioPlayer>().StopAllOtherAudio();
    //     gameObject.GetComponent<AudioPlayer>().StartAllOtherAudio();
    //     gameObject.GetComponent<AudioPlayer>().PlayAudioWithMultipleName("Run");
    // }
     IEnumerator DisablePlayer()
     {
         while (playerDisableTimer>0)
         {
             yield return new WaitForSeconds(1);
             playerDisableTimer -= 1;
         }
         isPlayer = false;
         gameObject.GetComponent<GaurdController>().SetRandomWaypoint();
         //gameObject.GetComponent<AudioPlayer>().StopAllOtherAudio();
         //gameObject.GetComponent<AudioPlayer>().StartAllOtherAudio();
         //gameObject.GetComponent<AudioPlayer>().PlayAudioWithName("RunEnd");
         //GameObject.Find("GameController").GetComponent<AudioController>().isChasing = false;
     }

        


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
            angleInDegrees += FromPoint.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
