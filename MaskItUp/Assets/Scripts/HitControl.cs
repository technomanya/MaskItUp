using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitControl : MonoBehaviour
{
    [SerializeField] private GameObject Body;
    
    // Start is called before the first frame update
    void Start()
    {
        Body = transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowMask"))
        {
            Body.GetComponent<EnemyController>().MaskUp();
        }
    }
}
