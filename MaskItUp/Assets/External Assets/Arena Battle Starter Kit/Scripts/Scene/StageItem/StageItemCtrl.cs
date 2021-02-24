using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItemCtrl : MonoBehaviour {

    /* component refs */
    public GameObject[] OurTeamPositions;
    public GameObject[] EnemyTeamPositions;
    public GameObject[] GrassPositions;

    /* public vars */
    public UnitItemBaseCtrl[] ourTeam;
    public UnitItemBaseCtrl[] enemyTeam;

    void Start () {

	}

    public void Init()
    {
        this.SpawnOurTeam();
        this.SpawnEnemyTeam();
    }

    public void SpawnOurTeam()
    {
        this.ourTeam = new UnitItemBaseCtrl[3];
        int randomIndex = Random.Range(0, OurTeamPositions.Length);

        for (int i = 0; i < OurTeamPositions.Length; i++)
        {
            UnitItemBaseCtrl unit = SceneController.instance.AddUnitItem(i, -1, this.OurTeamPositions[i].transform.position.x, this.OurTeamPositions[i].transform.position.z, true);
            if (i == randomIndex)
                SceneController.instance.SetPlayerUnit(unit);

            this.ourTeam[i] = unit;
        }
    }

    public void SpawnEnemyTeam()
    {
        this.enemyTeam = new UnitItemBaseCtrl[3];

        for (int i = 0; i < EnemyTeamPositions.Length; i++)
        {
            UnitItemBaseCtrl unit = SceneController.instance.AddUnitItem(i, -1, this.EnemyTeamPositions[i].transform.position.x, this.EnemyTeamPositions[i].transform.position.z, false);
            this.enemyTeam[i] = unit;
        }
    }

}
