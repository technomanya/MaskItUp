using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Rigidbody2D Body;//Enemy body
    private Transform Target;//Player transform
    private Vector2 PlayerLastPos = new Vector2(0, 0);//player position where he was visible to the enemy for the last time
    private bool follow = false;//If the enemy has ceased to see the player, he will follow on PlayerLastPos

    public float HP;// Horse Power, of course
    public float Speed;
    public int reward;//money reward for killing

    //this for the rifleman ("Enemy 2") ((zombie with a gun))
    public bool rifleman = false;//("Enemy 2")
    public GameObject bullet;
    public Transform ShootPoint;// bullets spawn point
    public float cooldown = 0.5f;//interval between shots
    private float curCD;//timer
    public int Magazine = 5;//Bulets in magazine (or in shooting bursts)
    private int curMagazine = 5;//counter
    public float Reload = 1;//magazine reload time (or interval between bursts)
    private float curReload;//current time until the end of reload
    private AudioSource audio;
    public AudioClip shoot;//shot sounds

    //blood sprites (GameObjects)
    public GameObject Blood1;//for bullet_1 (pistol, AK, machine gun)
    public GameObject Blood2;//for bullet_2 (sniper rifle, crossbow)
    public GameObject Blood3;//for bullet_3 (shotgun)
    public GameObject BloodDead;//here you can use the corpse

    //particles (GameObjects)
    public GameObject BloodParticle;//for all bullets
    public GameObject Explosion;//used on collision whith mine

    private RaycastHit2D hit;//for the visibility system
    public float curHP;//current HP

    void Start () {
        curHP = HP;
        audio = GetComponent<AudioSource>();
	}
	

	void Update () {
        //Reduction of weapon timers and loading magazine
        if (rifleman) { curCD -= Time.deltaTime; curReload -= Time.deltaTime; if (curReload <= 0 & curMagazine <= 0) curMagazine = Magazine; }

        //check of reaching the last pos 
        if(follow & Vector2.Distance(PlayerLastPos, transform.position) <= 0.1f) follow = false;

        Target = GameObject.FindWithTag("Player").transform;//find player
        hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)Player.Player_Pos - (Vector2)transform.position, 5, 1025);//a ray from the enemy to the player
        if (hit.collider != null)
        {
            if (!hit.collider.gameObject.CompareTag("Wall"))//ray does not touch the walls
            {
                //follow the target
                Vector3 moveDirection = Target.transform.position - transform.position;
                if (moveDirection != Vector3.zero)
                {
                    float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                    PlayerLastPos = Target.position;//update last position when the enemy sees the player
                    follow = true;

                    if (rifleman & curCD <= 0 & curMagazine > 0)//shot if all conditions are met
                    {
                        audio.PlayOneShot(shoot, MainMenu.volume);//shot sound (float in parenthesis indicates the volume)
                        Instantiate(bullet, ShootPoint.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-2, 2)));//bullet spawn
                        curCD = cooldown;//reset current interval between shots
                        curMagazine -= 1;//decrease bullets in magazine
                        if (curMagazine <= 0) curReload = Reload;//start reload, if bullets ended
                    }
                }
                //show rays:
                //Debug.DrawLine((Vector2)transform.position, (Vector2)Player.Player_Pos);
            }
            else if(follow)
            {//follow the last target position
                Vector3 moveDirection = new Vector3 (PlayerLastPos.x, PlayerLastPos.y, 0) - transform.position;
                if (moveDirection != Vector3.zero)
                {
                    float angle = Mathf.Atan2(-moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                }
            }
        }
	}

    void FixedUpdate()
    {
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))//going, if the enemy sees the player
            {
                Body.AddRelativeForce(new Vector2(Speed, 0));
            }
            
        }
        if (follow)//going, if the enemy dont sees the player (twice slower)
            {
                Body.AddRelativeForce(new Vector2(Speed*0.5f, 0));
            }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet")) {//on collision with any bullet

            Instantiate(BloodParticle, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + 180));

            //PLAYER BULLETS
            if (col.gameObject.name.Contains("Bullet 1"))//collision with bullet 1
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood1, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15) + 180));
                curHP -= 10f;//damage
            }
            if (col.gameObject.name.Contains("Bullet 2"))//collision with bullet 2
            {
                //instantiation blood sprite
                Instantiate(Blood2, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + 180));
                curHP -= 50f;//damage
            }
            if (col.gameObject.name.Contains("Bullet 3"))//collision with bullet 3
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood3, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15) + 180));
                curHP -= 15f;//damage
            }
            if (col.gameObject.name.Contains("Bullet 4"))//collision with bullet 4
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood2, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + 180));
                curHP -= 30f;//damage
            }

            //TURRET BULLETS
            if (col.gameObject.name.Contains("Bullet Turret 1"))//collision with bullet Turret 1
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood1, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15) + 180));
                curHP -= 8f;//damage
            }
            else if (col.gameObject.name.Contains("Bullet Turret 2"))//collision with bullet Turret 2
            {
                Destroy(col.gameObject);//bullet destroy
                //instantiation blood sprite
                Instantiate(Blood1, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z + Random.Range(-15, 15) + 180));
                curHP -= 20f;//damage
            }

            //repulsion on collision with PLAYER bullets:
            else { Body.AddRelativeForce (new Vector2(-PlayerShooting.repulsion, 0)); }

            Death();//death test
        }
        if (col.gameObject.CompareTag("Trap"))//collision with trap(mine)
        {
            //instantiation explosion particles
            Instantiate(Explosion, new Vector2(col.transform.position.x, col.transform.position.y), Quaternion.Euler(0, 0, col.transform.rotation.eulerAngles.z));
            Destroy(col.gameObject);//trap destroy
            curHP -= 80f;//damage
            Death();//death test
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))//collision stay with player
        {
            Player.HP -= 1;//damage to the player
            Player.delayTimer = Player.delay;//Update delay for regeneration
            if (Player.HP <= 0)//player death
            {
                PlayerPrefs.SetInt("money", MainMenu.MONEY);//SAVE money ON DEATH
                Application.LoadLevel(0);//load menu
            }
        }
    }
    void Death() {
        if (curHP <= 0)//checking health points
        {
            Destroy(gameObject);//destroy enemy
            //instantiation death blood sprite
            Instantiate(BloodDead, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
            Finish.currentKills += 1;//counter dead enemies
            MainMenu.MONEY += reward;//reward accruing
        }
    }
}
