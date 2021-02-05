using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public Image cooldown;
    public Image reload;
    public Image HP;
    public Image Ammo;
    public Image Turret;
    public Image Mine;

	void Start () {
	
	}
	
	void Update () {
        if (PlayerShooting.curKD > 0) cooldown.fillAmount = (PlayerShooting.KD - PlayerShooting.curKD) / PlayerShooting.KD;//cooldown between shots
        else cooldown.fillAmount = 1;

        if (PlayerShooting.curReload > 0) reload.fillAmount = (PlayerShooting.Reload - PlayerShooting.curReload) / PlayerShooting.Reload;//reload cooldown
        else reload.fillAmount = (1.0f * PlayerShooting.curMagazine / PlayerShooting.Magazine);

        HP.fillAmount = Player.HP / Player.maxHP;//heath point bar
        Ammo.fillAmount = 1.0f * PlayerShooting.curAmmo / PlayerShooting.Ammo;//ammo bar

        if (PlayerShooting.turret) Turret.fillAmount = 1; else Turret.fillAmount = 0;//turret button
        Mine.fillAmount = 1.0f * PlayerShooting.curmine / PlayerShooting.mines;//mine button
	}
}
