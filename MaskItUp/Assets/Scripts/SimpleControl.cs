using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControl : MonoBehaviour
{
    private bool yon = true;

    public float force;

    public float speed;

    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.x >= 3.5f)
        //    yon = true;
        //else if (transform.position.x <= -3.5f)
        //    yon = false;

        //if (!yon)
        //    gameObject.transform.Translate(Vector3.right * Time.deltaTime * force);
        //else
        //{
        //    gameObject.transform.Translate(Vector3.left * Time.deltaTime * force);
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    transform.parent = null;
        //    GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*force, ForceMode.Impulse);
        //}

        var rotX = Input.GetAxis("Horizontal");
        var moveForward = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward*moveForward*speed*Time.deltaTime);
        transform.Rotate(Vector3.up, angle * rotX * Time.deltaTime);

    }
}
