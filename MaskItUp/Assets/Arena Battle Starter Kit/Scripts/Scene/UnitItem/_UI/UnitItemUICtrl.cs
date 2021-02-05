using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitItemUICtrl : MonoBehaviour {

    /* component refs */
    public SpriteRenderer BottomCircle;
    public Transform NavigationCircle;
    public TextMeshPro NameText;

    public SpriteRenderer EnergyFiller;

    /* private vars */
    private UnitItemBaseCtrl _parent;
    private Color _energyFillerDefaultColor;
    private float _energyFillerFullWidth;

    private void Start()
    {
        this._parent = this.GetComponentInParent<UnitItemBaseCtrl>();

        this.BottomCircle.color = this._parent.isOurTeam ? Color.green : Color.red;
        this.NameText.text = this._parent.playerName;

        if (this._parent == SceneController.instance.player)
            this.EnergyFiller.color = Color.yellow;

        this.NavigationCircle.gameObject.SetActive(this._parent == SceneController.instance.player);

        this._energyFillerDefaultColor = this.EnergyFiller.color;
        this._energyFillerFullWidth = this.EnergyFiller.size.x;
    }

    private void Update()
    {
        if (this._parent == SceneController.instance.player)
        {
            Vector2 direction = GameOverlayWindowCtrl.instance.NavigatorJoystickComponent.Direction;
            this.NavigationCircle.position = (this.transform.position + new Vector3(direction.x, 0, direction.y).normalized);
        }
    }

    public void SetEnergyBarProgress(float val)
    {
        this.EnergyFiller.size = new Vector2(this._energyFillerFullWidth * val, this.EnergyFiller.size.y);
        if (val < 0.5f)
            this.EnergyFiller.color = Color.red;
        else
            this.EnergyFiller.color = _energyFillerDefaultColor;
    }
}
