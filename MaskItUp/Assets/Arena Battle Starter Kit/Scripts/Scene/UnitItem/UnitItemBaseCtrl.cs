using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItemBaseCtrl : MonoBehaviour
{
    /* prefabs */
    public GameObject Bullet;
    public GameObject AttackParticle;
    public GameObject KillParticle;
    public GameObject UnitItemUI;

    /* component refs */
    public UnitItemAIHelperCtrl aiHelper;
    public Animator Animator;
    public GameObject Model;
    public Transform AttackRangeComponent;
    public SphereCollider GrassDetectingCollider;
    public GameObject UIContainer;

    /* public vars */
    [HideInInspector]
    public int instanceId;
    public bool isOurTeam;
    public bool isKilled;

    public string playerName;
    public float speed = 2f;
    public float attackRange = 4f;

    public float healthPoints = 10;
    public float hitPoints = 2;
    public float reloadTime = 1;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public bool isInsideGrass;


    /* private vars */
    private UnitItemUICtrl _itemUI;
    private bool _isMoving;
    private Common.State _currentState;
    private bool _isCollidingWithObstacle;
    private Vector3 _collisionVector;
     

    public void SetData(int itemId, float posX, float posZ, bool isOurTeam)
    {
        this.isOurTeam = isOurTeam;

        this.SetPosition(posX, posZ);
        this.LookAt(new Vector3(0, 0, isOurTeam ? 1 : -1));
        this.SetState(Common.State.IDLE);

        this.StopAim();
        this.StopMove();

        this.isKilled = false;

        //Instantiate item ui
        this._itemUI =  Utils.CreateInstance(this.UnitItemUI, this.UIContainer, true).GetComponent<UnitItemUICtrl>();
        this._itemUI.transform.localPosition = Vector3.zero;

        //reducing radius of grass detecting collider of enemy
        if (!isOurTeam)
            this.GrassDetectingCollider.radius = 0.1f;
            
        this.currentHealth = this.healthPoints;
    }

    public void Respawn()
    {
        this.isKilled = false;
        this.currentHealth = healthPoints;
        this._itemUI.SetEnergyBarProgress(1.0f);
        this.transform.position = this.isOurTeam ? SceneController.instance.Stage.OurTeamPositions[1].transform.position : SceneController.instance.Stage.EnemyTeamPositions[1].transform.position;
    }

    private int _grassEnterCounter;
    public void OnUnitEnterGrass()
    {
        this._grassEnterCounter++;
        if (this._grassEnterCounter > 0)
            this.isInsideGrass = true;

        this.Refresh();
    }

    public void OnUnitExitGrass()
    {
        this._grassEnterCounter--;
        if (this._grassEnterCounter <= 0)
            this.isInsideGrass = false;

        this.Refresh();

    }

    public void Refresh()
    {
        //if(!this.isOurTeam)
        //{
        //    if(isInsideGrass)
        //    {
        //        this.HideUnit(true);
        //    }
        //    else
        //    {
        //        this.HideUnit(false);
        //    }
        //}
    }

    public void HideUnit(bool isTrue)
    {
        if(isTrue)
        {
            this.Model.SetActive(false);
            this.UIContainer.SetActive(false);
        }
        else
        {
            this.Model.SetActive(true);
            this.UIContainer.SetActive(true);
            this.SetState(Common.State.IDLE);
            this.SetState(Common.State.RUN);
        }
    }

    public void Kill()
    {
        this.isKilled = true;
        this.PlayKillParticle();
        this.Respawn();
    }

    public void SetState(Common.State state)
    {
        if (this.Animator == null)
            return;

        if (state == _currentState)
            return;

        if (state == Common.State.IDLE)
            this.Animator.SetTrigger("idle");
        else if (state == Common.State.RUN)
            this.Animator.SetTrigger("run");
        else if (state == Common.State.ATTACK)
        {
            this.Animator.SetTrigger("attack");
        }

        this._currentState = state;

        //Debug.Log(state);
    }

    public void SetPosition(float posX, float posZ)
    {
        this.transform.position = new Vector3(posX, 0, posZ);
    }

    public void LookAt(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Move(Vector3 moveVector)
    {
        //this is for slip unit when it collide with obstacle
        if(this._isCollidingWithObstacle)
        {
            float angleBetweenCollisionAndMoveVector = Vector3.Angle(this._collisionVector, moveVector);
            if(angleBetweenCollisionAndMoveVector > 120)
            {
                if (this._collisionVector.x == 0)
                {
                    moveVector.z = 0;
                    moveVector.x = Mathf.Sign(moveVector.x) * 1f;
                }
                else
                {
                    moveVector.z = Mathf.Sign(moveVector.z) * 1f;
                    moveVector.x = 0;
                }
            }
        }

        this.LookAt(moveVector);
        transform.Translate(moveVector * speed * Time.deltaTime, Space.World);

        this._aimDirectionVector = this.transform.forward;

        this.SetState(Common.State.RUN);
        this._isMoving = true;
    }

    public void StopMove()
    {
        if (this._isMoving)
        {
            this._isMoving = false;
            this.SetState(Common.State.IDLE);
        }
    }

    private Vector3 _aimDirectionVector = new Vector3(0,0,1);
    public void Aim(Vector3 directionVector)
    {
        this.AttackRangeComponent.gameObject.SetActive(true);
        _aimDirectionVector = directionVector;
        this.AttackRangeComponent.rotation = Quaternion.LookRotation(_aimDirectionVector);

        float distanceToObstacle = attackRange;
        RaycastHit hit;
        Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Obstacle"))
        {
            float distance = Vector3.Distance(hit.point, unitCenterPos);
            if (distance < distanceToObstacle)
                distanceToObstacle = distance;
        }

        this.AttackRangeComponent.transform.localScale = new Vector3(1, 1, distanceToObstacle);
    }

    public void StopAim()
    {
        this.AttackRangeComponent.gameObject.SetActive(false);
    }

    private float _lastShootedTime;
    public void Attack()
    {
        UnitItemBaseCtrl nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != null)
        {
            this._aimDirectionVector = (nearestEnemy.transform.position - this.transform.position).normalized;
            this.LookAt(this._aimDirectionVector);
        }

        this.SetState(Common.State.ATTACK);

        if (Time.time - this._lastShootedTime > reloadTime) //wait one minute between
        {
            this.ShootBullet();
            this._lastShootedTime = Time.time;
            this.StartCoroutine(_StopAttack());
        }
    }

    public UnitItemBaseCtrl GetNearestEnemy()
    {
        UnitItemBaseCtrl nearestEnemy = null;
        float nearestDistance = 7;
        for (int i = 0; i < 3; i++)
        {
            UnitItemBaseCtrl unit = this.isOurTeam ? SceneController.instance.Stage.enemyTeam[i] : SceneController.instance.Stage.ourTeam[i];
         
            RaycastHit hit;
            Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);
            Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Obstacle"))
                continue;

            float dist = Vector3.Distance(unit.transform.position, this.transform.position);
            if (unit.isInsideGrass && dist > 2)
                continue;

            if (dist < nearestDistance)
                nearestEnemy = unit;
        }
        return nearestEnemy;
    }

    private IEnumerator _StopAttack()
    {
        yield return new WaitForSeconds(0.5f);
        this.SetState(Common.State.IDLE);
    }

    public void ShootBullet()
    {
        float distanceToObstacle = attackRange;
        RaycastHit hit;
        Vector3 unitCenterPos = transform.position + new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(unitCenterPos, _aimDirectionVector);
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Obstacle"))
        {
            float distance = Vector3.Distance(hit.point, unitCenterPos);
            if (distance < distanceToObstacle)
                distanceToObstacle = distance;
        }

        GameObject bullet = Utils.CreateInstance(this.Bullet, SceneController.instance.gameObject, true);
        bullet.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
        bullet.GetComponent<BulletCtrl>().SetData(_aimDirectionVector.normalized, 30f, distanceToObstacle, this.hitPoints, this.isOurTeam);

        GameObject particle = SceneController.instance.PlayParticle(this.AttackParticle, this.transform.position + new Vector3(0, 0.5f, 0), 3);
        particle.transform.rotation = Quaternion.LookRotation(_aimDirectionVector);
        Destroy(particle, distanceToObstacle/30);
    }


    public void PlayKillParticle()
    {
        GameObject particle = SceneController.instance.PlayParticle(this.KillParticle, this.transform.position + new Vector3(0, 0.1f, 0), 3);
    }

    public void OnReceiveHit(float hitPoints)
    {
        this.currentHealth -= hitPoints;
        if (this.currentHealth <= 0)
            this.Kill();

        this._itemUI.SetEnergyBarProgress(this.currentHealth / this.healthPoints);
    }

    void OnCollisionExit(Collision collision)
    {
        this._isCollidingWithObstacle = false;
    }

    void OnCollisionStay(Collision collision)
    {
        this._isCollidingWithObstacle = true;
        this._collisionVector = collision.contacts[0].normal;
    }
}
