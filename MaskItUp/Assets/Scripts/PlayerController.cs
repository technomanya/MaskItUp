using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager GM;
    public Camera mainCam;

    [Header("Mask Gun Attributes")]
    public float RotateAngle;
    public float GunAngle;
    public GameObject gun;
    public GameObject throwingMaskRef;
    public GameObject sabitMask;
    private GameObject gunObj;

    public GameObject[] throwingMasks;
    public int throwMaskCount = 0;
    private List<GameObject> throwMaskList = new List<GameObject>();
    private GameObject CurrentMask;
    private GameObject BeforeMask;
    public bool _thrown;
    private int maskId = 0;
    public float _mouseX, _mouseY, _mouseZ;
    public float ForwardForce;

    public Animator Hand;

    public GameObject[] Enemies;
    private Transform nearestEnemy;
    public List<Transform> nearEnemyList;

    private Vector3 lookAtVector;
    public float counter;


    // Start is called before the first frame update
    void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < throwMaskCount; i++)
        {
            var tempThrowMask = Instantiate(throwingMaskRef, Vector3.zero,
                Quaternion.identity);
            throwMaskList.Add(tempThrowMask);
        }
        throwingMasks =throwMaskList.ToArray();
    }
    void Start()
    {
        counter = 0.0f;
        lookAtVector = Vector3.forward;
        var posMaskOneScale = throwingMaskRef.transform.localScale;
        var posMaskOnePos = throwingMaskRef.transform.localPosition;
        var posZ = 0.5f;
        gunObj = GameObject.FindGameObjectWithTag("GunPivot");
        foreach (var mask in throwingMasks)
        {
            mask.transform.parent = gunObj.transform;
            mask.transform.localPosition = new Vector3(posMaskOnePos.x, posMaskOnePos.y, posMaskOnePos.z - posZ);
            mask.transform.localScale = posMaskOneScale;
            mask.SetActive(false);
        }

        maskId = 0;
        CurrentMask = throwingMasks[maskId];
        _thrown = false;
        nearEnemyList = new List<Transform>();
        nearestEnemy = null;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(nearEnemyList.Count > 0)
    //    {
    //        nearestEnemy = nearEnemyList[0];
    //        if(nearestEnemy != null)
    //        {
    //            var dir = nearestEnemy.position - transform.position;
    //            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
    //                Time.deltaTime * RotateAngle);
    //        }
    //    }

    //    //gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Horizontal")*RotateAngle);

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
    //        Debug.Log(ray.direction);
            
    //        _mouseX = ray.direction.x;
    //        _mouseY = ray.direction.y;
    //        _mouseZ = ray.direction.z;

    //        //Debug.Log("Y: " + ray.direction.y + " X: " + ray.direction.x + " Z: " + ray.direction.z);
    //        //gun.transform.LookAt(mousePosDiff, Vector3.up);
    //        gun.transform.localEulerAngles = new Vector3(_mouseY * -GunAngle, _mouseX * GunAngle, 0);

    //        counter = Time.timeSinceLevelLoad;
    //        _thrown = true;
    //        Throw();
            

    //        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        //{
    //        //    if (hit.collider.CompareTag("Enemy"))
    //        //    {
    //        //        hit.collider.GetComponent<EnemyController>().MaskUp();
    //        //    }
    //        //    else if (hit.collider.CompareTag("Friend"))
    //        //    {
    //        //        Debug.Log("WRONG!");
    //        //    }
    //        //    Debug.Log("Did Hit");
    //        //}

    //    }
    //}

    void FixedUpdate()
    {
        if(_thrown)
        {
            sabitMask.SetActive(false);
            if (Time.timeSinceLevelLoad-counter > 0.3f)
            {
                _thrown = false;
                counter = 0;
            }
            
        }
        if(!_thrown)
            sabitMask.SetActive(true);
    }       

    public void Throw(Vector3 direction)
    {
        Debug.Log(direction);
        BeforeMask = CurrentMask;
        var beginPos = BeforeMask.transform.position;

        BeforeMask.transform.parent = null;
        //BeforeMask.transform.localEulerAngles = gun.transform.localEulerAngles;

        CurrentMask.SetActive(true);
        sabitMask.SetActive(false);
        Hand.SetTrigger("Shoot");
        BeforeMask.GetComponent<Rigidbody>().isKinematic = false;
        BeforeMask.GetComponent<Rigidbody>().AddForce(direction * ForwardForce, ForceMode.VelocityChange);


        if (maskId >= throwingMasks.Length - 1)
        {
            maskId = 0;
            CurrentMask = throwingMasks[maskId];
        }
        else
        {
            maskId++;
            CurrentMask = throwingMasks[maskId];
        }

        sabitMask.SetActive(true);
        StartCoroutine(MaskReset(BeforeMask, beginPos));
    }

    IEnumerator MaskReset(GameObject maskCurr, Vector3 restPos)
    {
        yield return new WaitForSeconds(5.0f);
        maskCurr.GetComponent<Rigidbody>().isKinematic = true;
        maskCurr.transform.parent = gunObj.transform;
        maskCurr.transform.localPosition = throwingMaskRef.transform.localPosition;
        maskCurr.transform.localRotation = Quaternion.identity;
        maskCurr.transform.localScale = throwingMaskRef.transform.localScale;
        maskCurr.SetActive(false);
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.CompareTag("Enemy"))
            GM.GameOver(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            nearEnemyList.Add(other.transform);
    }
}
