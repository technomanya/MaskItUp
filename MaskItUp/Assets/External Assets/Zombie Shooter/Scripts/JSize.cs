using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JSize : MonoBehaviour {
    //This script should be placed in the elements of the joystick

    private Image I;//(Center, background and arrow of the joystick)

    void Start () {
        I = gameObject.GetComponent<Image>();
        I.rectTransform.anchorMax = new Vector2(MainMenu.joystickSize, MainMenu.joystickSize * 1.5f);//set size (configured in the main menu)
	}
}
