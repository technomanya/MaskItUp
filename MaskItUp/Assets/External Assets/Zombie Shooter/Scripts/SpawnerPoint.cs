using UnityEngine;
using System.Collections;

public class SpawnerPoint : MonoBehaviour {
    public float emergence;//start delay
    public float spawn_time;//spawn interval
    public int limit;//enemies in spawner (or bonuses)
    public GameObject trigger;//activating trigger (can be not defined)
    public GameObject Enemy;//(or bonus)

    private float timer;//current interval

    void Update()
    {
        if (trigger == null)// if trigger activated OR not defined
        {
            if (emergence > 0) emergence -= Time.deltaTime;//start delay
            else if (timer > 0) timer -= Time.deltaTime;//ELSE decrease interval timer
            else if (limit > 0)//ELSE if spawner has enemies - spawn enemy:
            {
                Instantiate(Enemy, new Vector2(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f)), transform.rotation);//spawn
                timer = spawn_time;//reset spawn timer
                limit -= 1;//decrease spawner enemy count 
            }
        }
    }
}
