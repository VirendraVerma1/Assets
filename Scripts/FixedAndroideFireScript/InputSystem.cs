using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ControlMode {Keyboard = 1,Touch = 2};

public class InputSystem : MonoBehaviour
{
	public Camera Cam;
	public FixedJoystick joystick;
	 
	void Update(){
		float angleV = joystick.Vertical;
		float angleH= joystick.Horizontal;
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Cam.gameObject.transform.localRotation = aimRotation;
	}
}
