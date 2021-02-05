using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextSize : MonoBehaviour {//proportional size of the text
    
    public int Size;//1-3 (coefficient)

	void Start () {
        GetComponent<Text>().fontSize = Mathf.FloorToInt(Screen.width / 100) * Size;//stretching the size of the text to the width of screen
	}
    void Update() {//for testing
        //GetComponent<Text>().fontSize = Mathf.FloorToInt(Screen.width/100) * Size;//dynamic stretching the size of the text to the width of screen
    }
}
