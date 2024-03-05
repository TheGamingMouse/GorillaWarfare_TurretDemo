using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    #region Events

    public static event Action OnDamage;

    #endregion
    
    #region Variables
    
    [Header("Ints")]
    readonly int damage = 1;

    [Header("Bools")]
    bool cooldown;

    #endregion

    #region StartUpdate

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().cooldown;
    }

    #endregion

    #region Methods

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerComp) && !cooldown)
        {
            playerComp.TakeDamage(damage);
            cooldown = true;
            OnDamage?.Invoke();
        }
    }

    #endregion
}
