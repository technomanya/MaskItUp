using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
    public GameObject Bullet_1;//Pistol / AK / Machine Gun
    public GameObject Bullet_2;//Snipe
    public GameObject Bullet_3;//Shotgun
    public GameObject Bullet_4;//Crossbow

    public GameObject Turret_1;//(using)
    public GameObject Turret_2;//
    public GameObject Turret_3;//
    public GameObject Mine;//(using)

    public static int GUN = 1;//selected gun (Pistol, AK, Machine Gun, Snipe, Shotgun, Crossbow) (1-6)

    public GameObject RazbrosLineLeft;//left visual boundary of accuracy (2 green lines on the screen)
    public GameObject RazbrosLineRight;//Right visual boundary of accuracy
    public ParticleSystem Flash;//shot flash

    private AudioSource audio;
    public AudioClip reload;//reload sound
    public AudioClip shoot1;//shot sounds
    public AudioClip shoot2;//
    public AudioClip shoot3;//
    public AudioClip shoot4;//
    public AudioClip shoot5;//
    public AudioClip shoot6;//

    public static float KD;//interval between shots
    public static int Magazine;//bullets in magazine
    public static int Ammo;//all ammunition
    public static float Reload;//magazine reload time
    public static float weight;//
    public static float repulsion;//
    public float accuracy;//max accuracy (deviation from the center in degrees)
    public float shoot_razbros;//shot recoil
    public float destabilization;//
    public float svedenie;//aiming speed
    public float max_razbros;//min accuracy (deviation from the center in degrees) (during continuous shooting)

    private float Razbros;//current accuracy (deviation from the center in degrees)
    private float curStabil;//
    public static float curKD;//current timer of interval between shots
    public static int curMagazine;//current bullets in magazine
    public static int curAmmo;//current ammunition
    public static float curReload;//current time until the end of reload

    public static bool turret = true;//has turret?
    public static int mines = 3;//max count of mines
    public static int curmine;//current count of mines
	void Start () {
        if (GUN == 1)//Pistol specifications
        {
            accuracy = 5;
            KD = 0.2f;
            shoot_razbros = 4.0f;
            destabilization = 3;
            svedenie = 15f;
            max_razbros = 16;
            Magazine = 12;
            Ammo = 150;
            Reload = 0.7f;
            weight = 1.0f;
            repulsion = 5.0f;
        }
        if (GUN == 2)//AK specifications
        {
            accuracy = 4;
            KD = 0.12f;
            shoot_razbros = 2.0f;
            destabilization = 5;
            svedenie = 9f;
            max_razbros = 17;
            Magazine = 30;
            Ammo = 120;
            Reload = 2;
            weight = 3.0f;
            repulsion = 6.0f;
        }
        if (GUN == 3)//Mashine gun specifications
        {
            accuracy = 8;
            KD = 0.1f;
            shoot_razbros = 1.4f;
            destabilization = 4;
            svedenie = 8f;
            max_razbros = 18;
            Magazine = 100;
            Ammo = 200;
            Reload = 6;
            weight = 5.0f;
            repulsion = 6.0f;
        }
        if (GUN == 4)//Snipe specifications
        {
            accuracy = 2;
            KD = 1.2f;
            shoot_razbros = 12.0f;
            destabilization = 10;
            svedenie = 6.5f;
            max_razbros = 20;
            Magazine = 8;
            Ammo = 30;
            Reload = 2.5f;
            weight = 6.0f;
            repulsion = 30.0f;
        }
        if (GUN == 5)//Shotgun specifications
        {
            accuracy = 15;
            KD = 0.5f;
            shoot_razbros = 4.0f;
            destabilization = 3;
            svedenie = 7f;
            max_razbros = 20;
            Magazine = 2;
            Ammo = 40;
            Reload = 1.2f;
            weight = 1.5f;
            repulsion = 7.0f;
        }
        if (GUN == 6)//Crossbow specifications
        {
            accuracy = 3;
            KD = 0.5f;
            shoot_razbros = 3.0f;
            destabilization = 4;
            svedenie = 6f;
            max_razbros = 10;
            Magazine = 20;
            Ammo = 60;
            Reload = 2.5f;
            weight = 2.0f;
            repulsion = 15.0f;
        }

        audio = GetComponent<AudioSource>();
        //resets:
        curMagazine = Magazine;
        curAmmo = Ammo;
        curReload = 0;
        curmine = mines;
        turret = true;
	}
	

	void Update () {

        if (Player.go)//
            if (curStabil < destabilization) curStabil += destabilization * Time.deltaTime * 1.5f;
            else curStabil = destabilization;
        else curStabil = 0;

        if (curKD > 0) { curKD -= Time.deltaTime; }//decrease current timer of interval between shots, if needed
        if (curReload > 0) { curReload -= Time.deltaTime; }//decrease current time until the end of reload, if needed
        else if (curMagazine <= 0) curMagazine = Magazine;//magazine filling after reload
        if (Razbros >= accuracy + curStabil) { Razbros -= Time.deltaTime * svedenie; }//aiming (decrease deviation from the center in degrees)
        else {
            Razbros = accuracy + curStabil;//max accuracy
        }

        //RAYS DEACTIVATION (accuracy visualization)
        /*if (PlayerAIM.HasTarget == false)
        {
            RazbrosLineLeft.SetActive(false);//disable visual boundary of current accuracy, if player hasn't target
            RazbrosLineRight.SetActive(false);
        }*/

        #region Keyboard
        if (Input.GetKey(KeyCode.KeypadEnter) | Input.GetKey(KeyCode.P) | Input.GetKey(KeyCode.Space)) { Shoot(); }//shot
        if (Input.GetKeyDown(KeyCode.R)) { Rld(); }//reload

        if (Input.GetKeyDown(KeyCode.E) & turret){//turret
            Instantiate(Turret_1, Player.Player_Pos, Quaternion.Euler(0, 0, 0));
            turret = false;
        }
        if (Input.GetKeyDown(KeyCode.Q) & curmine > 0){//mine
            Instantiate(Mine, Player.Player_Pos, Quaternion.Euler(0, 0, 0));
            curmine -= 1;
        }
        #endregion

        for (int i = 0; i < Input.touchCount; ++i)//touch control
        {
            Vector2 pos = new Vector2(Input.GetTouch(i).position.x / Screen.width, Input.GetTouch(i).position.y / Screen.height);//touch position(x,y) (0-1)
            if (pos.x > 0.8f & pos.y < 0.3f)//stay touch
            {
                Shoot();
            }

            if (Input.GetTouch(i).phase == TouchPhase.Began)//one touch
            {
                if (pos.x > 0.8f & pos.y > 0.3f & pos.y < 0.5f)
                {
                    Rld();//reload
                }
                if (pos.x > 0.7f & pos.x < 0.8f & pos.y < 0.15f & turret)//turret (if has turret)
                {
                    Instantiate(Turret_1, Player.Player_Pos, Quaternion.Euler(0, 0, 0));//spawn turret
                    turret = false;//remove the turret
                }
                if (pos.x > 0.6f & pos.x < 0.7f & pos.y < 0.15f & curmine > 0)//mine (if has mine)
                {
                    Instantiate(Mine, Player.Player_Pos, Quaternion.Euler(0, 0, 0));//spawn mine
                    curmine -= 1;//decrease mines count
                }
            }
        }

        //set angle of visual boundary of accuracy
        RazbrosLineLeft.transform.localRotation = Quaternion.AngleAxis(Razbros, Vector3.forward);
        RazbrosLineRight.transform.localRotation = Quaternion.AngleAxis(-Razbros, Vector3.forward);
	}
    void Shoot()
    {
        RazbrosLineLeft.SetActive(true); //make visible at a shot
        RazbrosLineRight.SetActive(true);

        if (PlayerAIM.HasTarget & curAmmo > 0 & curMagazine > 0 & curKD <= 0)//shot if has: target, bullet, bullet in magazine and if interval has elapsed
        {
            if (GUN == 1)//Pistol shot
            {
                audio.PlayOneShot(shoot5, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullet spawn:
                Instantiate(Bullet_1, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-Razbros, Razbros)));
            }
            if (GUN == 2)//AK shot
            {
                audio.PlayOneShot(shoot1, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullet spawn:
                Instantiate(Bullet_1, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-Razbros, Razbros)));
            }
            if (GUN == 3)//Mashine gun shot
            {
                audio.PlayOneShot(shoot1, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullet spawn:
                Instantiate(Bullet_1, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-Razbros, Razbros)));
            }
            if (GUN == 4)//Snipe shot
            {
                audio.PlayOneShot(shoot4, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullet spawn:
                Instantiate(Bullet_2, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-Razbros, Razbros)));
            }
            if (GUN == 5)//Shotgun shot
            {
                audio.PlayOneShot(shoot3, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullets spawn:
                Instantiate(Bullet_3, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - Razbros));
                Instantiate(Bullet_3, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - Razbros*0.5f));
                Instantiate(Bullet_3, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Razbros));
                Instantiate(Bullet_3, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Razbros * 0.5f));
                Instantiate(Bullet_3, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
            }
            if (GUN == 6)//Crossbow shot
            {
                audio.PlayOneShot(shoot6, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                //bullet spawn:
                Instantiate(Bullet_4, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-Razbros, Razbros)));
            }

            if (GUN != 6) Flash.Play(true);//flash at a shot, except a crossbow
            curMagazine -= 1;//decrease bullets in magazine
            curAmmo -= 1;//decrease all ammunition
            curKD = KD;//reset current interval between shots
            Razbros += shoot_razbros;//add recoil to the current accuracy
            if (Razbros > max_razbros + accuracy) Razbros = max_razbros;//deviation shall not exceed the maximum
            if (curMagazine <= 0 & curReload <= 0) { curReload = Reload; audio.PlayOneShot(reload, MainMenu.volume); }//start reload, if bullets ended
        }
        
    }
    void Rld()
    {
        if (curMagazine < Magazine & curMagazine > 0)//if magazine is not full and reload has not started yet
        {
            audio.PlayOneShot(reload, MainMenu.volume);//reload sound
            curReload = Reload;//reset reload timer
            curMagazine = 0;//empty magazine
        }
    }
}
