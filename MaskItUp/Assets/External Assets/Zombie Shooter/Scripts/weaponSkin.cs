using UnityEngine;
using System.Collections;

public class weaponSkin : MonoBehaviour {
    public GameObject Pistol;
    public GameObject AK;
    public GameObject MG;
    public GameObject Snipe;
    public GameObject Shotgun;
    public GameObject Crossbow;
    //public GameObject NewSkin;//for new gun

    void Start()
    {//activation of the desired skin (skins should be deactivated)
        if (PlayerShooting.GUN == 1) Pistol.SetActive(true);
        if (PlayerShooting.GUN == 2) AK.SetActive(true);
        if (PlayerShooting.GUN == 3) MG.SetActive(true);
        if (PlayerShooting.GUN == 4) Snipe.SetActive(true);
        if (PlayerShooting.GUN == 5) Shotgun.SetActive(true);
        if (PlayerShooting.GUN == 6) Crossbow.SetActive(true);
        //if (PlayerShooting.GUN == 7) NewSkin.SetActive(true);//for new gun
	}
}
