using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItemAIHelperCtrl : MonoBehaviour {

    /* private vars */
    private UnitItemBaseCtrl _parent;

    private UnitItemBaseCtrl _targetUnit;
    private Vector3 _targetPosition;
    private bool _targetFound;
    private Vector3 _moveVector;
    private bool _isCollidingWithObstacle;

    void Start () {
        this._parent = this.GetComponentInParent<UnitItemBaseCtrl>();
        this._targetFound = false;
    }
	
	void Update () {
        if (!_targetFound)
        {
            this.FindTarget();
        }
        else
        {
            if (this._targetUnit != null && Vector3.Distance(this._targetPosition, this.transform.position) < 5)
            {
                this._parent.StopMove();
                this._parent.Attack();
            }
            else if (this._targetUnit == null && Vector3.Distance(this._targetPosition, this.transform.position) < 1)
            {
                this._parent.StopMove();
            }
            else
            {
                if (this._targetUnit != null)
                    this._targetPosition = this._targetUnit.transform.position;

                if (!this._isCollidingWithObstacle)
                    this._moveVector = (this._targetPosition - this.transform.position).normalized;

                this._parent.Move(this._moveVector);
            }
        }
	}

    public void FindTarget()
    {
        int randomIndex = Random.Range(0, 3);

        this._targetUnit = this._parent.GetNearestEnemy();

        if(this._targetUnit == null)
            this._targetPosition = SceneController.instance.Stage.GrassPositions[Random.Range(0, SceneController.instance.Stage.GrassPositions.Length)].transform.position;
        else
            this._targetPosition = this._targetUnit.transform.position;

        this._moveVector = (this._targetPosition - this.transform.position).normalized;

        this._targetFound = true;
        this.StartCoroutine(_RepeatFindTargetAfterAWhile());
    }

    private IEnumerator _RepeatFindTargetAfterAWhile()
    {
        yield return new WaitForSeconds(Random.Range(2,4));
        this._targetFound = false;
    }

    void OnCollisionExit(Collision collision)
    {
        this._isCollidingWithObstacle = false;
    }

    void OnCollisionStay(Collision collision)
    {
        this._isCollidingWithObstacle = true;
    }

}
