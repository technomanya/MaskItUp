using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int lvlCount = 1;

    public List<Level> levels = new List<Level>();
    public string levelName;
    public int levelId;
    public GameObject[] lvlMaps;
    public GameObject[] playerPoses;
    public Level CurrLevel = new Level();

    public GameObject player;
    public NavMeshSurface navSurface;

    public bool isGameRun = false;

    public class Level
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public GameObject LvlMap { get; set; }
        public Vector3 PlayerPose { get; set; }
        public Vector3[] EnemyPoses { get; set; }
        public int EnemyCount { get; set; }
        
    }

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        //playerPoses = GameObject.FindGameObjectsWithTag("PlayerPose");
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < lvlCount; i++)
        {
            var tempLvl = new Level();
            tempLvl.Id = i;
            tempLvl.Name = "Level" + tempLvl.Id;
            switch (i)
            {
                case 0:
                    tempLvl.LvlMap = lvlMaps[0];
                    tempLvl.EnemyCount = 3;
                    break;
                case int n when n > 0 && n < 5 :
                    tempLvl.LvlMap = lvlMaps[1];
                    tempLvl.EnemyCount = UnityEngine.Random.Range(5, 7);
                    break;
                case int n when n >= 5 && n <10:
                    tempLvl.LvlMap = lvlMaps[2];
                    tempLvl.EnemyCount = UnityEngine.Random.Range(8,10);
                    break;
                case int n when n >= 5 && n < 10:
                    tempLvl.LvlMap = lvlMaps[3];
                    tempLvl.EnemyCount = UnityEngine.Random.Range(11, 13);
                    break;
                case int n when n >= 10 && n < 11:
                    tempLvl.LvlMap = lvlMaps[4];
                    tempLvl.EnemyCount = UnityEngine.Random.Range(13, 15);
                    break;

            }

            tempLvl.PlayerPose = playerPoses[i].transform.position;
            
            levels.Add(tempLvl);
            Debug.Log(levels.Count);
        }

        foreach (var item in levels)
        {
            Debug.Log(item.Id);
        }
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelName = PlayerPrefs.GetString("SavedLevel");
        }
        else
        {
            levelName = "Level0";
            PlayerPrefs.SetString("SavedLevel", levelName);
        }

        CurrLevel = levels.FirstOrDefault(x => x.Name == levelName);
        //foreach (var item in levels)
        //{
        //    if (item.Name == levelName)
        //    {
        //        CurrLevel = item;
        //        break;
        //    }
        //}
    }

    void Start()
    {
        isGameRun = false;
        //Begin();
    }
    
    void Update()
    {
        
    }

    public void Begin()
    {
        Debug.Log(CurrLevel.Name);
        foreach (var map in lvlMaps)
        {
            if(CurrLevel.LvlMap == map)
            {
                map.SetActive(true);
                Debug.Log(map.name);
            }
            else
            {
                map.SetActive(false);
            }
        }
        navSurface.BuildNavMesh();

        player.transform.position = CurrLevel.PlayerPose;
        isGameRun = true;
    }

    public void NewLevel()
    {
        var nId = CurrLevel.Id + 1;
        levelId = nId;
        levelName = "Level" + levelId;

        //CurrLevel = levels.FirstOrDefault(x => x.Id == nId);
        
        foreach (var item in levels)
        {
            if (item.Id == levelId)
            {
                Debug.Log("Yeni Level" + CurrLevel.Name);
                CurrLevel = item;
                
                break;
            }
        }
        PlayerPrefs.SetString("SavedLevel" ,levelName);
    }

    

}
