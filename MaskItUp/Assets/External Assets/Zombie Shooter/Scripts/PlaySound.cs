using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {
    
    public float volume = 1;// (0-1)
    public AudioClip sound;

    private AudioSource audio;

    void Start () {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(sound, MainMenu.volume * volume);//play sound
    }
}
