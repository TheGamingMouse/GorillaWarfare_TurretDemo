using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region Events

    public static event Action OnDamage;

    #endregion

    #region Variables
    
    [Header("Floats")]
    readonly float damage = 1f;
    readonly float despawnTime = 3f;

    [Header("Bools")]
    bool cooldown;

    #endregion

    #region StartUpdate
    
    void Awake()
    {
        StartCoroutine(DespawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = GameObject.FindWithTag("PlayerObject").GetComponent<PlayerHealth>().cooldown;
    }

    #endregion

    #region Methods
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerComp) && !cooldown)
        {
            playerComp.TakeDamage(damage);
            OnDamage?.Invoke();
        }
        Destroy(gameObject);
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    #endregion
}
