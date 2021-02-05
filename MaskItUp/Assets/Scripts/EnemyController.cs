using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject mask;

    [SerializeField] private NavMeshAgent navA;

    public Transform Target;
    public GameManager GM;
    public PlayerAIM player;
    public Transform Back;

    // Start is called before the first frame update
    void Start()
    {

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("GunPivot").GetComponent<PlayerAIM>();

        Target = GameObject.FindGameObjectWithTag("Player").transform;
        navA = GetComponent<NavMeshAgent>();
        
        Back = GameObject.FindGameObjectWithTag("Back").transform;
    }

    // Update is called once per frame
    void Update()
    {
        navA.SetDestination(Target.position);
    }

    public void MaskUp()
    {
        if(mask.activeInHierarchy == false)
        {
            mask.SetActive(true);
            GM.MaskedEnemy++;
            navA.SetDestination(transform.position);
            player.nearEnemyList.Remove(gameObject.transform);
            StartCoroutine(WaitSec());
        }
    }

    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(1);
        gameObject.transform.position = Back.position;
        //navA.SetDestination(transform.position);
        Destroy(gameObject);
    }
}
