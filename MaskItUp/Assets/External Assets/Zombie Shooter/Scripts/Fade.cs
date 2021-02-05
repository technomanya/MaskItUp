using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
    public SpriteRenderer sprite;//fading sprite
	public float speed=1;//fade speed
    public float wait = 0;//delay

    private float waittimer;//current delay timer
    public bool destroy = true;//destroy after fade?

	void OnEnable () {
        var color = sprite.color;
        color.a = 1;//transparency (0-1)
        sprite.color = color;
        waittimer = wait;
	}

	void Update () {
        waittimer -= Time.deltaTime;//delay timer
        if (waittimer <= 0)//after delay
        {
			var color = sprite.color;
            color.a -= speed * Time.deltaTime;//fading
            color.a = Mathf.Clamp(color.a, 0, 1);//color boundaries assignment (0-1)
            sprite.color = color;//applying color to sprite
			if (color.a<=0 & destroy) Destroy(gameObject);//destroy after fade
		}
	}
}
