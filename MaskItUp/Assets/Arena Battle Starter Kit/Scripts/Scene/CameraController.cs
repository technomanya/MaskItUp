using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    /* prefabs */

    /* public vars */
    public GameObject mainCamera;
    public Transform target;
    public float smoothTime = 0.3F;
    public float distanceToTarget = -10.0f;

    /* private vars */
    private float _currentVelocity = 0;
    private float _startXPos;
    private float _startYPos;

    void Awake()
    {
        instance = this;

        this._startXPos = this.transform.position.x;
        this._startYPos = this.transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetZPos = target.transform.position.z + distanceToTarget;
        float currentZpos = Mathf.SmoothDamp(this.transform.position.z, targetZPos, ref _currentVelocity, smoothTime);

        this.transform.position = new Vector3(_startXPos, _startYPos, currentZpos);
    }
}
