using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TEXTS : MonoBehaviour{//Used on level 1 for showing tips

    private float vis_dist = 1.5f;//visible distanse
	
	void Update () {
        float dist = Vector3.Distance(transform.position, Player.Player_Pos);//distance to the player
        if (dist < vis_dist & dist > 0.5f * vis_dist) GetComponent<TextMesh>().characterSize = 0.2f * (1.0f - (dist / vis_dist));//size changing
        else if (dist < 0.5f * vis_dist) GetComponent<TextMesh>().characterSize = 0.1f;//max size applying
        else GetComponent<TextMesh>().characterSize = 0;// 0 size
	}
}
