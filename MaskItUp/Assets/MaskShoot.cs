using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskShoot : MonoBehaviour
{
    public float ForwardForce;
    [SerializeField]private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Vector3.forward * ForwardForce, ForceMode.VelocityChange);
    }

    void OnEnable()
    {
        rb.AddForce(Vector3.forward*ForwardForce, ForceMode.VelocityChange);
    }
}
