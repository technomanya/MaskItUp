using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController pControl;

    public GameObject GameOverPanel;
    public GameObject WinText;
    public GameObject LoseText;

    [SerializeField] private int enemyCount;
    public int MaskedEnemy;

    private GameObject[] Enemies;


    void Awake()
    {
        pControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
        Enemies = pControl.Enemies;
        enemyCount = Enemies.Length;
        MaskedEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (MaskedEnemy == enemyCount)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            WinText.SetActive(true);
            LoseText.SetActive(false);
        }
        else
        {
            LoseText.SetActive(true);
            WinText.SetActive(false);
            foreach (var enemy in Enemies)
            {
                enemy.GetComponent<NavMeshAgent>().speed = 0;
            }
        }

        GameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
