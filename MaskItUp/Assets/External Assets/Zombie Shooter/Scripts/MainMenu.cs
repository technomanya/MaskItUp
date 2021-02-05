using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public static int MONEY;
    public static int LEVELS;//number of the last unlocked level
    public static float joystickSize;//setting the size of the joystick
    public Image joy;//joystick button (resizes when setting)
    public static float volume;//the volume of all sounds
    public Image vol;//volume button (changes color when setting)

    public int slide = 2;//tab number - settings(1), weapons(2), levels(3)
    public GameObject canvas1;//settings screen
    public GameObject canvas2;//weapons screen
    public GameObject canvas3;//levels screen
    //tab selection buttons:
    public Image s1;
    public Image s2;
    public Image s3;

    public Image selected;//illuminated background of the selected button (weapon, level)
    public GameObject lines;//common for weapons and levels decor. Is disabled on the settings tab

    public GameObject moneytext;
    public AudioClip select;//clicking sound
    public AudioClip select2;//clicking sound(slides)


    //GUN sprites:
    public Image a1;//pistol
    public Image a2;//ak
    public Image a3;//mashine gun
    public Image a4;//snipe
    public Image a5;//shotgun
    public Image a6;//crossbow
    //public Image a7;//new gun

    //LEVEL sprites:
    public Image L1;
    public Image L2;
    public Image L3;
    public Image L4;
    public Image L5;
    public Image L6;
    //public Image L7;//new level

    //Guns prices
    public int price1 = 0;//pistol
    public int price2 = 100;//ak
    public int price3 = 200;//mashine gun
    public int price4 = 350;//snipe
    public int price5 = 400;//shotgun
    public int price6 = 500;//crossbow
    //public int price7 = 100500;//new gun price

    public static int Level=1;//selected level
    private float x;//screen size
    private float y;
    private AudioSource audio;
    public GUIStyle Lite;//invisible buttons

	void Start () {
        x = Screen.width;//screen size
        y = Screen.height;
        
        audio = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("money"))//load money
            MONEY = PlayerPrefs.GetInt("money");
        else MONEY = 0;//default money=0

        if (PlayerPrefs.HasKey("levels"))//load levels
            LEVELS = PlayerPrefs.GetInt("levels");
        else LEVELS = 1;//default levels=1

        if (PlayerPrefs.HasKey("sound"))//load volume
            volume = PlayerPrefs.GetFloat("sound");
        else volume = 1;//default volume=1

        if (PlayerPrefs.HasKey("joystick"))//load joystick Size
            joystickSize = PlayerPrefs.GetFloat("joystick");
        else joystickSize = 0.3f;//default joystickSize=0.3f

        moneytext.GetComponent<Text>().text = MONEY.ToString();//show money
        Colorise();//set buttons colors
	}
	
    void OnGUI() {
        //START
        if (GUI.Button(new Rect(x * 0.8f, y * 0.8f, x * 0.2f, y * 0.2f), "", Lite))//button position & size (X,Y) (0-1)
        {
            Application.LoadLevel(Level);//load selected level
        }

        //tab selection buttons:
        if (GUI.Button(new Rect(x * 0.2f, y * 0.8f, x * 0.2f, y * 0.2f), "", Lite))//settings
        {
            slide = 1;
            Colorise();
            audio.PlayOneShot(select2, volume);//play sound
        }
        if (GUI.Button(new Rect(x * 0.4f, y * 0.8f, x * 0.2f, y * 0.2f), "", Lite))//weapons
        {
            slide = 2;
            Colorise();
            audio.PlayOneShot(select2, volume);//play sound
        }
        if (GUI.Button(new Rect(x * 0.6f, y * 0.8f, x * 0.2f, y * 0.2f), "", Lite))//levels
        {
            slide = 3;
            Colorise();
            audio.PlayOneShot(select2, volume);//play sound
        }

        if (slide == 1)
        {
            //Delete saves
            if (GUI.Button(new Rect(x * 0.6f, y * 0.3f, x * 0.2f, y * 0.2f), "", Lite))
            {
                PlayerPrefs.DeleteAll();//delete
                Application.LoadLevel(0);//restart level
                Level = 1;//reset selected level
                PlayerShooting.GUN = 1;//reset selected gun
            }
            //Sounds
            if (GUI.Button(new Rect(x * 0.4f, y * 0.3f, x * 0.2f, y * 0.2f), "", Lite))
            {
                volume += 0.2f;
                if (volume > 1) volume = 0;

                Colorise();
                audio.PlayOneShot(select, volume);//play sound
                PlayerPrefs.SetFloat("sound", volume);//SAVE
            }
            //Joystick size
            if (GUI.Button(new Rect(x * 0.2f, y * 0.3f, x * 0.2f, y * 0.2f), "", Lite))
            {
                joystickSize += 0.05f;
                if (joystickSize > 0.5f) joystickSize = 0.2f;

                Colorise();
                audio.PlayOneShot(select, volume);//play sound
                PlayerPrefs.SetFloat("joystick", joystickSize);//SAVE
            }
        }
        if (slide == 2)
        {
            //GUNS
            if (GUI.Button(new Rect(x * 0.05f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price1)//Pistol
            {
                PlayerShooting.GUN = 1;//select 1 gun
                Colorise();//set buttons colors
                audio.PlayOneShot(select, volume);//play sound
            }
            if (GUI.Button(new Rect(x * 0.2f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price2)//AK
            {
                PlayerShooting.GUN = 2;//...
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.35f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price3)//Mashine gun
            {
                PlayerShooting.GUN = 3;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.5f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price4)//Snipe
            {
                PlayerShooting.GUN = 4;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.65f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price5)//Shotgun
            {
                PlayerShooting.GUN = 5;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.8f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & MONEY >= price6)//Crossbow
            {
                PlayerShooting.GUN = 6;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            /*if (GUI.Button(new Rect(x * 0.0f, y * 0.0f, x * 0.15f, y * 0.2f), "", Lite) & MONEY>=price7)//new gun select
            {
                PlayerShooting.GUN = 7;
                Colorise();
                audio.PlayOneShot(select, volume);
            }*/
        }
        if (slide == 3)
        {
            //LEVELS
            if (GUI.Button(new Rect(x * 0.05f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 1)
            {
                Level = 1;//select level 1
                Colorise();//set buttons colors
                audio.PlayOneShot(select, volume);//play sound
            }
            if (GUI.Button(new Rect(x * 0.2f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 2)
            {
                Level = 2;//...
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.35f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 3)
            {
                Level = 3;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.5f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 4)
            {
                Level = 4;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.65f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 5)
            {
                Level = 5;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            if (GUI.Button(new Rect(x * 0.8f, y * 0.3f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS >= 6)
            {
                Level = 6;
                Colorise();
                audio.PlayOneShot(select, volume);
            }
            /*if (GUI.Button(new Rect(x * 0.0f, y * 0.0f, x * 0.15f, y * 0.2f), "", Lite) & LEVELS <= 7)//new level select
            {
                Level = 7;
                Colorise();
                audio.PlayOneShot(select, volume);
            }*/
        }
    }
    void Colorise()
    {
        canvas1.SetActive(false);//deactivate all
        canvas2.SetActive(false);
        canvas3.SetActive(false);
        s1.color = new Color(0, 1, 0, 0.3f);//DARK GREEN tab selection buttons (not selected)
        s2.color = new Color(0, 1, 0, 0.3f);
        s3.color = new Color(0, 1, 0, 0.3f);
        if (slide == 1) { s1.color = new Color(0, 1, 0, 0.9f); canvas1.SetActive(true); lines.SetActive(false); }//settings screen
        if (slide == 2) { s2.color = new Color(0, 1, 0, 0.9f); canvas2.SetActive(true); lines.SetActive(true);//weapons screen
            selected.rectTransform.anchorMin = new Vector2(PlayerShooting.GUN * 0.15f - 0.1f, 0.41f);// (min pos) illuminate background of the selected weapon button
            selected.rectTransform.anchorMax = new Vector2(PlayerShooting.GUN * 0.15f + 0.05f, 0.7f);// (max pos)
        }
        if (slide == 3) { s3.color = new Color(0, 1, 0, 0.9f); canvas3.SetActive(true); lines.SetActive(true);//levels screen
            selected.rectTransform.anchorMin = new Vector2(Level * 0.15f - 0.1f, 0.41f);// (min pos) illuminate background of the selected level button
            selected.rectTransform.anchorMax = new Vector2(Level * 0.15f + 0.05f, 0.7f);// (max pos)
        }

        joy.rectTransform.localScale = new Vector2(joystickSize * 2, joystickSize * 2);//resize joystick button for demonstration

        if (volume == 0) vol.color = new Color(1, 0, 0, 0.3f);//change volume button color for demonstrate the volume
        else vol.color = new Color(0, 1, 0, volume);

        //DARK GREEN GUN (not selected)
        a1.color = new Color(0, 1, 0, 0.3f);//Red, Green, Blue, Alfa
        a2.color = new Color(0, 1, 0, 0.3f);
        a3.color = new Color(0, 1, 0, 0.3f);
        a4.color = new Color(0, 1, 0, 0.3f);
        a5.color = new Color(0, 1, 0, 0.3f);
        a6.color = new Color(0, 1, 0, 0.3f);
        //a7.color = new Color(0, 1, 0, 0.3f);//new gun

        //DARK GREEN LEVEL (not selected)
        L1.color = new Color(0, 1, 0, 0.3f);
        L2.color = new Color(0, 1, 0, 0.3f);
        L3.color = new Color(0, 1, 0, 0.3f);
        L4.color = new Color(0, 1, 0, 0.3f);
        L5.color = new Color(0, 1, 0, 0.3f);
        L6.color = new Color(0, 1, 0, 0.3f);
        //L7.color = new Color(0, 1, 0, 0.3f);//new level

        //RED GUN (locked)
        if (MONEY < price1) a1.color = new Color(1, 0, 0, 0.3f);
        if (MONEY < price2) a2.color = new Color(1, 0, 0, 0.3f);
        if (MONEY < price3) a3.color = new Color(1, 0, 0, 0.3f);
        if (MONEY < price4) a4.color = new Color(1, 0, 0, 0.3f);
        if (MONEY < price5) a5.color = new Color(1, 0, 0, 0.3f);
        if (MONEY < price6) a6.color = new Color(1, 0, 0, 0.3f);
        //if (MONEY < price7) a7.color = new Color(1, 0, 0, 0.3f);//new gun

        //RED LEVEL (locked)
        if (LEVELS < 1) L1.color = new Color(1, 0, 0, 0.3f);
        if (LEVELS < 2) L2.color = new Color(1, 0, 0, 0.3f);
        if (LEVELS < 3) L3.color = new Color(1, 0, 0, 0.3f);
        if (LEVELS < 4) L4.color = new Color(1, 0, 0, 0.3f);
        if (LEVELS < 5) L5.color = new Color(1, 0, 0, 0.3f);
        if (LEVELS < 6) L6.color = new Color(1, 0, 0, 0.3f);
        //if (LEVELS < 7) L7.color = new Color(1, 0, 0, 0.3f);//new level
        
        //GREEN GUN (selected)
        if (PlayerShooting.GUN == 1) a1.color = new Color(0, 1, 0, 1);
        if (PlayerShooting.GUN == 2) a2.color = new Color(0, 1, 0, 1);
        if (PlayerShooting.GUN == 3) a3.color = new Color(0, 1, 0, 1);
        if (PlayerShooting.GUN == 4) a4.color = new Color(0, 1, 0, 1);
        if (PlayerShooting.GUN == 5) a5.color = new Color(0, 1, 0, 1);
        if (PlayerShooting.GUN == 6) a6.color = new Color(0, 1, 0, 1);
        //if (PlayerShooting.GUN == 7) a7.color = new Color(0, 1, 0, 1);//new gun
        
        //GREEN LEVEL (selected)
        if (Level == 1) L1.color = new Color(0, 1, 0, 1);
        if (Level == 2) L2.color = new Color(0, 1, 0, 1);
        if (Level == 3) L3.color = new Color(0, 1, 0, 1);
        if (Level == 4) L4.color = new Color(0, 1, 0, 1);
        if (Level == 5) L5.color = new Color(0, 1, 0, 1);
        if (Level == 6) L6.color = new Color(0, 1, 0, 1);
        //if (Level == 7) L7.color = new Color(0, 1, 0, 1);//new level
    }
}
