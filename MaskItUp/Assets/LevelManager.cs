using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public string levelName;

    public class Level
    {
        public string Name { get; set; }
        public string LvlMap { get; set; }
        public Vector3[] LvlPoses { get; set; }
        public int EnemyCount { get; }
        
    }

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelName = PlayerPrefs.GetString("SavedLevel");
        }
        else
        {
            levelName = "Level0";
            PlayerPrefs.SetString("SavedLevel",levelName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
