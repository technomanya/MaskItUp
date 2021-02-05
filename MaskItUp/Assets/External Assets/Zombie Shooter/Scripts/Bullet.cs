using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float lifetime;
	public float speed;//(6-12)
    public GameObject DestroyParticle;//in contact with the wall

	void Start () {
        Destroy(gameObject, lifetime);//destroy over lifetime
    }

	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * speed);//bullet movement
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))//bullets do not cause damage to the player
        {
            Destroy(gameObject);//bullet destroy
        }
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);//bullet destroy and instantiate sparks effect
            Instantiate(DestroyParticle, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, 0));
        }
    }
}
