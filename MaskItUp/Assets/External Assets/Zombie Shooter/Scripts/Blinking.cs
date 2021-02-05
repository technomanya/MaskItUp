using UnityEngine;
using System.Collections;

public class Blinking : MonoBehaviour {//for mines
    public GameObject go;
    public float time;
    private float timer;//current time

	void Start () {
	
	}
	
	void Update () {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if(go.activeInHierarchy)go.SetActive(false);
            else go.SetActive(true);
            timer = time;
        }
	}
}
