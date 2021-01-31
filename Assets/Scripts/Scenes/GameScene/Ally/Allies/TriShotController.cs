﻿#region Imports
using UnityEngine;
#endregion
[RequireComponent(typeof(Shot))]
/// <summary>
/// TriShot keep following the player and it try to keep focus to attack the nearest enemies
/// </summary>
public class TriShotController : Minion
{
    #region Variables
    [Header("TriShot Settings")]
    private Shot shot;

    [Space]
    private Transform _player;
    [Space]
    private float refreshTargetCount;
    [Space]
    private float damageTimeCount;
    private bool canDamage;
    [Space]
    public float maxRangeSize = 2;
    public float minRangeSize = 4;
    #endregion
    #region Events
    private void Start()
    {
        Get(out shot);
        
        LoadMinion();
        //al inicio se carga target con el transform de este obj
        _player = TargetManager.GetPlayer();
    }
    private void Update()
    {
        if (UpdateMinion())
        {

            //if exist a target then...
            if (target != null)
            {


                // Target refresher
                if (Timer(ref refreshTargetCount, 1))
                {
                    UpdateTarget();
                }

                //Rotation & attack Refresh
                if (target != transform)
                {
                    // rotates to that target
                    rotation.LookTo(target.position);

                    AttackUpdate();
                }


                //Player following
                UpdatePlayerFollow();

            }
            else
            {
                UpdateTarget();
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (_player != null)
        {
            //Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(transform.position, character.range);

            //Gizmos.color = Color.white;
            //Gizmos.DrawWireSphere(_player.position, character.range / maxRangeSize);

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(_player.position, character.range / minRangeSize);
        }
    }
    #endregion
    #region Methods

    /// <summary>
    /// Updates the attack
    /// </summary>
    private void AttackUpdate()
    {
        if (IsInRange())
        {
            shot.ShotBullet(character);
        }
    }

    /// <summary>
    /// updates the enemy target
    /// </summary>
    private void UpdateTarget()
    {
        target = TargetManager.GetEnemy(transform);
    }

    /// <summary>
    /// Brings the ally to the player with a range
    /// </summary>
    private void UpdatePlayerFollow()
    {

        float distance = Vector3.Distance(transform.position, _player.position);
        
        //bool inRange = distance > character.range / maxRangeSize && (distance < character.range / minRangeSize);

        //si están por encima del rango se quedan tranquilos
        if (distance > character.range / minRangeSize )
        {

            //si está por encima del rango maximo entonces regresa a donde el player
            if (distance > character.range / maxRangeSize)
            {
                Vector3 direction = _player.position - transform.position;
                direction = Vector3.Normalize(direction);
                movement.Move(direction,character.speed);
            }
        }
        else
        {
            Vector3 direction = transform.position - _player.position ;
            //direction = direction.normalized;
            direction = Vector3.Normalize(direction);

            movement.Move(direction, character.speed);

            //si están dentro del player
        }

    }

    #endregion
}
//Tri - Shot: Es una figura “Aéreo-levitando” que disparará cerca de tí, este te seguirá y procurará estar cerca. Escoge un enemigo y envía a veces triángulos, siendo este su ataque de un rango medio.