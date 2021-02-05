using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public GameObject trigger;//(key) At trigger destroy - the door will open
    public GameObject locked;//Skin of the closed door
    public GameObject unlocked;//Skin of the opened door

    void Update()
    {
        if (trigger == null)//Trigger destroyed
        {
            locked.SetActive(false);
            unlocked.SetActive(true);
        }
    }
}
