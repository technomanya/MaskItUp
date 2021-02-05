using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public FloatingJoystick fJoyStick;
    public float playerSpeed = 10;
    public Animator LaraAnim;

    public Vector3 movePose;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movePose =new Vector3( fJoyStick.Horizontal, 0, fJoyStick.Vertical);
        if(movePose.magnitude > 0.1)
        {
            rb.isKinematic = false;
            LaraAnim.SetBool("IdleBool", false);
            LaraAnim.SetBool("RunBool", true);
        }
        else
        {
            rb.isKinematic = true;
            LaraAnim.SetBool("RunBool", false);
            LaraAnim.SetBool("IdleBool", true);
        }
        gameObject.transform.Translate(movePose*playerSpeed*Time.deltaTime);
    }
}
