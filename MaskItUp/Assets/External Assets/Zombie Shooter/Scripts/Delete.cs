using UnityEngine;
using System.Collections;

public class Delete : MonoBehaviour {
    public float timer;//Destroy GameObject over time

	void Start () {
		if (timer!=0) Destroy (gameObject, timer);
	}
}