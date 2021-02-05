using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public static SceneController instance;

    /* prefabs */
    public GameObject[] UnitItems;

    /* component refs */
    public GameObject Design;
    public GameObject UnitsContainer;
    public GameObject PlayerSpawnPosition;
    public StageItemCtrl Stage;
    public GameObject ParticlesContainer;

    /* public vars */
    [HideInInspector]
    public UnitItemBaseCtrl player;

    /* private vars */
    private Dictionary<int, UnitItemBaseCtrl> _unitItemInstances;

    void Awake () {
        instance = this;
        Application.targetFrameRate = 60;

        //Ignore collision between players
        Physics.IgnoreLayerCollision(11, 11); //Layer 11 - Player


        this.Design.SetActive(false);
        this._unitItemInstances = new Dictionary<int, UnitItemBaseCtrl>();
    }

    private void Start()
    {

    }

    public void InitStage()
    {
        this.Stage.Init();
        UIController.instance.ShowGameOverlayWindow();
    }

    public void SetPlayerUnit(UnitItemBaseCtrl unit)
    {
        this.player = unit;
        CameraController.instance.target = this.player.transform;
        unit.aiHelper.enabled = false;
    }

    public UnitItemBaseCtrl AddUnitItem(int itemIndex, int instanceId, float posX, float posZ, bool isOurTeam)
    {
        UnitItemBaseCtrl instance = Utils.CreateInstance(this.UnitItems[itemIndex], this.UnitsContainer, true).GetComponent<UnitItemBaseCtrl>();

        if (instanceId == -1)
        {
            instanceId = this._GetUnusedInstanceId();
        }

        instance.instanceId = instanceId;
        this._unitItemInstances.Add(instanceId, instance);

        instance.SetData(itemIndex, posX, posZ, isOurTeam);
        instance.SetState(Common.State.IDLE);

        return instance;
    }

    private int _GetUnusedInstanceId()
    {
        int instanceId = Random.Range(10000, 99999);
        if (this._unitItemInstances.ContainsKey(instanceId))
        {
            return _GetUnusedInstanceId();
        }
        return instanceId;
    }

    public GameObject PlayParticle(GameObject  particle, Vector3 position, float time)
    {
        GameObject instance = Utils.CreateInstance(particle, this.ParticlesContainer, true);
        instance.transform.position = position;
        Destroy(instance, 3);
        return instance;
    }
}
