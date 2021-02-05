using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    private bool _isOurTeam;
    private float _hitPoints;
    private float _velocity;
    private Vector3 _moveDirection;
    private float _maxDistance;

    private bool _isStarted;

    public void SetData(Vector3 moveDirection, float velocity, float maxDistance, float hitPoints, bool isOurTeam)
    {
        this._moveDirection = moveDirection;
        this._velocity = velocity;
        this._maxDistance = maxDistance;
        this._hitPoints = hitPoints;
        this._isOurTeam = isOurTeam;

        this._isStarted = true; 
        Destroy(this.gameObject, maxDistance / velocity);
    }

    private void Update()
    {
        if (!this._isStarted)
            return;

        this.transform.Translate(this._moveDirection * this._velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitItemBaseCtrl unit = other.GetComponent<UnitItemBaseCtrl>();
        if(unit != null && unit.isOurTeam != _isOurTeam)
        {
            unit.OnReceiveHit(this._hitPoints);
            Destroy(this.gameObject);
        }
    }
}
