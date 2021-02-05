using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + CameraController.instance.mainCamera.transform.rotation * Vector3.forward,
            CameraController.instance.mainCamera.transform.rotation * Vector3.up);
    }
}
