using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControlMode {Keyboard = 1,Touch = 2};

public class InputSystem : MonoBehaviour
{
	public Camera Cam;
	public Joystick joystick;
	Vector3 temp = Vector3.zero;
	public bool miniBulletTouchField=false;

	void Update(){
		float angleV = Mathf.Clamp(joystick.Vertical,-1,1);
		float angleH= Mathf.Clamp(joystick.Horizontal,-1,1);
		Vector3 test = new Vector3(-angleV, angleH, 0);
		
		if (test!=Vector3.zero)
        {
			temp = test;
			miniBulletTouchField = true;
		}
		else
        {
			miniBulletTouchField = false;
		}
		Quaternion aimRotation = Quaternion.EulerAngles(temp);
		print(aimRotation);
		Cam.gameObject.transform.rotation = aimRotation;
	}
}
