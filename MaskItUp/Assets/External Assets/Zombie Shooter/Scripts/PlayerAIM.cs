using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAIM : MonoBehaviour
{

    public PlayerController pControll;
    public ThirdPersonController playerTPC;
    public GameObject mask;
    public bool shooting = false;
    public float ForwardForce;
    public List<Transform> nearEnemyList;
    private Transform nearestEnemy;
    public float RotateAngle;
    public static bool HasTarget = false;//it used in PlayerShooting (shooting only if player has target)
    private Vector3 nearDistance ;

    [SerializeField]private float FireRate = 10;  // The number of bullets fired per second
    private float lastfired = 0f;      // The value of Time.time at the last firing moment

	void Start () {
        //Marker.SetActive(false);//marker deactivation
	}
	

	void Update () {

        if (shooting && HasTarget)
        {
            if (Time.time - lastfired > FireRate)
            {
                lastfired = Time.time;
                pControll.counter = lastfired;
                pControll._thrown = true;
                pControll.Throw(nearDistance);
                //var currMask = Instantiate(mask, mask.transform.position, mask.transform.rotation);
                //currMask.SetActive(true);
                //currMask.GetComponent<Rigidbody>().AddForce(Vector3.forward*ForwardForce, ForceMode.VelocityChange);
            }
        }

        if (playerTPC.movePose.magnitude > 0.1)
            shooting = false;
        else
            shooting = true;


        //find the nearest Enemy
        if (nearEnemyList.Count > 0)
        {
            HasTarget = true;
            nearestEnemy = nearEnemyList[0];
            if (nearestEnemy != null)
            {
                var dir = nearestEnemy.position - transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
                    Time.deltaTime * RotateAngle);
                nearDistance = nearestEnemy.transform.position - transform.position;
            }
        }
        else
        {
            shooting = false;
            HasTarget = false;
            var dir = playerTPC.movePose;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir),
                Time.deltaTime * RotateAngle);
        }

	}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            nearEnemyList.Add(other.transform);
    }

    void OnTriggerExit(Collider other)
    {
        nearEnemyList.Remove(other.transform);
    }

    //IEnumerator MaskReset(GameObject maskCurr, Vector3 restPos)
    //{
    //    yield return new WaitForSeconds(5.0f);
    //    maskCurr.GetComponent<Rigidbody>().isKinematic = true;
    //    maskCurr.transform.parent = transform;
    //    maskCurr.transform.localPosition = throwingMaskRef.transform.localPosition;
    //    maskCurr.transform.localRotation = Quaternion.identity;
    //    maskCurr.transform.localScale = throwingMaskRef.transform.localScale;
    //    maskCurr.SetActive(false);
    //}
}
