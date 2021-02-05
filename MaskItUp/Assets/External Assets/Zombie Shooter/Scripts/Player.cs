using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public static Vector2 Player_Pos;//player position (x,y)
    public static bool go;// for PlayerShooting(stabilization) and touch control - is the character goes?

    public Rigidbody2D Body;//Player body
    public GameObject Blood1;//for enemy bullet ("EnBul 1")
    public GameObject BloodParticle;
    public AudioClip bonus;//bonus sound
    public static float Move_Angle;//used in PlayerAIM, if player has not target
    public Transform Move_Arrow;//arrow on the joystick
    public static float speed = 2.0f;//move speed
    public static float maxHP = 100f;
    public float regen = 1.0f;//HP regeneration per second
    public static float delay = 1.0f;//Delay after receive damage, before regeneration (seconds)

    public static float HP;//current HP
    private AudioSource audio;
    private Vector2 direction;
    public static float delayTimer;//current delay
    public Joystick jStick;
	void Start () {
        HP = maxHP;
        audio = GetComponent<AudioSource>();
	}
	

	void Update () {
        go = false;//zero out to a stationary position
        Player_Pos = new Vector2(transform.position.x, transform.position.y);//player position (x,y)

        if (delayTimer <= 0 & HP < maxHP) { HP += regen * Time.deltaTime; }//HP regeneration
        else if(delayTimer > 0) delayTimer -= Time.deltaTime;//Decrease delay
        if (HP >= maxHP) HP = maxHP;//HP should not exceed a maximum

        #region Keyboard
        if (Input.GetKey(KeyCode.W))//UP
        {
            Body.AddForce(new Vector2(0, speed / (1 + 0.1f * (PlayerShooting.weight)) * 70 * Time.deltaTime));
            go = true;
            Move_Angle = 0;
        }
        if (Input.GetKey(KeyCode.S))//DOWN
        {
            Body.AddForce(new Vector2(0, -speed / (1 + 0.1f * (PlayerShooting.weight)) * 70 * Time.deltaTime));
            go = true;
            Move_Angle = 180;
        }
        if (Input.GetKey(KeyCode.A))//LEFT
        {
            Body.AddForce(new Vector2(-speed / (1 + 0.1f * (PlayerShooting.weight)) * 70 * Time.deltaTime, 0));
            go = true;
            Move_Angle = 90;
        }
        if (Input.GetKey(KeyCode.D))//RIGHT
        {
            Body.AddForce(new Vector2(speed / (1 + 0.1f * (PlayerShooting.weight)) * 70 * Time.deltaTime, 0));
            go = true;
            Move_Angle = -90;
        }
        #endregion

        for (int i = 0; i < Input.touchCount; ++i)//Touch
        {
            Vector2 pos = new Vector2(Input.GetTouch(i).position.x / Screen.width, Input.GetTouch(i).position.y / Screen.height);//touch position(x,y) (0-1)
            if (pos.x < MainMenu.joystickSize & pos.y < MainMenu.joystickSize*1.5f)//joystick size
            {
                //direction from the center of of joystick to the touch point:
                direction = new Vector2(pos.x - MainMenu.joystickSize*0.5f, pos.y - MainMenu.joystickSize*0.75f);//half size of joystick (its center)
                //(don't forget to change it, if changed size of the joystick)
                go = true;
            }
        }


        if (Input.touchCount > 0)//only for touchscreen
        {
            Move_Angle = Vector2.Angle(Vector2.up, direction / direction.magnitude);//angle of the character movement
            if (direction.x > 0) { Move_Angle = 360 - Move_Angle; }
        }
        
	}
    void LateUpdate() {
        Move_Arrow.rotation = Quaternion.Euler(0, 0, Move_Angle);//rotation arrow on the joystick
    }

    void FixedUpdate() {
        if (go & Input.touchCount > 0)//only for touchscreen
            Body.AddForce(direction / direction.magnitude * speed/(1+0.1f*(PlayerShooting.weight)));//divide the vector by its length to get the angle
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnBul"))//if collision with Enemy Bullet
        {
            Instantiate(BloodParticle, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + 180));
            if (col.gameObject.name.Contains("EnBul 1"))//collision with Enemy Bullet 1
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood1, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15) + 180));
                HP -= 10f;//damage

                if (Player.HP <= 0)//player death
                {
                    PlayerPrefs.SetInt("money", MainMenu.MONEY);//SAVE money ON DEATH
                    Application.LoadLevel(0);//load menu
                }
            }
        }
        if (col.gameObject.CompareTag("Bonus"))//collision with any bonus
        {
            if (col.gameObject.name.Contains("HP"))//HP bonus
            {
                if (HP < maxHP)//if less than the maximum
                {
                    Destroy(col.gameObject);//destroy bonus
                    audio.PlayOneShot(bonus, MainMenu.volume*0.5f);//play sound
                    HP += 20;//increase HP
                    if (HP >= maxHP) HP = maxHP;//HP should not exceed a maximum
                }
            }
            if (col.gameObject.name.Contains("Ammo"))//Ammo bonus
            {
                if (PlayerShooting.curAmmo < PlayerShooting.Ammo)//if less than the maximum
                {
                    Destroy(col.gameObject);//destroy bonus
                    audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                    PlayerShooting.curAmmo += Mathf.RoundToInt(PlayerShooting.Ammo * 0.25f);//+ a quarter of the maximum ammo
                    if (PlayerShooting.curAmmo >= PlayerShooting.Ammo) PlayerShooting.curAmmo = PlayerShooting.Ammo;//Ammo should not exceed a maximum
                }
            }
            if (col.gameObject.name.Contains("Turret"))//Turret bonus
            {
                if (PlayerShooting.turret == false)//if has no turret
                {
                    Destroy(col.gameObject);//destroy bonus
                    audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                    PlayerShooting.turret = true;//give turret
                }
            }
            if (col.gameObject.name.Contains("Mine"))//Mine bonus
            {
                if (PlayerShooting.curmine < PlayerShooting.mines)//if less than the maximum
                {
                    Destroy(col.gameObject);//destroy bonus
                    audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                    PlayerShooting.curmine += 1;//increase Mines
                }
            }
            if (col.gameObject.name.Contains("Money"))//money bonus
            {

                Destroy(col.gameObject);//destroy bonus
                audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                MainMenu.MONEY += 5;//reward
            }
        }
        if (col.gameObject.CompareTag("Trigger"))
        {
            Destroy(col.gameObject);//destroy trigger on collision with player
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Box"))//collision with Box
        {
            if (col.gameObject.name.Contains("HP"))//HPBox
            {
                audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                HP = maxHP;//restore all HP
            }
            if (col.gameObject.name.Contains("Ammo"))//AmmoBox
            {
                audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                PlayerShooting.curAmmo = PlayerShooting.Ammo;//restore all Ammo
            }
            if (col.gameObject.name.Contains("Device"))//AmmoBox
            {
                audio.PlayOneShot(bonus, MainMenu.volume * 0.5f);//play sound
                PlayerShooting.turret = true;//restore turret
                PlayerShooting.curmine = PlayerShooting.mines;//restore all Mines
            }
        }
    }
}
